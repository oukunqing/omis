using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace OMIS.DBUtility
{
    /// <summary>
    /// MySqlHelper 的摘要说明
    /// </summary>
    public abstract class MysqlHelper
    {
        /*
         * 采用 MySql.Data.dll 操作Mysql数据库
         * 字符串示例:
         * host=localhost;user id=root;password=12345;database=test;port=3306;allow zero datetime=no;charset=gb2312;
         * host=127.0.0.1;user id=root;password=12345;database=test;port=3306;allow zero datetime=no;charset=gb2312;
         * host=127.0.0.1;user id=root;password=12345;database=test;allow zero datetime=no;charset=gb2312;
         */


        // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region  ExecuteDataSet Return DataSet
        public static DataSet ExecuteDataSet(string connString, string cmdText)
        {
            try
            {
                return ExecuteDataSet(new MySqlConnection(connString), cmdText);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet ExecuteDataSet(MySqlConnection conn, string cmdText)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                return ds;
            }
            catch (Exception ex) { throw (ex); }
            finally { conn.Close(); }
        }

        public static DataSet ExecuteDataSet(string connString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParams)
        {
            try
            {
                return ExecuteDataSet(new MySqlConnection(connString), cmdType, cmdText, cmdParams);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet ExecuteDataSet(MySqlConnection conn, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParams)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);

                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                return ds;
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }
        #endregion
        
        #region  MySqlDataReader Return MySqlDataReader
        public static MySqlDataReader ExecuteReader(string connString, string cmdText)
        {
            try
            {
                return ExecuteReader(new MySqlConnection(connString), cmdText);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static MySqlDataReader ExecuteReader(MySqlConnection conn, string cmdText)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                conn.Open();

                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) { throw (ex); }
            finally { conn.Close(); }
        }

        public static MySqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParams)
        {
            try
            {
                return ExecuteReader(new MySqlConnection(connString), cmdType, cmdText, cmdParams);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static MySqlDataReader ExecuteReader(MySqlConnection conn, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParams)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);

                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }
        #endregion
        
        #region  ExecuteScalar Return Object
        public static object ExecuteScalar(string connString, string cmdText)
        {
            try
            {
                return ExecuteScalar(new MySqlConnection(connString), cmdText);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object ExecuteScalar(MySqlConnection conn, string cmdText)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                conn.Open();

                return cmd.ExecuteScalar();
            }
            catch (Exception ex) { throw (ex); }
            finally { conn.Close(); }
        }

        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParams)
        {
            try
            {
                return ExecuteScalar(new MySqlConnection(connString), cmdType, cmdText, cmdParams);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object ExecuteScalar(MySqlConnection conn, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParams)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);

                return cmd.ExecuteScalar();
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }
        #endregion

        #region  ExecuteNonQuery Return int
        public static int ExecuteNonQuery(string connString, string cmdText)
        {
            try
            {
                return ExecuteNonQuery(new MySqlConnection(connString), cmdText);
            }
            catch (Exception ex) { throw (ex); }
        }
        public static int ExecuteNonQuery(MySqlConnection conn, string cmdText)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                conn.Open();

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw (ex); }
            finally { conn.Close(); }
        }

        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params MySqlParameter[] param)
        {
            try
            {
                return ExecuteNonQuery(new MySqlConnection(connString), cmdType, cmdText, param);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int ExecuteNonQuery(MySqlConnection conn, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParams)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParams);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw (ex); }
            finally { cmd.Parameters.Clear(); conn.Close(); }
        }
        #endregion

        #region  PrepareCommand
        public static void CacheParameters(string cacheKey, params MySqlParameter[] cmdParams)
        {
            parmCache[cacheKey] = cmdParams;
        }

        public static MySqlParameter[] GetCachedParameters(string cacheKey)
        {
            MySqlParameter[] cachedParms = (MySqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
            {
                return null;
            }

            //新建一个参数的克隆列表
            MySqlParameter[] clonedParms = new MySqlParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
            {
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (MySqlParameter)((ICloneable)cachedParms[i]).Clone();
            }

            return clonedParms;
        }
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (MySqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

    }
}