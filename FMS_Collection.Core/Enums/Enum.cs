namespace FMS_Collection.Core.Enum
{
    public enum Role
    {
        None = 0,
        SuperAdmin = 1,
        GroupAdmin = 2,
        Supervisor = 3,
        Person = 4
    }
    public enum Gender
    {
        Male = 77,   // ASCII value of 'M'
        Female = 70, // ASCII value of 'F'
        Couple = 67, // ASCII value of 'C'
        Other = 79   // ASCII value of 'O'
    }

    public enum EntityAction
    {
        View = 1,
        Save = 2,
        Update = 3,
        Delete = 4,
        Login = 5,
        Start = 6,
        End = 7,
        Sent = 8,
        AlreadyExecutedJob = 9
    }

    public enum NotificationPriority
    {
        Normal = 0,
        High = 1,
        Medium = 2,
        Low = 3
    }

    public enum AssetType
    {
        Image,
        Document
    }

    public enum FileType
    {
        Original = 1,
        Thumbnail = 2,
        Preview = 3,
    }
}
