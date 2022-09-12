using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events {
    public class OrderStatusChangedToAwaitingValidationDomainEvent : INotification {
        public int OrderId { get; private set; }
        public IEnumerable<OrderItem> OrderItems { get; private set; }

        public OrderStatusChangedToAwaitingValidationDomainEvent(int orderId, IEnumerable<OrderItem> orderItems) {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }
}