using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Security;
using System.Collections;
using System.Data.SqlClient;

namespace OMIS.DBUtility
{

    /// <summary>
    /// 数据库的通用访问代码
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>
    public abstract class MssqlHelper
    {

        // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());


        #region  ExecuteDataSet
        public static DataSet ExecuteDataSet(string connString, string cmdText)
        {
            try
            {
                return ExecuteDataSet(connString, cmdText.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, string cmdText)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                return ds;
            }
            catch (Exception ex) { throw (ex); }
            finally { conn.Close(); }
        }

        public static DataSet ExecuteDataSet(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            try
            {
                return ExecuteDataSet(new SqlConnection(connString), cmdType, cmdText, cmdParams);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }
        #endregion

        #region  ExecuteNonQuery
        public static int ExecuteNonQuery(string connString, string cmdText)
        {
            try
            {
                return ExecuteNonQuery(new SqlConnection(connString), cmdText);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int ExecuteNonQuery(SqlConnection conn, string cmdText)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                conn.Open();

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw (ex); }
            finally { conn.Close(); }
        }

        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            try
            {
                return ExecuteNonQuery(new SqlConnection(connString), cmdType, cmdText, cmdParams);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParams);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); trans.Connection.Close(); }
        }
        #endregion

        #region  ExecuteReader
        public static SqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);

                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }
        #endregion
        
        #region  ExecuteScalar
        public static object ExecuteScalar(string connString, string cmdText)
        {
            try
            {
                return ExecuteScalar(new SqlConnection(connString), cmdText);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object ExecuteScalar(SqlConnection conn, string cmdText)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                conn.Open();

                return cmd.ExecuteScalar();
            }
            catch (Exception ex) { throw (ex); }
            finally { conn.Close(); }
        }

        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            try
            {
                return ExecuteScalar(new SqlConnection(connString), cmdType, cmdText, cmdParams);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);

                return cmd.ExecuteScalar();
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }
        #endregion

        #region  PrepareCommand
        /// <summary>
        /// 缓存参数数组
        /// </summary>
        /// <param name="cacheKey">参数缓存的键值</param>
        /// <param name="cmdParms">被缓存的参数列表</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParams)
        {
            parmCache[cacheKey] = cmdParams;
        }

        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
            {
                return null;
            }

            //新建一个参数的克隆列表
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
            {
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
            }
            return clonedParms;
        }

        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

    }

}