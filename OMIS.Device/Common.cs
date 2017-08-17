using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Device
{
    public class Common
    {

        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

    }
}
