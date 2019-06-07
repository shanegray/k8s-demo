using System;

namespace DriverService.Events
{
    public class TimedEvent
    {
        public string DriverId { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }
}
