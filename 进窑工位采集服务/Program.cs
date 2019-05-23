using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using 进窑工位采集服务.common;

namespace 进窑工位采集服务
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                 { 
                new Service1() 
                 };
                ServiceBase.Run(ServicesToRun);
            }
            else if (args.Length == 1 && System.Text.RegularExpressions.Regex.IsMatch(args[0], @"^[1-5]$"))
            {
                try
                {
                    Process p = null;
                    ServiceController service = null;
                    switch (int.Parse(args[0]))
                    {
                        case 1:
                            //取当前可执行文件路径
                            var path = Process.GetCurrentProcess().MainModule.FileName + "";
                            p = Process.Start("sc", "create 进窑工位采集服务 binpath= \"" + path + "\" displayName= 进窑工位采集服务 start= auto");
                            p.WaitForExit();
                            break;
                        case 2:
                            p = Process.Start("sc", "delete 进窑工位采集服务");
                            p.WaitForExit();
                            break;
                        case 3:
                            service = new ServiceController("进窑工位采集服务");
                            service.Start();
                            service.WaitForStatus(ServiceControllerStatus.Running);
                            break;
                        case 4:
                            service = new ServiceController("进窑工位采集服务");
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                            break;
                        case 5:
                            service = new ServiceController("进窑工位采集服务");
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                            service.Start();
                            service.WaitForStatus(ServiceControllerStatus.Running);
                            break;

                    }
                }
                catch (Exception ex)
                {
                    #region 错误日记
                    AppLog.WriteErr(ex.Message);
                    #endregion
                }
            }

        }
    }
}
