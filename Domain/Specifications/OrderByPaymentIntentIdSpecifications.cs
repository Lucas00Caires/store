

using Domain.Model.Entities.OrderAggregate;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class OrderByPaymentIntentIdSpecifications : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecifications(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
