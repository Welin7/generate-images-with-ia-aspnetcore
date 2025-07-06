namespace ProjectOpenAi.Dtos
{
    public class GameImageRequestModel
    {
        public string prompt { get; set; } = string.Empty;
        public int n { get; set; }
        public string size { get; set; } = string.Empty;
        public string quality { get; set; } = string.Empty;
        public string background { get; set; } = string.Empty;        
        public string outputFormat { get; set; } = string.Empty;
    }
}
