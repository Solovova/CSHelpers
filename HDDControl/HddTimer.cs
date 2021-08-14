using System;
using System.Timers;

namespace SoloVova.Helpers.HDDControl{
    public class HddTimer{
        private System.Timers.Timer? _timer;
        private HddControl _hddControl = new HddControl();
        
        
        
        private void SetTimer()
        {
            _timer = new System.Timers.Timer(30000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            this._hddControl.WriteCycle();
        }

        public void Start(){
            this.SetTimer();
            while (true){
                System.Threading.Thread.Sleep(2000);
            }
            
            _timer?.Stop();
            _timer?.Dispose();
            
        }
    }
}