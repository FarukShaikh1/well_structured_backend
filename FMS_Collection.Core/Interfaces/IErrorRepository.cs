namespace FMS_Collection.Core.Interfaces
{
    public interface IErrorRepository
    {
        Task AddErrorLog(Exception ex,
        string? parameters = null,
        Guid? loggedBy = null);
    }

}


