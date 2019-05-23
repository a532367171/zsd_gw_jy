using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using 进窑工位采集服务.common;
using System.Threading;
using System.IO;

namespace 进窑工位采集服务.ModBus
{
    internal class SocketWrapper : IDisposable
    {
        private static string IP = ConfigurationManager.AppSettings["进窑工控机IP"];
        private static int Port = Int32.Parse(ConfigurationManager.AppSettings["进窑工控机Port"]);
        private static int TimeOut = Int32.Parse(ConfigurationManager.AppSettings["SocketTimeOut"]);

        public ILog Logger { get; set; }
        private Socket socket = null;

        public void Connect()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, TimeOut);

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(IP), Port);



            try
            {
                this.socket.Connect(ip);

            }
            catch (Exception ex)
            {
                int i = 0;
                while (true)
                {
                    try
                    {
                        Thread.Sleep(3000);
                        this.socket.Connect(ip);
                    }
                    catch
                    {
                        #region 错误日记
                        AppLog.WriteErr(ex.Message);
                        #endregion
                        i++;
                        if (i > 5)
                        {
                            System.Diagnostics.Process.Start("D:\\管片工位采集服务(中水电)\\进窑工位采集\\进窑工位采集服务\\进窑工位采集服务\\bin\\Debug\\进窑工位采集服务.exe", "5");
                        }
                        continue;
                    }
                    break;
                }
            }


        }




        public byte[] Read(int length)
        {
            byte[] data = new byte[length];
            this.socket.Receive(data);
            this.Log("Receive:", data);
            return data;
        }

        public void Write(byte[] data)
        {
            this.Log("Send:", data);

            try
            {
                this.socket.Send(data);
            }
            catch (Exception ex)
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(3000);
                        Connect();
                    }
                    catch
                    {
                        #region 错误日记
                        AppLog.WriteErr(ex.Message);
                        #endregion

                        continue;
                    }
                    break;
                }
            }


            //this.socket.Send(data);
        }

        private void Log(string type, byte[] data)
        {
            if (this.Logger != null)
            {
                StringBuilder logText = new StringBuilder(type);
                foreach (byte item in data)
                {
                    logText.Append(item.ToString() + " ");
                }

                this.Logger.Write(logText.ToString());
            }
        }

        #region IDisposable 成员
        public void Dispose()
        {
            if (this.socket != null)
            {
                this.socket.Close();
            }
        }
        #endregion
    }
}