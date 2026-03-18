namespace VoteApi.Models
{
    public class VoteItem
    {
        public int Id { get; set; }

        public string Topic { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public List<VoteOption> VoteOptions { get; set; } = new List<VoteOption>();

    }
}
