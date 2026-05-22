using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Order : IIdAble
    {
        #region Properties
        //Properties til at gemme og sæt værdier ind
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
        public Parent Parent { get; set; }

        public List<OrderLine> OrderLines = new List<OrderLine>();
        #endregion

        //Constructor er til når man laver en ny instans og skal enten give den alle argumenterne eller intet
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


        //til at kunne printe den enkelte objekts infomation
        #region Methods
        public override string ToString()
        {
            return $"Order: \n\tID: {Id}\n\tDate: {Date}\n\tTotal Price: {TotalPrice}\n\tParent: {Parent.Name}\n\t";
        } 
        #endregion
    }
}
