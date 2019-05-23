using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using 进窑工位采集服务.common;
using 进窑工位采集服务.ModBus;

namespace 进窑工位采集服务
{
    public class 进窑工位周期执行类
    {

        #region 字段声明

        string read_old_value;

        string read_middle_value;

        string read_new_value;

        public static object locker = new Object();

        string Manufacture = ConfigurationManager.AppSettings["生产企业"];

        #endregion

        #region 初始化
        public 进窑工位周期执行类()
        {
            IniFileReference _iniFile = new IniFileReference(AppDomain.CurrentDomain.BaseDirectory + "Geometry.ini");

            string local_old_value = _iniFile.IniReadValue("SYSDNSection", "local_old_value");

            read_new_value = _iniFile.IniReadValue("SYSDNSection", "local_middle_value");

            read_old_value = local_old_value;

            _iniFile = null;

        }
        #endregion

       // #region 事件的申明

        //public delegate void NumManipulationHandler1(string x, string y, string z);

        //public event NumManipulationHandler1 ChangeNum1;

        //protected virtual void OnNumChanged(object obj)
        //{
        //    新旧值结构 struct参数 = (新旧值结构)obj;

        //    if (ChangeNum1 != null)
        //    {
        //        ChangeNum1(struct参数.old_value, struct参数.new_value, struct参数.middle_value); /* 事件被触发 */
        //    }
        //    else
        //    {
        //        Console.WriteLine("event not fire");
        //        Console.ReadKey(); /* 回车继续 */
        //    }
        //}


        //#endregion

        #region 周期执行（定时器到期执行）
        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            System.Timers.Timer t1 = (System.Timers.Timer)source;
            t1.Enabled = false;
            t1.Stop();

            try
            {
                //lock (locker)
                //{
                #region 读取工控机

                read_new_value = ModBusTCPIPWrapper.Instance.Read();

                #endregion

                if (read_new_value == read_old_value || read_new_value == read_middle_value)
                {
                }
                else
                {
                    //新旧值结构 rowCol = new 新旧值结构();
                    //rowCol.new_value = read_new_value;
                    //rowCol.old_value = read_old_value;
                    //rowCol.middle_value = read_old_value;
                    //Thread t = new Thread((new ParameterizedThreadStart(OnNumChanged)));
                    //t.Start(rowCol);
                    //read_middle_value = read_old_value;
                    //read_old_value = read_new_value;


                    try
                    {
                        #region 保旧值
                        IniFileReference _iniFile = new IniFileReference(AppDomain.CurrentDomain.BaseDirectory + "Geometry.ini");

                        _iniFile.IniWriteValue("SYSDNSection", "local_old_value", Convert.ToString(read_new_value));

                        _iniFile.IniWriteValue("SYSDNSection", "local_middle_value", Convert.ToString(read_old_value));

                        _iniFile = null;
                        #endregion

                        #region 工位判断

                        //string Manufacture = ConfigurationManager.AppSettings["生产企业"];


                        //string UUID = System.Guid.NewGuid().ToString("N");
                        int i = 0;

                        string strD = "";

                        if (read_old_value == "94" && read_new_value == "126")
                        {
                            string str = "INSERT INTO CmdGongWei (cGWid,cGWlineCode,cManufacture) VALUES ('08','01','" + Manufacture + "')";
                            strD = str;
                        }
                        if (read_old_value == "125" && read_new_value == "93")
                        {
                            string str = "INSERT INTO CmdGongWei (cGWid,cGWlineCode,cManufacture) VALUES ('09','01','" + Manufacture + "')";
                            strD = str;
                        }
                        if (read_old_value == "123" && read_new_value == "91")
                        {
                            string str = "INSERT INTO CmdGongWei (cGWid,cGWlineCode,cManufacture) VALUES ('09','02','" + Manufacture + "')";
                            strD = str;
                        }
                        if (read_old_value == "119" && read_new_value == "87")
                        {
                            string str = "INSERT INTO CmdGongWei (cGWid,cGWlineCode,cManufacture) VALUES ('09','03','" + Manufacture + "')";
                            strD = str;
                        }
                        if (read_old_value == "111" && read_new_value == "79")
                        {
                            string str = "INSERT INTO CmdGongWei (cGWid,cGWlineCode,cManufacture) VALUES ('09','04','" + Manufacture + "')";
                            strD = str;
                        }
                        try
                        {
                            if (strD != "")
                            {
                                i = DbUtils.ExecuteNonQuerySp(strD);

                            }
                        }
                        catch (Exception ex)
                        {
                            #region 错误日记
                            AppLog.WriteErr(ex.Message);
                            #endregion

                            #region 保旧值
                            IniFileReference _iniFile1 = new IniFileReference(AppDomain.CurrentDomain.BaseDirectory + "Geometry.ini");

                            _iniFile1.IniWriteValue("SYSDNSection", "local_old_value", Convert.ToString(read_old_value));

                            _iniFile1.IniWriteValue("SYSDNSection", "local_middle_value", Convert.ToString(read_middle_value));

                            _iniFile1 = null;
                            #endregion

                            System.Diagnostics.Process.Start("D:\\管片工位采集服务(中水电)\\进窑工位采集\\进窑工位采集服务\\进窑工位采集服务\\bin\\Debug\\进窑工位采集服务.exe", "5");


                        }
                        finally
                        {
                            #region 变化日记
                            AppLog.Write(System.Convert.ToString(Convert.ToInt32(read_old_value), 2).PadLeft(7, '0') + "--" + System.Convert.ToString(Convert.ToInt32(read_new_value), 2).PadLeft(7, '0') + "--插入标识：" + Convert.ToString(i) + "--" + strD);
                            #endregion
                            read_middle_value = read_old_value;
                            read_old_value = read_new_value;
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {

                        #region 错误日记
                        AppLog.WriteErr(ex.Message);
                        #endregion
                    }


                }
            }
            catch (SocketException ex)
            {
                #region 错误日记
                AppLog.WriteErr(ex.Message);
                #endregion
                System.Diagnostics.Process.Start("D:\\管片工位采集服务(中水电)\\进窑工位采集\\进窑工位采集服务\\进窑工位采集服务\\bin\\Debug\\进窑工位采集服务.exe", "5");

            }
            finally
            {
                t1.Enabled = true;
                t1.Start();
            }
        }

        #endregion

    }
}
