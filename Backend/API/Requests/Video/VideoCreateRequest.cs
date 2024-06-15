namespace API.Requests.Video
{
    public class VideoCreateRequest
    {
        public int CategoryId { get; set; }

        public string Prompt { get; set; }

        public string Tittle { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Seconds { get; set; }

        public int Fps { get; set; }
    }
}
