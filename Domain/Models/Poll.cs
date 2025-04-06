namespace Domain.Models
{
    public class Poll
    {
        public int PollId { get; set; }

        public required string Title { get; set; }

        public required string Option1Text { get; set; }
        public required string Option2Text { get; set; }

        public required string Option3Text { get; set; }

        public int Option1VotesCount { get; set; } = 0;
        public int Option2VotesCount { get; set; } = 0;
        public int Option3VotesCount { get; set; } = 0;

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
