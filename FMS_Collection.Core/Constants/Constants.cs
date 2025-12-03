namespace FMS_Collection.Core.Constants
{
    public class Constants
    {
        public struct Messages
        {
            public const string TransactionRecordsFetched = "Transaction Records Fetched successfully";
            public const string GenericError = "An error occurred";
            public const string GenericErrorWithActual = "An error occurred. actual error is :{0}";
            public const string RetrieveAssetsError = "An error occurred while retrieving assets.";
            public const string RetrieveAssetDetailsError = "An error occurred while retrieving asset details.";
            public const string AddAssetError = "An error occurred while adding the asset.";
            public const string UpdateAssetError = "An error occurred while updating the asset.";
            public const string InvalidUserOrPassword = "Invalid user or password.";
            public const string RetrieveTransactionsError = "An error occurred while retrieving Transactions.";

            // Assets
            public const string AssetsFetchedSuccessfully = "Assets fetched successfully";
            public const string AssetDetailsFetchedSuccessfully = "Asset details fetched successfully";
            public const string AssetCreatedSuccessfully = "Asset created successfully";
            public const string AssetDeletedSuccessfully = "Asset deleted successfully";
            public const string AssetSavedSuccessfully = "Asset saved successfully";

            // Notifications
            public const string NotificationsFetchedSuccessfully = "Notifications fetched successfully";
            public const string NotificationListFetchedSuccessfully = "Notification list fetched successfully";
            public const string NotificationDetailsFetchedSuccessfully = "Notification details fetched successfully";
            public const string NotificationCreatedSuccessfully = "Notification created successfully";
            public const string NotificationUpdatedSuccessfully = "Notification updated successfully";
            public const string NotificationDeletedSuccessfully = "Notification deleted successfully";
            public const string NotificationSummaryFetchedSuccessfully = "Notification summary fetched successfully";

            // Users
            public const string UsersFetchedSuccessfully = "Users fetched successfully";
            public const string UserListFetchedSuccessfully = "User list fetched successfully";
            public const string UserDetailsFetchedSuccessfully = "User details fetched successfully";
            public const string UserSavedSuccessfully = "User saved successfully";
            public const string UserDeletedSuccessfully = "User deleted successfully";
            public const string UserPermissionsUpdatedSuccessfully = "User permissions updated successfully";
            public const string UserPermissionsFetchedSuccessfully = "User permissions fetched successfully";
            public const string ModulesFetchedSuccessfully = "Modules fetched successfully";
            public const string LoginSuccessful = "Login successful";
            public const string TooManyLoginAttempts = "Too many failed login attempt done, you are locked for 24 hours.";

            // Common Lists
            public const string CommonListsFetchedSuccessfully = "Common lists fetched successfully";
            public const string CommonListItemsFetchedSuccessfully = "Common list items fetched successfully";
            public const string CommonListDetailsFetchedSuccessfully = "Common list details fetched successfully";
            public const string CommonListItemDetailsFetchedSuccessfully = "Common list item details fetched successfully";
            public const string CommonListCreatedSuccessfully = "Common list created successfully";
            public const string CommonListItemCreatedSuccessfully = "Common list item created successfully";
            public const string CommonListUpdatedSuccessfully = "Common list updated successfully";
            public const string CommonListItemUpdatedSuccessfully = "Common list item updated successfully";
            public const string CommonListDeletedSuccessfully = "Common list deleted successfully";
            public const string CommonListItemDeletedSuccessfully = "Common list item deleted successfully";
            public const string CountriesFetchedSuccessfully = "Countries fetched successfully";

            // Settings/Configurations
            public const string ConfigurationsFetchedSuccessfully = "Configurations fetched successfully";
            public const string ActiveConfigurationsFetchedSuccessfully = "Active configurations fetched successfully";
            public const string ConfigurationDetailsFetchedSuccessfully = "Configuration details fetched successfully";
            public const string ConfigurationCreatedSuccessfully = "Configuration created successfully";
            public const string ConfigurationUpdatedSuccessfully = "Configuration updated successfully";
            public const string ConfigurationDeletedSuccessfully = "Configuration deleted successfully";
            public const string ConfigurationDeactivatedSuccessfully = "Configuration deactivated successfully";
            public const string AccountsFetchedSuccessfully = "Accounts fetched successfully";
            public const string RelationsFetchedSuccessfully = "Relations fetched successfully";
            public const string OccasionTypesFetchedSuccessfully = "Occasion types fetched successfully";

            // Transactions
            public const string TransactionsFetchedSuccessfully = "Transactions fetched successfully";
            public const string TransactionListFetchedSuccessfully = "Transaction list fetched successfully";
            public const string TransactionSummaryFetchedSuccessfully = "Transaction summary fetched successfully";
            public const string BalanceSummaryFetchedSuccessfully = "Balance summary fetched successfully";
            public const string TransactionReportFetchedSuccessfully = "Transaction report fetched successfully";
            public const string TransactionDetailsFetchedSuccessfully = "Transaction details fetched successfully";
            public const string TransactionSuggestionsFetchedSuccessfully = "Transaction suggestions fetched successfully";
            public const string TransactionCreatedSuccessfully = "Transaction created successfully";
            public const string TransactionUpdatedSuccessfully = "Transaction updated successfully";
            public const string TransactionDeletedSuccessfully = "Transaction deleted successfully";

            // Coin/Note Collections
            public const string CoinNoteCollectionsFetchedSuccessfully = "Coin/note collections fetched successfully";
            public const string CoinNoteCollectionListFetchedSuccessfully = "Coin/note collection list fetched successfully";
            public const string CoinNoteCollectionDetailsFetchedSuccessfully = "Coin/note collection details fetched successfully";
            public const string CoinNoteCollectionCreatedSuccessfully = "Coin/note collection created successfully";
            public const string CoinNoteCollectionUpdatedSuccessfully = "Coin/note collection updated successfully";
            public const string CoinNoteCollectionDeletedSuccessfully = "Coin/note collection deleted successfully";
            public const string CoinNoteCollectionSummaryFetchedSuccessfully = "Coin/note collection summary fetched successfully";

            // Documents
            public const string DocumentFetchedSuccessfully = "Document fetched successfully";
            public const string DocumentListFetchedSuccessfully = "Document list fetched successfully";
            public const string DocumentDetailsFetchedSuccessfully = "Document details fetched successfully";
            public const string DocumentCreatedSuccessfully = "Document created successfully";
            public const string DocumentUpdatedSuccessfully = "Document updated successfully";
            public const string DocumentDeletedSuccessfully = "Document deleted successfully";
            public const string DocumentummaryFetchedSuccessfully = "Document summary fetched successfully";

            public const string IssueInCoinDeletionNoteCollection = "Facing issue in deletion of this data";

            // Special Occasions (Days)
            public const string DaysFetchedSuccessfully = "Days fetched successfully";
            public const string DayListFetchedSuccessfully = "Day list fetched successfully";
            public const string DayDetailsFetchedSuccessfully = "Day details fetched successfully";
            public const string DayCreatedSuccessfully = "Day created successfully";
            public const string DayUpdatedSuccessfully = "Day updated successfully";
            public const string DayDeletedSuccessfully = "Day deleted successfully";

            // OTP
            public const string OtpSentSuccessfully = "OTP sent successfully";
            public const string WelcomeInviteSentSuccessfully = "Welcome invite sent successfully";
            public const string OtpVerifiedSuccessfully = "OTP verified successfully";
            public const string OtpInvalidOrExpired = "OTP is invalid or expired";
            public const string PasswordResetSuccessful = "Password reset successful";
            public const string OtpResentSuccessfully = "OTP resent successfully";
            public const string OtpGenerationFailed = "Unable to generate OTP";
            public const string OtpDeliveryFailed = "Unable to send OTP";
        }
        public struct Roles
        {
            public const string None = "None";
            public const string SuperAdmin = "Super Admin";
            public const string SystemAdmin = "System Admin";
            public const string GroupAdmin = "Group Admin";
            public const string Supervisor = "Supervisor";
            public const string Person = "Person";
        }

        public struct OtpPurpose
        {
            public const string PasswordReset = "password_reset";
            public const string Login = "Login";
        }
        public struct Permissions
        {
            public const string None = "None";
            //public const string GroupSetup = "GROUP_SETUP";

            public const string ViewCollection = "VIEW_COLLECTION";
            public const string AddCollection = "ADD_COLLECTION";
            public const string UpdateCollection = "UPDATE_COLLECTION";
            public const string DeleteCollection = "DELETE_COLLECTION";
            public const string ManageCollection = "MANAGE_COLLECTION";
            public const string ViewDay = "VIEW_DAY";
            public const string AddDay = "ADD_DAY";
            public const string UpdateDay = "UPDATE_DAY";
            public const string DeleteDay = "DELETE_DAY";
            public const string ManageDay = "MANAGE_DAY";
            //public const string ImportDay = "IMPORT_DAY";
            public const string ManageExpense = "MANAGE_EXPENSE";
            //public const string ImportExpense = "IMPORT_EXPENSE";
            //public const string ViewUser = "VIEW_USER";
            //public const string AddUser = "ADD_USER";
            //public const string UpdateUser = "UPDATE_USER";
            //public const string DeleteUser = "DELETE_USER";
            //public const string BlockUser = "BLOCK_USER";
            //public const string UnblockUser = "UNBLOCK_USER";
            public const string ManageUser = "MANAGE_USER";
            public const string ManageRole = "MANAGE_ROLE";
            public const string ManageSetting = "MANAGE_SETTING";
            public const string ViewNotification = "VIEW_NOTIFICATION";
            public const string ManageGlobalSetting = "MANAGE_GLOBAL_SETTING";

        }

        public struct DocumentType
        {
            public const string BIRTHDAY_PERSON_PIC = "Birthday_Person_Pic";
            public const string COLLECTION_COINS = "Collection_Coins";
            public const string USER_PROFILE_PICTURE = "User_Profile_Picture";
            public const string THUMBNAILS = "/thumbnails/";
            public const string DOCUMENTS = "documents";
        }

        public struct CacheKeys
        {
            public const string APP_TOKEN = "data:apptoken";
            public const string APPSETTINGS_List_KEY = "data:{0}:appsettingLists";
            public const string GROUPCONFIG_List_KEY = "data:{0}groupConfigLists";
            public const string APPLICATION_USER_DETAIL = "data:userToken:{0}:applicationUserDetail";
            public const string NOTIFICATION_TEMPLATES = "data:{0}:notificationTemplates";
        }

        public struct AppMenus
        {
            public const string HOME = "HOME";
            public const string GROUP = "GROUP";
            public const string COLLECTION = "COLLECTION";
            public const string PERSON = "PERSON";
            public const string DAYS = "DAYS";
            public const string SETTINGS = "SETTINGS";
            public const string NOTIFICATION = "NOTIFICATION";
            public const string USER = "USER";
            public const string EXPENSE = "EXPENSE";
        }

        public struct CommonFiles
        {
            public const string DayFormat = "DayFormat";
            public const string PersonFormat = "PersonFormat";
            public const string LeaveFormat = "LeaveFormat";
            public const string ExpenseTemplate = "ExpenseTemplate";
        }


        public static readonly HashSet<string> ImageContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpg",
            "image/jpeg",
            "image/pjpeg",
            "image/png",
            "image/x-png",
            "image/gif",
            "image/bmp",
            "image/webp",
            "image/tiff",
            "image/svg+xml",
            "image/x-icon"
        };

        public static readonly HashSet<string> ImageExtensionList = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".bmp",
            ".webp",
            ".tiff",
            ".svg",
            ".ico"
        };

        public struct EncryptionDecryptionKeys
        {
            public const string PasswordHash = "P@@Sw0rd";
            public const string SaltKey = "S@LT&KEY";
            public const string VIKey = "@1B2c3D4e5F6g7H8";
        }

        public struct UserConfig
        {
            public const string ACCOUNT = "Account";
            public const string RELATION = "Relation";
            public const string OCCASION_TYPE = "OccasionType";
        }

    }
}
