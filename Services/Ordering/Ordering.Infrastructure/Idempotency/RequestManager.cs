using Ordering.Domain.Exceptions;

namespace Ordering.Infrastructure.Idempotency {

    public class RequestManager : IRequestManager {
        private readonly OrderingContext context;
        
        public RequestManager(OrderingContext context) {
            this.context = context;
        }

        public async Task<bool> ExitAsync(Guid id) {
            var request = await context.FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id) {
            var exists = await ExitAsync(id);

            var request = exists ? 
                throw new OrderingDomainException($"Request with {id} already exists") :
                new ClientRequest() {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };
            
            context.Add(request);

            await context.SaveChangesAsync();
        }
    }
}