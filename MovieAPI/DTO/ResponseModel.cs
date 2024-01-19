namespace MovieAPI.DTO;

public abstract class ResponseModelDto
{
    public class ResponseModel<T>
    {
        [JsonProperty("statusCode")] public int StatusCode { get; set; }

        [JsonProperty("statusMessage")] public string? StatusMessage { get; set; }

        [JsonProperty("result")] public T? Result { get; set; }
    }
}