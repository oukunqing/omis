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
    public class ModuleMenuDBA : DataAccess
    {

        #region  获得单个模块菜单
        public DataResult GetModuleMenu(int menuId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.menu_name as parent_name from `sys_module_menu` d ");
                sql.Append(" left outer join `sys_module_menu` pd on d.`parent_id` = pd.`menu_id` ");
                sql.Append(String.Format(" where d.`menu_id` = {0} ", menuId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个模块菜单
        public DataResult GetModuleMenu(string menuIdList)
        {
            try
            {
                if (!CheckIdList(ref menuIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.menu_name as parent_name from `sys_module_menu` d ");
                sql.Append(" left outer join `sys_module_menu` pd on d.`parent_id` = pd.`menu_id` ");
                sql.Append(String.Format(" where d.`menu_id` in({0}) ", menuIdList));
                sql.Append(" order by d.`level`,d.`sort_order` desc,d.`menu_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  根据编码获得ID
        public int GetMenuIdByMenuCode(string code)
        {
            try
            {
                return this.GetMenuIdByMenuCode(code, 0);
            }
            catch (Exception ex) { throw (ex); }
        }

        private int GetMenuIdByMenuCode(string code, int menuId)
        {
            try
            {
                if (menuId > 0 || code.Equals(string.Empty))
                {
                    return menuId;
                }
                string sql = String.Format(" select ifnull(`menu_id`,-1),count(`menu_id`) from `sys_module_menu` where `menu_code` = '{0}' limit 1;", Filter(ref code));

                return Convert.ToInt32(Scalar(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块菜单
        /// <summary>
        /// 获得模块菜单
        /// </summary>
        /// <param name="dic">
        /// RootId: int, ParentId : int, GetSubset : int, MenuCode : string, Enabled : int, MenuType : int, OpenType: int, ExcludeId : int,
        /// Keywords : string, SearchField : string, PageIndex : int, PageSize : int
        /// </param>
        /// <returns></returns>
        public DataResult GetModuleMenu(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int rootId = ConvertValue(dic, "RootId", 0);
                con.Append(rootId > 0 ? String.Format(" and d.parent_tree like '%({0})%' ", rootId) : "");

                int parentId = this.GetMenuIdByMenuCode(ConvertValue(dic, "ParentCode"), ConvertValue(dic, "ParentId", 0));
                if (parentId > 0)
                {
                    bool isGetSubset = ConvertValue(dic, "GetSubset", 0) == 1;
                    bool isIncludeOneself = isGetSubset ? ConvertValue(dic, "IncludeSelf|IncludeOneself", 1) == 1 : true;
                    con.Append(String.Format(isGetSubset ? " and d.parent_tree like '%({0})%' " : " and d.`parent_id` = {0} ", parentId));
                    if (!isIncludeOneself)
                    {
                        con.Append(String.Format(" and d.`menu_id` <> {0} ", parentId));
                    }
                }

                string menuCode = ConvertValue(dic, "MenuCode");
                con.Append(Filter(ref menuCode).Length > 0 ? String.Format(" and t.menu_code = '{0}' ", menuCode) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                int menuType = ConvertValue(dic, "MenuType", -1);
                con.Append(menuType >= 0 ? String.Format(" and d.`menu_type` = {0} ", menuType) : "");

                int openType = ConvertValue(dic, "OpenType", -1);
                con.Append(openType >= 0 ? String.Format(" and d.`open_type` = {0} ", openType) : "");
                
                int excludeId = ConvertValue(dic, "ExcludeId", 0);
                if (excludeId > 0)
                {
                    con.Append(String.Format(" and d.parent_tree not like '%({0})%' ", excludeId));
                }

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

                sql.Append(" select d.*,pd.menu_name as parent_name from `sys_module_menu` d ");
                sql.Append(" left outer join `sys_module_menu` pd on d.`parent_id` = pd.`menu_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`level`,d.`sort_order` desc,d.`menu_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.menu_id) as dataCount from `sys_module_menu` d ");
                sql.Append(" left outer join `sys_module_menu` pd on d.`parent_id` = pd.`menu_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增模块菜单
        public DataResult AddModuleMenu(ModuleMenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_module_menu`(");
                sql.Append("`menu_name`,`menu_code`,`menu_url`,`menu_pic`,`menu_desc`,`menu_type`,`open_type`,`level`,`parent_id`,`parent_tree`");
                sql.Append(",`enabled`,`sort_order`,`operator_id`,`create_time`,`crc_code`");
                sql.Append(")values(");
                sql.Append("?MenuName,?MenuCode,?MenuUrl,?MenuPic,?MenuDesc,?MenuType,?OpenType,?Level,?ParentId,?ParentTree");
                sql.Append(",?Enabled,?SortOrder,?OperatorId,?CreateTime,?CrcCode");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?MenuName", "?MenuCode", "?MenuUrl", "?MenuPic", "?MenuDesc", "?MenuType", "?OpenType", "?Level", "?ParentId", "?ParentTree", 
					"?Enabled", "?SortOrder", "?OperatorId", "?CreateTime", "?CrcCode"
				};

                BuildCrcCode(o, true);

                List<object> value = new List<object>() {
					o.MenuName, o.MenuCode, o.MenuUrl, EncodePic(o.MenuPic), o.MenuDesc, o.MenuType, o.OpenType, o.Level, o.ParentId, o.ParentTree, 
					o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime), o.CrcCode
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_module_menu", "menu_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新模块菜单
        public DataResult UpdateModuleMenu(ModuleMenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_module_menu` set ");
                sql.Append("`menu_name` = ?MenuName,`menu_code` = ?MenuCode,`menu_url` = ?MenuUrl,`menu_pic` = ?MenuPic,`menu_desc` = ?MenuDesc");
                sql.Append(",`menu_type` = ?MenuType,`open_type` = ?OpenType,`level` = ?Level,`parent_id` = ?ParentId,`parent_tree` = ?ParentTree");
                sql.Append(",`enabled` = ?Enabled,`sort_order` = ?SortOrder,`update_time` = ?UpdateTime,`crc_code` = ?CrcCode");
                sql.Append(" where `menu_id` = ?MenuId;");

                List<string> name = new List<string>() {
					"?MenuName", "?MenuCode", "?MenuUrl", "?MenuPic", "?MenuDesc", "?MenuType", "?OpenType", "?Level", "?ParentId", "?ParentTree", 
					"?Enabled", "?SortOrder", "?UpdateTime", "?CrcCode", "?MenuId"
				};

                BuildCrcCode(o, true);

                List<object> value = new List<object>() {
					o.MenuName, o.MenuCode, o.MenuUrl, EncodePic(o.MenuPic), o.MenuDesc, o.MenuType, o.OpenType, o.Level, o.ParentId, o.ParentTree, 
					o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.CrcCode, o.MenuId
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
                string sql = String.Format("update `sys_module_menu` set `menu_pic` = '{0}' where `menu_id` = {1};", EncodePic(Filter(path)), menuId);

                return new DataResult(sql, Update(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得层级
        public int GetLevel(int menuId)
        {
            return GetTypeLevel(DBConnString, "level", "sys_module_menu", "menu_id", menuId);
        }
        #endregion

        #region  更新菜单目录树
        public int UpdateParentTree(int menuId)
        {
            return UpdateParentTree(DBConnString, "sys_module_menu", "menu_id", menuId, 0);
        }
        public int UpdateParentTree(int menuId, int minLevel)
        {
            return UpdateParentTree(DBConnString, "sys_module_menu", "menu_id", menuId, minLevel);
        }
        #endregion
        
        #region  获得子菜单数量
        public int GetMenuChildCount(int menuId)
        {
            try
            {
                return GetChildCount(DBConnString, "sys_module_menu", "menu_id", menuId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除模块菜单
        public DataResult DeleteModuleMenu(int menuId)
        {
            try
            {
                string sql = String.Format("delete from `sys_module_menu` where `menu_id` = {0};", menuId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public ModuleMenuInfo FillModuleMenuInfo(DataRow dr)
        {
            try
            {
                ModuleMenuInfo o = new ModuleMenuInfo();

                o.MenuId = DataConvert.ConvertValue(dr["menu_id"], 0);
                o.MenuName = dr["menu_name"].ToString();
                o.MenuCode = dr["menu_code"].ToString();
                o.MenuUrl = dr["menu_url"].ToString();
                o.MenuPic = DecodePic(dr["menu_pic"].ToString());
                o.MenuDesc = dr["menu_desc"].ToString();
                o.MenuType = DataConvert.ConvertValue(dr["menu_type"], 0);
                o.OpenType = DataConvert.ConvertValue(dr["open_type"], 0);
                o.Level = DataConvert.ConvertValue(dr["level"], 0);
                o.ParentId = DataConvert.ConvertValue(dr["parent_id"], 0);

                o.ParentTree = dr["parent_tree"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();
                o.CrcCode = dr["crc_code"].ToString();

                if (CheckColumn(dr, "parent_name"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"ParentName", dr["parent_name"].ToString()}
                    };
                }

                return CheckCrcCode(o, o.CrcCode) ? o : DataCheck.AlwaysPass ? o : null;
            }
            catch (Exception ex) { throw (ex); }
        }

        public ModuleMenuInfo FillModuleMenuInfo(DataRowView drv)
        {
            try
            {
                return this.FillModuleMenuInfo(drv.Row);
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
        private string BuildCrcCode(ModuleMenuInfo o, bool isEncrypt)
        {
            if (o != null)
            {
                o.MenuUrl = isEncrypt ? UCAuthCode.AuthCodeEncode(o.MenuUrl, KeyApi.OMIS_KEY) : o.MenuUrl;

                string con = String.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-ZYRH",
                    o.MenuType, o.MenuName, o.MenuCode, o.MenuUrl, o.ParentId, o.OpenType, o.Enabled);
                o.CrcCode = CRC.ToCRC16(con, Encoding.UTF8);

                return o.CrcCode;
            }
            return string.Empty;
        }
        #endregion

        #region  CheckCrcCode
        private bool CheckCrcCode(ModuleMenuInfo o, string crcCode)
        {
            bool pass = !crcCode.Trim().Equals(string.Empty) && this.BuildCrcCode(o, false).Equals(crcCode);
            
            o.MenuUrl = UCAuthCode.AuthCodeDecode(o.MenuUrl, KeyApi.OMIS_KEY);

            return pass;
        }
        #endregion

    }
}