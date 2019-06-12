using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace ShareMe.Carefully.Rabbit
{
    public class RabbitHandler : IDisposable
    {
        private readonly ConnectionFactory factory;
        private IConnection connection;
        private readonly List<RabbitListener> listeners = new List<RabbitListener>();

        public RabbitHandler(string uri)
        {
            this.factory = new ConnectionFactory
            {
                Uri = new Uri(uri),
                DispatchConsumersAsync = true
            };
        }

        public void Startup()
        {
            this.connection = this.factory.CreateConnection();
        }

        public void AddListener(RabbitListener listener)
        {
            listener.Connect(this.connection.CreateModel());
            this.listeners.Add(listener);
        }

        public void AddListeners(params RabbitListener[] listeners)
        {
            foreach(var listener in listeners)
            {
                AddListener(listener);
            }
        }

        public IModel CreateChannel()
        {
            return this.connection.CreateModel();
        }

        public void ShutDown()
        {
            this.listeners.ForEach(l => l.Close());

            if (this.connection.IsOpen) this.connection.Close();
            this.connection.Dispose();
        }

        public void Dispose()
        {
            this.ShutDown();
        }
    }
}
