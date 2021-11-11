using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WeatherForecast
{
    class Logger
    {
        private string _fileDirectory;
        private readonly object _obj = new object();

        public Logger(string filedirectory)
        {
            _fileDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public bool WriteLog(string details)
        {
            var result = false;

            try
            {
                lock (_obj)
                {

                    var directory = Path.Combine(_fileDirectory + "Logs", DateTime.Now.ToString("yyyy-MM-dd") + "_debug" + ".log");

                    var logDetails = new StringBuilder();

                    if (!Directory.Exists(_fileDirectory + "Logs"))
                    {
                        Directory.CreateDirectory(_fileDirectory + "Logs");
                    }

                    if (!File.Exists(directory))
                    {
                        File.Create(directory).Dispose();
                    }

                    using (StreamWriter file = File.AppendText(directory))
                    {
                        file.WriteLine(details);
                        file.Flush();
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }
    }
}
