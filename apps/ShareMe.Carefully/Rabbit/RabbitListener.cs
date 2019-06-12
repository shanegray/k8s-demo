using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ShareMe.Carefully.Rabbit
{
    public abstract class RabbitListener
    {
        public string QueueName { get; }
        private IModel channel;

        public RabbitListener(string queueName)
        {
            this.QueueName = queueName;
        }

        public void Connect(IModel channel)
        {
            this.channel = channel;
            this.channel.QueueDeclarePassive(this.QueueName);

            var consumer = new AsyncEventingBasicConsumer(this.channel);
            consumer.Received += Message_ReceivedAsync;
            this.channel.BasicConsume(this.QueueName, false, consumer);
        }

        private async Task Message_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);

            // TODO : Log

            try
            {
                var success = await this.HandleMessage(message);
                if (success)
                    this.channel.BasicAck(e.DeliveryTag, false);
                else
                    this.channel.BasicReject(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                // TODO : Log
                this.channel.BasicReject(e.DeliveryTag, false);
                throw;
            }
        }

        public void Close()
        {
            if (this.channel.IsOpen) this.channel.Close();
            this.channel.Dispose();
        }

        public abstract Task<bool> HandleMessage(string message);
    }
}
