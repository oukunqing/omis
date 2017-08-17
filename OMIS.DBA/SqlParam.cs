using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.DBA
{

    #region  SqlParam
    public class SqlParam
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public SqlParam()
        {
        }

        public SqlParam(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
    #endregion

}