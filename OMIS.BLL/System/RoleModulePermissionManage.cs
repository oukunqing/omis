using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class RoleModulePermissionManage : OMIS.DAL.System.RoleModulePermissionDBA
    {

        #region  获得单个角色-模块-权限信息
        public RoleModulePermissionInfo GetRoleModulePermissionInfo(int id)
        {
            try
            {
                DataSet ds = this.GetRoleModulePermission(id).DataSet;

                return CheckTable(ds, 0) ? this.FillRoleModulePermissionInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个角色-模块-权限信息
        public List<RoleModulePermissionInfo> GetRoleModulePermissionInfo(string idList)
        {
            try
            {
                List<RoleModulePermissionInfo> list = new List<RoleModulePermissionInfo>();

                DataSet ds = this.GetRoleModulePermission(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleModulePermissionInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色-模块-权限信息
        public List<RoleModulePermissionInfo> GetRoleModulePermissionInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<RoleModulePermissionInfo> list = new List<RoleModulePermissionInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleModulePermissionInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<RoleModulePermissionInfo> GetRoleModulePermissionInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetRoleModulePermissionInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}