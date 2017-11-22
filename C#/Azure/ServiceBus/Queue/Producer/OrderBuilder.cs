using System.Collections.Generic;
using Common;

namespace Producer
{
    public static class OrderBuilder
    {
        public static Order RandomOrder()
        {
            var random = new System.Random();

            return new Order(random.Next(), random.Next(), GetOrdersLine());
        }

        private static List<OrderLine> GetOrdersLine()
        {
            var random = new System.Random();
            int numerOfOrdersLine = random.Next(1, 5);

            List<OrderLine> ordersLine = new List<OrderLine>();

            for (var i = 0; i < numerOfOrdersLine; i++)
            {
                ordersLine.Add(new OrderLine(i, $"Description {i}", random.Next()));
            }

            return ordersLine;
        }
    }
}