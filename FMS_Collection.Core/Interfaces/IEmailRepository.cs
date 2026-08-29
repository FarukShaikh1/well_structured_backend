using YourProject.Core.Entities;

public interface IEmailRepository
{
    Task<EmailTemplate?> GetByCodeAsync(
        string templateCode,
        CancellationToken cancellationToken = default);

    Task<List<EmailTemplate>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<EmailTemplate?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}