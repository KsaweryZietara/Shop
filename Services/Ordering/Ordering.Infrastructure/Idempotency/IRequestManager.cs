namespace Ordering.Infrastructure.Idempotency {
    public interface IRequestManager {
        Task<bool> ExitAsync(Guid id);
        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}