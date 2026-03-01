using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Text;
using System.Text.RegularExpressions;
using Tesseract;

namespace FMS_Collection.Application.Services
{
    public class CoinNoteCollectionService
    {
        private readonly ICoinNoteCollectionRepository _repository;
        private readonly AzureBlobService _blobService;
        private readonly AssetService _assetService;
        public CoinNoteCollectionService(ICoinNoteCollectionRepository repository, AzureBlobService blobService, AssetService assetService)
        {
            _repository = repository;
            _blobService = blobService;
            _assetService = assetService;
        }

        public async Task<ServiceResponse<List<CoinNoteCollection>>> GetAllCoinNoteCollectionsAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionsFetchedSuccessfully
            );
        }// => 
        public async Task<ServiceResponse<List<CoinNoteCollectionListResponse>>> GetCoinNoteCollectionListAsync(Guid userId)
        {
            // Fetch data from repository
            var response = await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCoinNoteCollectionListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionListFetchedSuccessfully
            );

            // Null or empty check
            if (response?.Data == null || !response.Data.Any())
                return response;

            // Replace ImagePath and ThumbnailPath with Blob SAS URLs
            foreach (var item in response.Data)
            {
                if (!string.IsNullOrEmpty(item.ImagePath))
                {
                    item.ImagePathSasUrl = _blobService.GetBlobSasUrl(item.ImagePath);
                }

                if (!string.IsNullOrEmpty(item.ThumbnailPath))
                {
                    item.ThumbnailPathSasUrl = _blobService.GetBlobSasUrl(item.ThumbnailPath);
                }
            }

            return response;
        }

        public async Task<ServiceResponse<CoinNoteCollectionDetailsResponse>> GetCoinNoteCollectionDetailsAsync(Guid coinNoteCollectionId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionDetailsFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<Guid>> AddCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection, Guid coinNoteCollectionId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(CoinNoteCollection, coinNoteCollectionId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionCreatedSuccessfully
            );
        }

        public async Task<ServiceResponse<bool>> UpdateCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection, Guid coinNoteCollectionId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateAsync(CoinNoteCollection, coinNoteCollectionId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionUpdatedSuccessfully
            );
        }

        public async Task<int> UpdateCoinAIData()
        {
            var userId = Guid.TryParse("C3D0A1D1-78F3-4128-8C22-C394AD7F55E5", out var userId1);
            var coinList = await GetCoinNoteCollectionListAsync(userId1);
            int count = 0;
            string extractedText;
            foreach (var coin in coinList.Data)
            {
                string imagePath = coin.ImagePathSasUrl;
                string currencyLanguage = coin.CurrencyLanguages;
                if (String.IsNullOrEmpty(coin.ExtractedText))
                {
                    extractedText = await ExtractTextFromImageUrlByTessData(imagePath, currencyLanguage);
                    if (String.IsNullOrEmpty(extractedText))
                    {
                        extractedText = await ExtractTextFromImageUrlByComputerVisionAI(imagePath);
                    }
                    extractedText = NormalizeOcrText(extractedText);
                    if (String.IsNullOrEmpty(extractedText))
                    {
                        continue;
                    }
                    count++;
                }
                else
                {
                    continue;
                }
                string description = "";// await GenerateGPTDescription(prompt);
                decimal estimatedValue = 0;// EstimateValue(coin);
                decimal predictedFutureValue = estimatedValue * 1.15m;
                float confidence = 0.78f;

                await _repository.UpdateCoinAIData(
                    coin.Id,
                    extractedText,
                    description,
                    estimatedValue,
                    predictedFutureValue,
                    confidence
                );
            }
            return count;
        }

        public string NormalizeOcrText(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
                return string.Empty;

            // Normalize Unicode
            rawText = rawText.Normalize(NormalizationForm.FormKC);

            // Replace newlines & tabs
            rawText = rawText.Replace("\n", " ")
                             .Replace("\t", " ");

            // Remove OCR junk but keep Arabic + numbers
            rawText = Regex.Replace(rawText, @"[^\u0600-\u06FF0-9\s]", "");

            // Remove extra spaces
            rawText = Regex.Replace(rawText, @"\s{2,}", " ");

            return rawText.Trim();
        }

        public async Task<string> ExtractTextFromImageUrlByTessData(string imageUrl, string currencyLanguage)
        {
            try
            {
                // IMPORTANT: This must be the folder containing the .traineddata files
                string tessDataPath = @"..\tessdata-main";

                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = Timeout.InfiniteTimeSpan;  // Remove timeout limit

                    // Download image from Azure Blob URL
                    byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    // Load image directly from memory (no need to save file)
                    //string languages ="eng+fra+deu+spa+ita+por+rus+ara+tur+fas+hin+ben+guj+mar+tam+kan+tel+urd+chi_sim+chi_tra+jpn+kor+tha+vie+khm";
                    string languages = currencyLanguage;

                    using (var img = Pix.LoadFromMemory(imageBytes))
                    {
                        using (var engine = new TesseractEngine(tessDataPath, languages, EngineMode.Default))
                        {
                            using (var page = engine.Process(img))
                            {
                                string text = page.GetText();
                                return text ?? "";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> ExtractTextFromImageUrlByComputerVisionAI(string imageUrl)
        {
            try
            {
                string endpoint = AppSettings.AzureVision_Endpoint;
                string key = AppSettings.AzureVision_Key;
                var credentials = new ApiKeyServiceClientCredentials(key);
                var client = new ComputerVisionClient(credentials)
                {
                    Endpoint = endpoint
                };

                // STEP 1: Submit image for OCR
                ReadHeaders headers = await client.ReadAsync(imageUrl);

                // STEP 2: Extract operation ID from response headers
                string operationLocation = headers.OperationLocation;
                string operationId = operationLocation.Split('/').Last();

                ReadOperationResult readResult;

                // STEP 3: Poll for result
                do
                {
                    await Task.Delay(1000);
                    readResult = await client.GetReadResultAsync(Guid.Parse(operationId));
                }
                while (
                    readResult.Status == OperationStatusCodes.Running ||
                    readResult.Status == OperationStatusCodes.NotStarted
                );

                if (readResult.Status != OperationStatusCodes.Succeeded)
                    return "";

                // STEP 4: Read text
                var sb = new StringBuilder();

                foreach (var page in readResult.AnalyzeResult.ReadResults)
                {
                    foreach (var line in page.Lines)
                    {
                        sb.AppendLine(line.Text);
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ServiceResponse<bool>> DeleteCoinNoteCollectionAsync(Guid coinNoteCollectionId, Guid userId)
        {
            // first delete assets related to the selected coin/note
            CoinNoteCollectionDetailsResponse coinDetails = await _repository.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, userId);
            var response = await _assetService.DeleteAssetAsync(coinDetails.AssetId, userId);
            if (response == null)
            {
                return await ServiceExecutor.ExecuteAsync(
                () => null,
                FMS_Collection.Core.Constants.Constants.Messages.IssueInCoinDeletionNoteCollection
            );
            }
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(coinNoteCollectionId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionDeletedSuccessfully
            );
        }

        public async Task<ServiceResponse<List<CoinNoteCollectionSummaryResponse>>> GetSummaryAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetSummaryAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionSummaryFetchedSuccessfully
            );
        }

    }
}
