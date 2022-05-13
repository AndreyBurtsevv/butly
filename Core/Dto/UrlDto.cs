namespace Bitly.Core.Dto
{
    public class UrlDto
    {
        public int Id { get; set; }

        public string FullUrl { get; set; }

        public string Key { get; set; }
    }

    public class CreateUrlDto
    {
        public string FullUrl { get; set; }
    }
}
