using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderShippedDomainEvent : INotification {
    public Order Order { get; private set; }

    public OrderShippedDomainEvent(Order order) {
        Order = order;
    }
}