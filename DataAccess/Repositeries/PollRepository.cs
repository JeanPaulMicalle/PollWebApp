using DataAccess.Context;
using Domain.Models;

namespace DataAccess.Repositeries
{
    public interface IPollRepository
    {
        void CreatePoll(string title, string option1Text, string option2Text, string option3Text);
        IQueryable<Poll> GetPolls();
        void Vote(int pollId, int optionNumber);
    }

    public class PollRepository : IPollRepository
    {
        private readonly PollDbContext _context;

        public PollRepository(PollDbContext context)
        {
            _context = context;
        }

        // CreatePoll creates a new Poll and saves it in the database.
        public void CreatePoll(string title, string option1Text, string option2Text, string option3Text)
        {
            var newPoll = new Poll
            {
                Title = title,
                Option1Text = option1Text,
                Option2Text = option2Text,
                Option3Text = option3Text,
                Option1VotesCount = 0,
                Option2VotesCount = 0,
                Option3VotesCount = 0,
                DateCreated = DateTime.Now
            };

            _context.Polls.Add(newPoll);
            _context.SaveChanges();
        }

        // Returns all pols as an IQueryable for efficiency.
        public IQueryable<Poll> GetPolls()
        {
            return _context.Polls;
        }

        // vote method updates the vote count for the selected option.
        public void Vote(int pollId, int optionNumber)
        {
            // Find the poll using the provided pollId.
            var poll = _context.Polls.FirstOrDefault(p => p.PollId == pollId);
            if (poll != null)
            {
                // Increment the appropriate option's vote count.
                switch (optionNumber)
                {
                    case 1:
                        poll.Option1VotesCount++;
                        break;
                    case 2:
                        poll.Option2VotesCount++;
                        break;
                    case 3:
                        poll.Option3VotesCount++;
                        break;
                    default:
                        throw new ArgumentException("Invalid option number", nameof(optionNumber));
                }
                // Save the changes to the database.
                _context.SaveChanges();
            }

        }
    }
}