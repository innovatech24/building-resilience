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
        // Run python script that returns a sentiment score of a given text
        public float getSentimentScore(string text)
        {
            string res = run_cmd("Scripts/py/sentiment_analysis.py", text);
            return (float.Parse(res));
        }

        // Run python script that returns a set of sentences representing the most positive and negative
        public string getSentimentMinMax(string text)
        {
            string res = run_cmd("Scripts/py/sentiment_analysis2.py", text);
            return (res); //Json format string
        }

        // Run python script returning a set of datasets
        public string getDatasets()
        {
            string res = run_cmd("Scripts/py/load_datasets.py", AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/"));
            return (res); //Json format string
        }

        // Function to run a command in the command prompt
        private string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = findPy();
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

        // Function to find python in the machine.
        public string findPy()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"cmd.exe";
            start.CreateNoWindow = true;
            start.Arguments = "/C where python";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    //Console.Write(result);
                    //process.WaitForExit();
                    result = @"C:\users\kiran\anaconda3\python.exe";
                    //result = @"C:\users\amulya\anaconda3\python.exe";
                    return result;
                }
            }
        }

    }
}