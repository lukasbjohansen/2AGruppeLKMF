using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Order : IIdAble
    {
        #region Properties
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
        public Parent Parent { get; set; }

        public List<OrderLine> OrderLines = new List<OrderLine>();
        #endregion

        #region constructor
        public Order()
        {
            
        }

        public Order(int id, DateTime date, double totalPrice, Parent parent, List<OrderLine> orderLines)
        {
            Id = id;
            Date = date;
            TotalPrice = totalPrice;
            Parent = parent;
            OrderLines = orderLines;

        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Order: \n\tID: {Id}\n\tDate: {Date}\n\tTotal Price: {TotalPrice}\n\tParent: {Parent.Name}\n\t";
        } 
        #endregion
    }
}
