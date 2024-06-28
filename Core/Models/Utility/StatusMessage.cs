using Newtonsoft.Json;

namespace Core.Models.Utility
{
    public class StatusMessage
    {
        public StatusMessage()
        {
        }

        public StatusMessage(string? message, bool isSuccess = true)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        public string? Message { get; set; }
        public bool IsSuccess { get; set; }

        public string ToJSon()
        {
            return JsonConvert.SerializeObject(new StatusMessage(Message, IsSuccess));
        }
        public static StatusMessage? FromJSon(string json)
        {
            return JsonConvert.DeserializeObject<StatusMessage>(json);

        }
    }
}
