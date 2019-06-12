using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ShareMe.Carefully.Rabbit
{
    public abstract class GenericListener<T> : RabbitListener
    {
        public GenericListener(string queue) : base(queue) { }

        public override Task<bool> HandleMessage(string message)
        {
            var converted = JsonConvert.DeserializeObject<T>(message);
            return this.HandleMessage(converted);
        }

        public abstract Task<bool> HandleMessage(T message);
    }
}
