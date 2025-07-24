namespace OnlineEducationPlatform.Web.ViewModels
{
    public class ExamListItemViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public bool HasSubmitted { get; set; }
        public decimal? Score { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
    }
}
