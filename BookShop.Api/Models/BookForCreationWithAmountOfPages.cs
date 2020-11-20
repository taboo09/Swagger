namespace BookShop.Api.Models
{
    public class BookForCreationWithAmountOfPages : BookForCreation
    { 
        public int AmountOfPages { get; set; }
    }
}