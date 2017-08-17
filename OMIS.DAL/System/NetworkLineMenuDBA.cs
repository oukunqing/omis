using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class NetworkLineMenuDBA : DataAccess
    {

        #region  获得单个网络线路-菜单
        public DataResult GetNetworkLineMenu(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_network_line_menu` d ");
                sql.Append(String.Format(" where d.`id` = {0} ", id));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个网络线路-菜单
        public DataResult GetNetworkLineMenu(string idList)
        {
            try
            {
                if (!CheckIdList(ref idList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_network_line_menu` d ");
                sql.Append(String.Format(" where d.`id` in({0}) ", idList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得网络线路-菜单
        public DataResult GetNetworkLineMenu(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_network_line_menu` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增网络线路-菜单
        public DataResult AddNetworkLineMenu(NetworkLineMenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_network_line_menu`(");
                sql.Append("`menu_id`,`line_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?MenuId,?LineId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?MenuId", "?LineId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.MenuId, o.LineId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_network_line_menu", "id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新网络线路-菜单
        public DataResult UpdateNetworkLineMenu(NetworkLineMenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_network_line_menu` set ");
                sql.Append("`menu_id` = ?MenuId,`line_id` = ?LineId");
                sql.Append(" where `id` = ?Id;");

                List<string> name = new List<string>() {
					"?MenuId", "?LineId", "?Id"
				};

                List<object> value = new List<object>() {
					o.MenuId, o.LineId, o.Id
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除网络线路-菜单
        public DataResult DeleteNetworkLineMenu(int id)
        {
            try
            {
                string sql = String.Format("delete from `sys_network_line_menu` where `id` = {0};", id);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public NetworkLineMenuInfo FillNetworkLineMenuInfo(DataRow dr)
        {
            try
            {
                NetworkLineMenuInfo o = new NetworkLineMenuInfo();

                o.Id = DataConvert.ConvertValue(dr["id"], 0);
                o.MenuId = DataConvert.ConvertValue(dr["menu_id"], 0);
                o.LineId = DataConvert.ConvertValue(dr["line_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public NetworkLineMenuInfo FillNetworkLineMenuInfo(DataRowView drv)
        {
            try
            {
                return this.FillNetworkLineMenuInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}