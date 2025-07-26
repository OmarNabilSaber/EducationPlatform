namespace OnlineEducationPlatform.Web.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Text { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int Points { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public List<string>? Options { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? CorrectAnswer { get; set; }
    }
}
