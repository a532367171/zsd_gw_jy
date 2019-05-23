using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

namespace 进窑工位采集服务.ModBus
{
    public class ModBus采集类:ILog //, IDisposable
    {
        //private ModBusWrapper Wrapper = null;

        //public ModBus采集类()
        //{
        //    System.Timers.Timer t = new System.Timers.Timer(1000);//实例化Timer类，设置间隔时间为10000毫秒；

        //    //或者是实例化时不传递参数.通过 myTimer.Interval=1000; 来设置间隔时间;
        //    t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件； 
        //    //t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)； 
        //    t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)； 

        //    t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件； 
        //}

       

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            string x = ModBusTCPIPWrapper.Instance.Read();
            //MessageBox.Show(x);
        }


        #region ILog 成员 接口实现
        public void Write(string log)
        {
            //this.tbxHistory.AppendText(log + Environment.NewLine);
            //this.tbxHistory.Select(this.tbxHistory.TextLength, 0);
            //this.tbxHistory.ScrollToCaret();
        }
        #endregion

     
    }
}
