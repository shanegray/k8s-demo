using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace ShareMe.Carefully.Rabbit
{
    public class RabbitMessenger
    {
        private readonly RabbitHandler rabbitHandler;

        public RabbitMessenger(RabbitHandler rabbitHandler)
        {
            this.rabbitHandler = rabbitHandler;
        }

        public void SendMessage<T>(string exchange, T message, string routingKey = "*")
        {
            using (var channel = this.rabbitHandler.CreateChannel())
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var data = JsonConvert.SerializeObject(message, settings);
                var body = Encoding.UTF8.GetBytes(data);

                var properties = channel.CreateBasicProperties();
                properties.ContentType = "application/json";
                properties.ContentEncoding = "utf8";                
                channel.BasicPublish(exchange, routingKey, true, properties, body);
            }
        }
    }
}
