using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events {
    public class OrderCancelledDomainEvent : INotification {
        public Order Order { get; private set; }

        public OrderCancelledDomainEvent(Order order) {
            Order = order;
        }
    }
}