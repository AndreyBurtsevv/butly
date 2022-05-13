namespace Bitly.Core.Data.Entities
{
    public class Url
    {
        public int Id { get; set; }

        public string FullUrl { get; set; }

        public string Key { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
