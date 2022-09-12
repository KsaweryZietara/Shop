using Ordering.Domain.Events;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate {
    
    public class Buyer : Entity, IAggregateRoot {
        public string? IdentityGuid { get; private set; }

        public string? Name { get; private set; }

        private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        public Buyer(string identity, string name) {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        public PaymentMethod VerifyOrAddPaymentMethod(int cardTypeId, 
                                                    string alias, 
                                                    string cardNumber,
                                                    string securityNumber, 
                                                    string cardHolderName, 
                                                    DateTime expiration, 
                                                    int orderId) {
        
            var existingPayment = _paymentMethods
                .SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

            if(existingPayment != null) {
                AddDomainEvent(new BuyerPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

                return existingPayment;
            }

            var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

            _paymentMethods.Add(payment);

            AddDomainEvent(new BuyerPaymentMethodVerifiedDomainEvent(this, payment, orderId));

            return payment;
        }
    }
}