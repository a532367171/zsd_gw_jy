﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 进窑工位采集服务.common
{
    public class ValueHelper
    {
        #region Factory
        public static ValueHelper _Instance = null;
        internal static ValueHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ValueHelper();
                }
                return _Instance;
            }
        }
        #endregion
     

        public virtual byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        public virtual string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }

        public static string[] StringToStringArray(string m)
        {
            string s = System.Convert.ToString(Convert.ToInt32(m), 2).PadLeft(7, '0');
            String[] IDList = new String[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                IDList[i] = s.Substring(s.Length - 1 - i, 1);
            }
            if (IDList[0] == "0")
            {
                if (IDList[5] == "0" && IDList[6] == "0")
                {
                }
                else
                {
                    IDList[5] = "1";
                    IDList[6] = "0";
                }
            }
            else
            {
                if (IDList[5] == "1" && IDList[6] == "1")
                {
                }
                else
                {
                    IDList[5] = "0";
                    IDList[6] = "0";
                }
            }

            return IDList;

        }

    }
}
