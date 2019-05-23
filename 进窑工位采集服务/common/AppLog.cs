using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace 进窑工位采集服务.common
{
    public class AppLog
    {
        public static void Write(string content)
        {
            try
            {
                string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
                bool flag = !Directory.Exists(text);
                if (flag)
                {
                    Directory.CreateDirectory(text);
                }
                StreamWriter streamWriter = new StreamWriter(Path.Combine(text, DateTime.Now.ToString("yyyyMMdd") + ".log"), true, Encoding.UTF8);
                streamWriter.WriteLine(DateTime.Now.ToString() + "\t" + content);
                streamWriter.WriteLine("------------------------------------------------------------------------------");
                streamWriter.Close();
            }
            catch
            {
            }
        }
        public static void WriteErr(string content)
        {
            try
            {
                string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
                bool flag = !Directory.Exists(text);
                if (flag)
                {
                    Directory.CreateDirectory(text);
                }
                StreamWriter streamWriter = new StreamWriter(Path.Combine(text, DateTime.Now.ToString("yyyyMMdd") + "Err.log"), true, Encoding.UTF8);
                streamWriter.WriteLine(DateTime.Now.ToString() + "\t" + content);
                streamWriter.WriteLine("------------------------------------------------------------------------------");
                streamWriter.Close();
            }
            catch
            {
            }
        }

    }
}
