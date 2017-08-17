using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;
using OMIS.Common;

namespace OMIS.DAL.System
{
    public class MenuDBA : DataAccess
    {

        #region  获得单个导航菜单
        public DataResult GetMenu(int menuId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,t.type_name from `sys_menu` d ");
                sql.Append(" left outer join `sys_menu_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(String.Format(" where d.menu_id = {0} ", menuId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个导航菜单
        public DataResult GetMenu(string menuIdList)
        {
            try
            {
                if (!CheckIdList(ref menuIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,t.type_name from `sys_menu` d ");
                sql.Append(" left outer join `sys_menu_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(String.Format(" where d.`menu_id` in({0}) ", menuIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得导航菜单
        /// <summary>
        /// 获得导航菜单
        /// </summary>
        /// <param name="dic">
        /// TypeId: int, MenuCode : string, Enabled : int, OpenType: int,
        /// Keywords : string, SearchField : string, PageIndex : int, PageSize : int
        /// </param>
        /// <returns></returns>
        public DataResult GetMenu(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int typeId = ConvertValue(dic, "TypeId", 0);
                con.Append(typeId > 0 ? String.Format(" and d.`type_id` = {0} ", typeId) : "");

                string menuCode = ConvertValue(dic, "MenuCode");
                con.Append(Filter(ref menuCode).Length > 0 ? String.Format(" and t.menu_code = '{0}' ", menuCode) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                int openType = ConvertValue(dic, "OpenType", -1);
                con.Append(openType >= 0 ? String.Format(" and d.`open_type` = {0} ", openType) : "");

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`menu_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`menu_name` like '%{0}%' "));
                            break;
                        case "Code":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`menu_code` like '%{0}%' "));
                            break;
                        case "Url":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`menu_url` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,t.type_name from `sys_menu` d ");
                sql.Append(" left outer join `sys_menu_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.sort_order desc,d.`menu_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.menu_id) as dataCount from `sys_menu` d ");
                sql.Append(" left outer join `sys_menu_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  检测名称是否存在
        public bool CheckNameIsExist(string menuName, int menuId)
        {
            try
            {
                return CheckDataIsExist(DBConnString, "sys_menu", "menu_id", "menu_name", menuName, menuId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  检测名称是否存在
        public bool CheckCodeIsExist(string menuCode, int menuId)
        {
            try
            {
                return CheckDataIsExist(DBConnString, "sys_menu", "menu_id", "menu_code", menuCode, menuId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增导航菜单
        public DataResult AddMenu(MenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_menu`(");
                sql.Append("`type_id`,`menu_name`,`menu_code`,`menu_url`,`menu_pic`,`open_type`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(",`crc_code`");
                sql.Append(")values(");
                sql.Append("?TypeId,?MenuName,?MenuCode,?MenuUrl,?MenuPic,?OpenType,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(",?CrcCode");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeId", "?MenuName", "?MenuCode", "?MenuUrl", "?MenuPic", "?OpenType", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime", 
					"?CrcCode"
				};

                BuildCrcCode(o, true);

                List<object> value = new List<object>() {
					o.TypeId, o.MenuName, o.MenuCode, o.MenuUrl, EncodePic(o.MenuPic), o.OpenType, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime),
					o.CrcCode
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_menu", "menu_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新导航菜单
        public DataResult UpdateMenu(MenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_menu` set ");
                sql.Append("`type_id` = ?TypeId,`menu_name` = ?MenuName,`menu_code` = ?MenuCode,`menu_url` = ?MenuUrl,`menu_pic` = ?MenuPic");
                sql.Append(",`open_type` = ?OpenType,`enabled` = ?Enabled,`sort_order` = ?SortOrder,`update_time` = ?UpdateTime,`crc_code` = ?CrcCode");
                sql.Append(" where `menu_id` = ?MenuId;");

                List<string> name = new List<string>() {
					"?TypeId", "?MenuName", "?MenuCode", "?MenuUrl", "?MenuPic", "?OpenType", "?Enabled", "?SortOrder", "?UpdateTime", "?CrcCode",
                    "?MenuId"
				};

                BuildCrcCode(o, true);

                List<object> value = new List<object>() {
					o.TypeId, o.MenuName, o.MenuCode, o.MenuUrl, EncodePic(o.MenuPic), o.OpenType, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.CrcCode,
                    o.MenuId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region  更新菜单图片
        public DataResult UpdateMenuPhoto(string path, int menuId)
        {
            try
            {
                string sql = String.Format("update `sys_menu` set `menu_pic` = '{0}' where `menu_id` = {1};", EncodePic(Filter(path)), menuId);

                return new DataResult(sql, Update(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除导航菜单
        public DataResult DeleteMenu(int menuId)
        {
            try
            {
                string sql = String.Format("delete from `sys_menu` where `menu_id` = {0};", menuId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public MenuInfo FillMenuInfo(DataRow dr)
        {
            try
            {
                MenuInfo o = new MenuInfo();

                o.MenuId = DataConvert.ConvertValue(dr["menu_id"], 0);
                o.TypeId = DataConvert.ConvertValue(dr["type_id"], 0);
                o.MenuName = dr["menu_name"].ToString();
                o.MenuCode = dr["menu_code"].ToString();
                o.MenuUrl = dr["menu_url"].ToString();
                o.MenuPic = DecodePic(dr["menu_pic"].ToString());
                o.OpenType = DataConvert.ConvertValue(dr["open_type"], 0);
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);

                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();
                o.CrcCode = dr["crc_code"].ToString();

                if (CheckColumn(dr, "type_name"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"TypeName", dr["type_name"].ToString()}
                    };
                }

                return CheckCrcCode(o, o.CrcCode) ? o : DataCheck.AlwaysPass ? o : null;
            }
            catch (Exception ex) { throw (ex); }
        }

        public MenuInfo FillMenuInfo(DataRowView drv)
        {
            try
            {
                return this.FillMenuInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        
        #region  UcAuthCode
        private string EncodePic(string pic)
        {
            return pic.Equals(string.Empty) ? pic : UCAuthCode.AuthCodeEncode(pic, KeyApi.OMIS_KEY);
        }

        private string DecodePic(string pic)
        {
            try
            {
                return pic.Equals(string.Empty) ? pic : UCAuthCode.AuthCodeDecode(pic, KeyApi.OMIS_KEY);
            }
            catch (Exception ex)
            {
                return pic;
            }
        }
        #endregion

        #region  BuildCrcCode
        private string BuildCrcCode(MenuInfo o, bool isEncrypt)
        {
            if (o != null)
            {
                o.MenuUrl = isEncrypt ? UCAuthCode.AuthCodeEncode(o.MenuUrl, KeyApi.OMIS_KEY) : o.MenuUrl;

                string con = String.Format("{0}-{1}-{2}-{3}-{4}-{5}-ZYRH",
                    o.TypeId, o.MenuName, o.MenuCode, o.MenuUrl, o.OpenType, o.Enabled);
                o.CrcCode = CRC.ToCRC16(con, Encoding.UTF8);

                return o.CrcCode;
            }
            return string.Empty;
        }
        #endregion

        #region  CheckCrcCode
        private bool CheckCrcCode(MenuInfo o, string crcCode)
        {
            bool pass = !crcCode.Trim().Equals(string.Empty) && this.BuildCrcCode(o, false).Equals(crcCode);

            o.MenuUrl = UCAuthCode.AuthCodeDecode(o.MenuUrl, KeyApi.OMIS_KEY);

            return pass;
        }
        #endregion

    }
}