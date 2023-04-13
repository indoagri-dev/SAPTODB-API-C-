using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IndoAgri.SapToDB.Api.Libs
{
    public class Common
    {
        public static void Log(string fileName, string msg)
        {
            using (StreamWriter w = File.AppendText(fileName))
            {
                WriteToFile(msg, w);
               
            }
        }

        public static void WriteToFile(string logMessage, TextWriter w )
        {
           // using (StreamWriter w = File.AppendText(fileName))
            {
                w.Write("\r\nLog : ");
                w.WriteLine(DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());
                w.WriteLine("  :");
                w.WriteLine(logMessage);
                w.WriteLine("-------------------------------");
            }

        }
    }
}