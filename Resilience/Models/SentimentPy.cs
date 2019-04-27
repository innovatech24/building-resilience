using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Resilience.Models
{
    public class SentimentPy
    {

        public float getSentimentScore(string text)
        {
            string res = run_cmd("Scripts/py/sentiment_analysis.py", text);
            return (float.Parse(res));
        }

        public string getSentimentMinMax(string text)
        {
            string res = run_cmd("Scripts/py/sentiment_analysis2.py", text);
            return (res); //Json format string
        }

        private string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Program Files (x86)\Python37-32\python.exe";
            start.CreateNoWindow = true;
            start.Arguments = string.Format("{0} \"{1}\"", AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/") + cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    //Console.Write(result);
                    //process.WaitForExit();
                    return result;
                }
            }
        }
    }
}
