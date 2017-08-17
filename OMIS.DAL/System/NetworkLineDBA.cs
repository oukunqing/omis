using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class NetworkLineDBA : DataAccess
    {

        #region  获得单个网络线路
        public DataResult GetNetworkLine(int lineId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_network_line` d ");
                sql.Append(String.Format(" where d.`line_id` = {0} ", lineId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个网络线路
        public DataResult GetNetworkLine(string lineIdList)
        {
            try
            {
                if (!CheckIdList(ref lineIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_network_line` d ");
                sql.Append(String.Format(" where d.`line_id` in({0}) ", lineIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得网络线路
        public DataResult GetNetworkLine(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_network_line` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增网络线路
        public DataResult AddNetworkLine(NetworkLineInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_network_line`(");
                sql.Append("`line_name`,`line_code`,`line_number`,`line_desc`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?LineName,?LineCode,?LineNumber,?LineDesc,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?LineName", "?LineCode", "?LineNumber", "?LineDesc", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.LineName, o.LineCode, o.LineNumber, o.LineDesc, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_network_line", "line_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新网络线路
        public DataResult UpdateNetworkLine(NetworkLineInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_network_line` set ");
                sql.Append("`line_name` = ?LineName,`line_code` = ?LineCode,`line_number` = ?LineNumber,`line_desc` = ?LineDesc,`enabled` = ?Enabled");
                sql.Append(",`sort_order` = ?SortOrder");
                sql.Append(" where `line_id` = ?LineId;");

                List<string> name = new List<string>() {
					"?LineName", "?LineCode", "?LineNumber", "?LineDesc", "?Enabled", "?SortOrder", "?LineId"
				};

                List<object> value = new List<object>() {
					o.LineName, o.LineCode, o.LineNumber, o.LineDesc, o.Enabled, o.SortOrder, o.LineId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除网络线路
        public DataResult DeleteNetworkLine(int lineId)
        {
            try
            {
                string sql = String.Format("delete from `sys_network_line` where `line_id` = {0};", lineId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public NetworkLineInfo FillNetworkLineInfo(DataRow dr)
        {
            try
            {
                NetworkLineInfo o = new NetworkLineInfo();

                o.LineId = DataConvert.ConvertValue(dr["line_id"], 0);
                o.LineName = dr["line_name"].ToString();
                o.LineCode = dr["line_code"].ToString();
                o.LineNumber = DataConvert.ConvertValue(dr["line_number"], 0);
                o.LineDesc = dr["line_desc"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public NetworkLineInfo FillNetworkLineInfo(DataRowView drv)
        {
            try
            {
                return this.FillNetworkLineInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}