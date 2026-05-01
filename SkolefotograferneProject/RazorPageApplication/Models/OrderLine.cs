using RazorPageApplication.Enums;

namespace RazorPageApplication.Models
{
    public class OrderLine
    {
        #region Properties
        public int Id { get; set; }
        public PhotoType PhotoType { get; set; }
        public PhotoColorFormat PhotoColorFormat { get; set; }
        public PhotoDimensions PhotoDimensions { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }


        public double TotalPrice
        {
            get { return Price * Amount; }
        } 
        #endregion

        #region Constructors
        public OrderLine()
        {

        }

        public OrderLine(int id, PhotoType photoType, PhotoColorFormat photoColorFormat, PhotoDimensions photoDimensions, double price, int amount)
        {
            Id = id;
            PhotoColorFormat = photoColorFormat;
            PhotoDimensions = photoDimensions;
            Price = price;
            Amount = amount;
        } 
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Orderline: \n\tID: {Id}\n\tPhoto type: {PhotoType}\n\tPhoto Color Format: {PhotoColorFormat}\n\t" +
                $"Photo Dimensions: {PhotoDimensions}\n\tPrice: {Price}\n\tAmount: {Amount}";
        } 
        #endregion
    }
}
