
using Newtonsoft.Json;

namespace DemoElasticKibana.Models
{
    public class RabbitMQMessage<T> where T : class
    {
        public RabbitMQMessage(T data, string type)
        {
            Data = data;
            Type = type;
        }

        public T Data { get; set; }
        public string Type { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
