using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 进窑工位采集服务.ModBus
{
    //枚举
    public enum Protocol
    {
        TCPIP,
        SerialPort
    }
    //抽象类
    public abstract class ModBusWrapper : IDisposable
    {
        private static ModBusWrapper _Instance;

        public static ModBusWrapper CreateInstance(Protocol protocol)
        {
            if (_Instance == null)
            {
                switch (protocol)
                {
                    case Protocol.TCPIP:
                        _Instance = new ModBusTCPIPWrapper();
                        break;
                    //case Protocol.SerialPort:
                    //    _Instance = new ModBusSerialPortWrapper();
                    //    break;
                    default:
                        break;
                }
            }
            return _Instance;
        }


        #region Transaction Identifier
        /// <summary>
        /// 数据序号标识
        /// </summary>
        private byte dataIndex = 0;

        protected byte CurrentDataIndex
        {
            get { return this.dataIndex; }
        }

        protected byte NextDataIndex()
        {
            return ++this.dataIndex;
        }
        #endregion

        public ILog Logger { get; set; }

        public abstract void Connect();

        public abstract byte[] Receive();

        public abstract string Read();


        public abstract void Send(byte[] data);

        #region IDisposable 成员
        public virtual void Dispose()
        {
        }
        #endregion
    }
}
