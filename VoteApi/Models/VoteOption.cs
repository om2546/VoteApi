namespace VoteApi.Models
{
    public class VoteOption
    {
        public int Id { get; set; }

        public string Option { get; set; } = string.Empty;

        public int VoteCount { get; set; }

        public int VoteItemId { get; set; }
        public VoteItem? VoteItem { get; set; }

    }
}
