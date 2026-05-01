using RazorPageApplication.Enums;

namespace RazorPageApplication.Models
{
    public class OrderLine
    {
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

        public override string ToString()
        {
            return $"Order line: \n\tID: {Id}\n\tPhoto type: {PhotoType}\n\tPhoto Color Format: {PhotoColorFormat}\n\t" +
                $"Photo Dimensions: {PhotoDimensions}\n\tPrice: {Price}\n\tAmount: {Amount}";
        }
    }
}
