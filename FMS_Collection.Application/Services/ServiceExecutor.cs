using FMS_Collection.Core.Common;

namespace FMS_Collection.Application.Services
{
    public static class ServiceExecutor
    {
        public static async Task<ServiceResponse<T>> ExecuteAsync<T>(Func<Task<T>> action, string successMessage)
        {
            var response = new ServiceResponse<T>();
            try
            {
                var data = await action();
                response.Success = true;
                response.Data = data;
                response.Message = successMessage;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = default;
                response.Message = ex.Message;
            }
            return response;
        }

        public static async Task<ServiceResponse<bool>> ExecuteAsync(Func<Task> action, string successMessage)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                await action();
                response.Success = true;
                response.Data = true;
                response.Message = successMessage;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}


