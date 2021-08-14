using System;
using System.Diagnostics;
using System.IO;

namespace SoloVova.Helpers.HDDControl{
    public class HddControl{
        public void GetAllHDD(){
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives){
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true){
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
        }

        public void GetFilteredHDD(){
            const long plotSize = 109000000000;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives){
                if (d.DriveType != DriveType.Fixed){
                    continue;
                }
                string result = $"Drive {d.Name} ";
                if (d.IsReady){
                    if (!d.VolumeLabel.StartsWith("D")){
                        continue;
                    }
                    result += $"Volume label: {d.VolumeLabel,-7} ";
                    result += $"Available space: {d.TotalFreeSpace,15} bytes ";

                    long plotsFree = d.TotalFreeSpace / plotSize;
                    if (plotsFree > 0){
                        result += $"Is place for: {plotsFree,4}";
                    }
                }
                else{
                    result += " not ready";
                }
                Console.WriteLine(result);
            }
        }

        private long WriteToDriveTimer(string nameDrive){
            const string strForWrite = "HDD wake up";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (FileStream fStream = new FileStream($"{nameDrive}wakeup.txt", FileMode.OpenOrCreate)){
                byte[] array = System.Text.Encoding.Default.GetBytes(strForWrite);
                fStream.Write(array, 0, array.Length);
            }

            stopwatch.Stop();
            return stopwatch.ElapsedTicks;
        }

        private string WriteToDrive(DriveInfo dInfo){
            string result = $"{DateTime.Now} Drive {dInfo.Name}";
            if (dInfo.IsReady){
                if (!dInfo.VolumeLabel.StartsWith("D")){
                    return "";
                }
                result += "     ready ";
            }
            else{
                result += " not ready ";
            }
            result += $" {WriteToDriveTimer(dInfo.Name)}\n";
            return result;
        }

        private void WriteToLog(string str){
            const string logFileName = "wakeup_log.txt";
            FileMode modOpen = File.Exists(logFileName) ? FileMode.Append : FileMode.Create;


            using (FileStream fStream = new(logFileName, modOpen)){
                byte[] array = System.Text.Encoding.Default.GetBytes(str);
                fStream.Write(array, 0, array.Length);
            }
        }

        public void WriteCycle(){
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives){
                if (d.DriveType != DriveType.Fixed){
                    continue;
                }
                string res = this.WriteToDrive(d);
                if (!string.IsNullOrEmpty(res)){
                    Console.WriteLine(res);
                    this.WriteToLog(res);
                }
            }
        }
    }
}