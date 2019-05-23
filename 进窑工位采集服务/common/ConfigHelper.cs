﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace 进窑工位采集服务.common
{
    public class ConfigHelper
    {
        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }


    }

}
