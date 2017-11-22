namespace Common
{
    public class OrderLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public OrderLine(int id, string description, int quantity) 
        {
            Id = id;
            Description = description;
            Quantity = quantity;
        }
    }
}