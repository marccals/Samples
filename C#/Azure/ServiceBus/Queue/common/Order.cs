using System.Collections.Generic;

namespace Common
{    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<OrderLine> OrdersLine { get; set; }

        public Order(int id, int userId, IEnumerable<OrderLine> ordersLine) 
        {
            Id = id;
            UserId = userId;
            OrdersLine = ordersLine;
        }
    }
}