namespace AWSsnsApi.Models
{
    public class SnsMessage
    {
        public string TopicArn { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
