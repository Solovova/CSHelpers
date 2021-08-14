using System;

namespace SoloVova.Helpers.HDDControl{
    internal static class Program{
        static void Main(string[] args){
            // HddControl hddControl = new HddControl();
            // hddControl.GetFilteredHDD();
            // string? p = Console.ReadLine();
            HddTimer hddTimer = new HddTimer();
            hddTimer.Start();
        }
    }
}