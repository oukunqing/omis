using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace OMIS.Model
{
    public class DataResult
    {

        #region  属性

        /// <summary>
        /// SQL String
        /// </summary>
        public string Sql { get; set; }
        /// <summary>
        /// Result: DataSet
        /// </summary>
        public DataSet DataSet { get; set; }
        /// <summary>
        /// Result: ResultNumber
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// Result: Count
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Result: String
        /// </summary>
        public string ResultText { get; set; }
        /// <summary>
        /// Result: Object
        /// </summary>
        public object ResultObject { get; set; }
        /// <summary>
        /// Result: ObjectList
        /// </summary>
        public List<object> ResultList { get; set; }

        #endregion

        #region  构造函数
        public DataResult()
        {
            this.ResultList = new List<object>();
        }

        public DataResult(string sql)
        {
            this.Sql = sql;
        }

        public DataResult(StringBuilder sql)
        {
            this.Sql = sql.ToString();
        }

        public DataResult(string sql, DataSet ds)
        {
            this.Sql = sql;
            this.DataSet = ds;
        }

        public DataResult(StringBuilder sql, DataSet ds)
        {
            this.Sql = sql.ToString();
            this.DataSet = ds;
        }

        public DataResult(DataSet ds)
        {
            this.DataSet = ds;
        }

        public DataResult(string sql, int n)
        {
            this.Sql = sql;
            this.Result = n;
        }

        public DataResult(StringBuilder sql, int n)
        {
            this.Sql = sql.ToString();
            this.Result = n;
        }

        public DataResult(int n)
        {
            this.Result = n;
        }
        #endregion

    }
}