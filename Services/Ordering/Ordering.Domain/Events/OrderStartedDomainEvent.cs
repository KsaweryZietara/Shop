using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events {
    public class OrderStartedDomainEvent : INotification {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public int CardTypeId { get; private set; }
        public string CardNumber { get; private set; }
        public string CardSecurityNumber { get; private set; }
        public string CardHolderName { get; private set; }
        public DateTime CardExpiration { get; private set; }
        public Order Order { get; private set; }

        public OrderStartedDomainEvent(Order order, 
                                       string userId, 
                                       string userName, 
                                       int cardTypeId, 
                                       string cardNumber,
                                       string cardSecurityNumber,
                                       string cardHolderName, 
                                       DateTime cardExpiration) {
            
            Order = order;
            UserId = userId;
            UserName = userName;
            CardTypeId = cardTypeId;
            CardNumber = cardNumber;
            CardSecurityNumber = cardSecurityNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
        }
    }    
}