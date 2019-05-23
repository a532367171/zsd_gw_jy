using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using 进窑工位采集服务.common;

namespace 进窑工位采集服务
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            InitService();
        }
        /// <summary>
        /// 初始化服务参数
        /// </summary>
        private void InitService()
        {
            base.AutoLog = false;
            base.CanShutdown = true;
            base.CanStop = true;
            base.CanPauseAndContinue = true;
            base.ServiceName = "进窑工位采集服务";  //这个名字很重要，设置不一致会产生 1083 错误哦(在文章最后会说到这个问题)！
        }
        protected override void OnStart(string[] args)
        {
            #region 错误日记
            AppLog.WriteErr("服务启动");
            #endregion

            进窑工位周期执行类 x = new 进窑工位周期执行类();

            //事件执行类 v = new 事件执行类(); /* 实例化对象 */

            //x.ChangeNum1 += new 进窑工位周期执行类.NumManipulationHandler1(v.WriteLog1); /* 注册 */


            string str采集周期 = ConfigHelper.GetValue("采集周期").ToLower();

            int MyInt = Convert.ToInt32(str采集周期);


            System.Timers.Timer t = new System.Timers.Timer(MyInt);//实例化Timer类，设置间隔时间为10000毫秒；

            //或者是实例化时不传递参数.通过 myTimer.Interval=1000; 来设置间隔时间;

            t.Elapsed += new System.Timers.ElapsedEventHandler(x.theout);//到达时间的时候执行事件； 

            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
 
            //t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)； 

            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件； 
        }

        protected override void OnStop()
        {
            #region 错误日记
            AppLog.WriteErr("服务停止");
            #endregion
        }
    }
}
