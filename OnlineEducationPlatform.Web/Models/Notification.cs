namespace OnlineEducationPlatform.Web.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static void SendNotification(Infrastructure.Data.ApplicationDbContext context, string userId, string message, string title = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Title = title,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            context.Notifications.Add(notification);
            
        }
    }
}
