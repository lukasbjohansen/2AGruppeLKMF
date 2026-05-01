namespace RazorPageApplication.Models
{
    public class Order
    {
        #region Properties
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
        public Parent Parent { get; set; }
        #endregion

        #region constructor
        public Order()
        {
            
        }

        public Order(int id, DateTime date, double totalPrice, Parent parent)
        {
            Id = id;
            Date = date;
            TotalPrice = totalPrice;
            Parent = parent;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Order: \n\tID: {Id}\n\tDate: {Date}\n\tTotal Price: {TotalPrice}\n\tParent: {Parent.Name}";
        } 
        #endregion
    }
}
