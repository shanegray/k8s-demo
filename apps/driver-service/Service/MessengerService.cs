using DriverService.Events;
using ShareMe.Carefully.Rabbit;

namespace DriverService.Service
{
    public class MessengerService
    {
        private readonly RabbitMessenger rabbitMessenger;

        public MessengerService(RabbitMessenger rabbitMessenger)
        {
            this.rabbitMessenger = rabbitMessenger;
        }

        public void DriverHiredMessage(HiredEvent hiredEvent)
        {
            this.rabbitMessenger.SendMessage("driver.hired", hiredEvent);
        }

        public void StatusUpdate(TimedEvent timedEvent, string key)
        {
            this.rabbitMessenger.SendMessage("driver.status-update", timedEvent, key);
        }
    }
}
