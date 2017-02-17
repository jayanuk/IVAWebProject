using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace IVA.Common
{
    public class ConfigurationHelper
    {
        public static double TIME_OFFSET
        {          
            get { return Convert.ToDouble(ConfigurationManager.AppSettings[Constant.ConfigurationKeys.UTC_Offset]); }
        }

        public static double DAYS_TO_EXPIRE_REQUEST
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings[Constant.ConfigurationKeys.DAYS_TO_EXPIRE_REQUEST]); }
        }

        public static int HOURS_TO_FOLLOW_UP
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings[Constant.ConfigurationKeys.HOURS_TO_FOLLOW_UP]); }
        }

        public static string NOTIFICATION_TITLE
        {
            get { return ConfigurationManager.AppSettings[Constant.ConfigurationKeys.NOTIFICATION_TITLE].ToString(); }
        }
    }
}
