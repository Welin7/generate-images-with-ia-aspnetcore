namespace ProjectOpenAi.Dtos
{
    public class GameImageResponseModel
    {
        public long Created { get; set; }
        public List<GameImageDataModel> Data { get; set; } = new List<GameImageDataModel>();
    }
}
