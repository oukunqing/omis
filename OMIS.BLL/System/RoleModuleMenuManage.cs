using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class RoleModuleMenuManage : OMIS.DAL.System.RoleModuleMenuDBA
    {

        #region  获得单个角色-模块菜单信息
        public RoleModuleMenuInfo GetRoleModuleMenuInfo(int id)
        {
            try
            {
                DataSet ds = this.GetRoleModuleMenu(id).DataSet;

                return CheckTable(ds, 0) ? this.FillRoleModuleMenuInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个角色-模块菜单信息
        public List<RoleModuleMenuInfo> GetRoleModuleMenuInfo(string idList)
        {
            try
            {
                List<RoleModuleMenuInfo> list = new List<RoleModuleMenuInfo>();

                DataSet ds = this.GetRoleModuleMenu(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleModuleMenuInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色-模块菜单信息
        public List<RoleModuleMenuInfo> GetRoleModuleMenuInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<RoleModuleMenuInfo> list = new List<RoleModuleMenuInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleModuleMenuInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<RoleModuleMenuInfo> GetRoleModuleMenuInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetRoleModuleMenuInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块菜单列表
        public List<Dictionary<string,object>> GetRoleModuleMenuInfo(DataTable dt, bool filterShow)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(dt))
                {
                    if (filterShow)
                    {
                        int minLevel = ConvertValue(dt.Rows[0]["level"].ToString(), 0);

                        DataView dv = new DataView(dt, "", "", DataViewRowState.CurrentRows);

                        Hashtable htShow = new Hashtable();

                        foreach (DataRow dr in dt.Rows)
                        {
                            Dictionary<string, object> dic = FillDataValue(dr, true);
                            if (dic != null)
                            {
                                if (ConvertValue(dic, "Level", 0) > minLevel)
                                {
                                    if (htShow.ContainsKey(ConvertValue(dic, "ParentId", 0)) && ConvertValue(dic, "Enabled", 0) == 1)
                                    {
                                        htShow.Add(ConvertValue(dic, "MenuId", 0), ConvertValue(dic, "Enabled", 0));
                                        list.Add(dic);
                                    }
                                }
                                else if (ConvertValue(dic, "Enabled", 0) == 1)
                                {
                                    htShow.Add(ConvertValue(dic, "MenuId", 0), ConvertValue(dic, "Enabled", 0));
                                    list.Add(dic);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(FillDataValue(dr));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}