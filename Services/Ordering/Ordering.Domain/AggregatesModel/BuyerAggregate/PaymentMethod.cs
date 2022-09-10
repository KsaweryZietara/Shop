using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate {

    public class PaymentMethod : Entity {
        private string? _alias;
        private string? _cardNumber;
        private string? _securityNumber;
        private string? _cardHolderName;
        private DateTime _expiration;

        private int _cardTypeId;
        public CardType CardType { get; private set; }

        protected PaymentMethod() { }

        public PaymentMethod(int cardTypeId, 
                             string alias,
                             string cardNumber,
                             string securityNumber,
                             string cardHolderName,
                             DateTime expiration) {

            _cardNumber = !string.IsNullOrEmpty(cardNumber) ? cardNumber : throw new OrderDomainException(nameof(cardNumber));
            _securityNumber = !string.IsNullOrEmpty(securityNumber) ? securityNumber : throw new OrderDomainExeption(nameof(securityNumber));
            _cardHolderName = !string.IsNullOrEmpty(cardHolderName) ? cardHolderName : throw new OrderDomainException(nameof(cardHolderName));

            if(expiration < DateTime.UtcNow) {
                throw new OrderDomainException(nameof(expiration));
            } 

            _alias = alias;
            _expiration = expiration;
            _cardTypeId = cardTypeId;
        }

        public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration) {
            return _cardTypeId == cardTypeId 
                && _cardNumber == cardNumber
                && _expiration == expiration;
        }
    }
}