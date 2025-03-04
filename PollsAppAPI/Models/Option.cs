using System.ComponentModel.DataAnnotations.Schema;

namespace PollsApp.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int PollId { get; set; }
        public Poll Poll { get; set; }

        [NotMapped]
        public int VotesCount { get; set; }
    }
}
