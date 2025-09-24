namespace FMS_Collection.Core.Constants
{
    public class Constants
    {
        public struct Roles
        {
            public const string None = "None";
            public const string SuperAdmin = "Super Admin";
            public const string SystemAdmin = "System Admin";
            public const string GroupAdmin = "Group Admin";
            public const string Supervisor = "Supervisor";
            public const string Person = "Person";
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
            public const string THUMBNAILS = "thumbnails\\";
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
