namespace API.Requests.Video
{
    public class VideoUpdateRequest
    {
        public string Prompt { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Seconds { get; set; }

        public int Fps { get; set; }
    }
}
