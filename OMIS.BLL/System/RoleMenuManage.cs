using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class RoleMenuManage : OMIS.DAL.System.RoleMenuDBA
    {

        #region  获得单个角色-导航菜单信息
        public RoleMenuInfo GetRoleMenuInfo(int id)
        {
            try
            {
                DataSet ds = this.GetRoleMenu(id).DataSet;

                return CheckTable(ds, 0) ? this.FillRoleMenuInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个角色-导航菜单信息
        public List<RoleMenuInfo> GetRoleMenuInfo(string idList)
        {
            try
            {
                List<RoleMenuInfo> list = new List<RoleMenuInfo>();

                DataSet ds = this.GetRoleMenu(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleMenuInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色-导航菜单信息
        public List<RoleMenuInfo> GetRoleMenuInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<RoleMenuInfo> list = new List<RoleMenuInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleMenuInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<RoleMenuInfo> GetRoleMenuInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetRoleMenuInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}