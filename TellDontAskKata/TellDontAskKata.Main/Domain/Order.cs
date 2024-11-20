using System.Collections.Generic;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public decimal Total { get; set; }
        public string Currency { get; set; }
        public IList<OrderItem> Items { get; set; }
        public decimal Tax { get; set; }
        public OrderStatus Status { get; set; }
        public int Id { get; set; }

        public void Ship()
        {
            if (Status is OrderStatus.Created or OrderStatus.Rejected)
            {
                throw new OrderCannotBeShippedException();
            }

            if (Status is OrderStatus.Shipped)
            {
                throw new OrderCannotBeShippedTwiceException();
            }

            Status = OrderStatus.Shipped;
        }
    }
}