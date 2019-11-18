using Domain.Entities.Items;


namespace Domain.Entities.Extensions
{
  public static class ItemExtension
    {
        // use date service to calculte time elasped from completed time
        public static string GetStatus(this Item item)
        {
            string status;

            status = item.isCompleted ? "Completed" : "Incomplete";
            
            return status;
        }
    }
}
