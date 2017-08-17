using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.DAL
{
    public abstract class DataCheck : OMIS.DBA.DataCheck
    {

        /// <summary>
        /// 是否免检
        /// </summary>
        public static bool AlwaysPass = false;

    }
}