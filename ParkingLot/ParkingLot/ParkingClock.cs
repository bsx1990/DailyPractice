using System;
using System.Timers;

namespace ParkingLot
{
    public class ParkingClock
    {
        public DateTime CurrentTime { get; set; }

        public ParkingClock(DateTime time)
        {
            CurrentTime = time;
            var timer = new Timer {Enabled = true, Interval = 60*1000};
            timer.Start();
            timer.Elapsed += UpdateTime;
        }

        private void UpdateTime(object sender, ElapsedEventArgs e)
        {
            CurrentTime = CurrentTime.AddMinutes(1);
        }

        public void SetTime(DateTime time)
        {
            CurrentTime = time;
            //Todo
        }

        public DateTime GetTime()
        {
            return CurrentTime;
        }
    }
}