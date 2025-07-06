using Microsoft.AspNetCore.Mvc;
using OpenAI.Images;
using ProjectOpenAi.Dtos;

namespace ProjectOpenAi.Controllers
{
    [ApiController]
    [Route("api/game-creator")]
    public class GamesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GamesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("OpenAI");
            _apiKey = configuration["OpenAI:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "API key cannot be null.");
        }

        [HttpPost("generate-image")]
        public async Task<IActionResult> GenerateImage(GameImageRequestModel gameImageRequestModel)
        {
            object request;
            if (!string.IsNullOrWhiteSpace(gameImageRequestModel.background))
            {
                request = new
                {
                    prompt = gameImageRequestModel.prompt,
                    model = "gpt-image-1",
                    n = gameImageRequestModel.n,
                    size = gameImageRequestModel.size,
                    quality = gameImageRequestModel.quality,
                    background = gameImageRequestModel.background,
                    output_format = gameImageRequestModel.outputFormat
                };
            }
            else
            {
                request = new
                {
                    prompt = gameImageRequestModel.prompt,
                    model = "dall-e-3",
                    n = gameImageRequestModel.n,
                    size = gameImageRequestModel.size,
                    quality = gameImageRequestModel.quality
                };
            }

            var response = await _httpClient.PostAsJsonAsync("images/generations", request);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, responseContent);
            }

            var result = await response.Content.ReadFromJsonAsync<GameImageResponseModel>();
            return Ok(result);
        }

        [HttpPost("generate-image-library")]
        public async Task<IActionResult> GenerateImageSdkOpenAI(GameImageRequestModel gameImageRequestModel)
        {
            var client = new ImageClient("dall-e-3", _apiKey);
            
            var options = new ImageGenerationOptions
            {
                Size = GeneratedImageSize.W1024xH1024,                               
                Quality = GeneratedImageQuality.Standard,
                Style = GeneratedImageStyle.Vivid,
                ResponseFormat = GeneratedImageFormat.Uri
            };

            var response = await client.GenerateImageAsync(gameImageRequestModel.prompt, options);
            var image = response.Value;

            return Ok(image);
        }
    }
}
