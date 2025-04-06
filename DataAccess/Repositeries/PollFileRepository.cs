using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositeries
{
    public class PollFileRepository : IPollRepository
    {
        // File path for the JSON file (adjust the path as needed).
        private readonly string _filePath = "polls.json";

        // Creates a new poll and saves it to the JSON file.
        public void CreatePoll(string title, string option1Text, string option2Text, string option3Text)
        {
            // Retrieve existing polls (or an empty list if none exist).
            List<Poll> polls = GetPolls().ToList();

            int newPollId = polls.Any() ? polls.Max(p => p.PollId) + 1 : 1;

            Poll newPoll = new Poll
            {
                PollId = newPollId,
                Title = title,
                Option1Text = option1Text,
                Option2Text = option2Text,
                Option3Text = option3Text,
                Option1VotesCount = 0,
                Option2VotesCount = 0,
                Option3VotesCount = 0,
                DateCreated = DateTime.Now
            };

            // Add the new poll to the collection.
            polls.Add(newPoll);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(polls, options);

            // Write the JSON string to the file.
            File.WriteAllText(_filePath, jsonString);
        }

        // Retrieves all pols from the JSON file as an IQueryable.
        public IQueryable<Poll> GetPolls()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Poll>().AsQueryable();
            }

            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return new List<Poll>().AsQueryable();
            }

            List<Poll> polls = JsonSerializer.Deserialize<List<Poll>>(jsonString) ?? new List<Poll>();

            // Return the list as IQueryable for efficient querying.
            return polls.AsQueryable();
        }

        // Implement Vote in the file-based repository.
        public void Vote(int pollId, int optionNumber)
        {
            // Retrieve current polls from the file.
            List<Poll> polls = GetPolls().ToList();

            var poll = polls.FirstOrDefault(p => p.PollId == pollId);
            if (poll == null)
            {
                throw new ArgumentException("Invalid poll id");
            }

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

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(polls, options);
            File.WriteAllText(_filePath, jsonString);
        }
    }
}
