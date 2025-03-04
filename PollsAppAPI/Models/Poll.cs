namespace PollsApp.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
