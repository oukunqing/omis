using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class RoleManage:OMIS.DAL.System.RoleDBA
    {

        #region  获得单个用户角色信息
        public RoleInfo GetRoleInfo(int roleId)
        {
            try
            {
                DataSet ds = this.GetRole(roleId).DataSet;

                return CheckTable(ds, 0) ? this.FillRoleInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个用户角色信息
        public List<RoleInfo> GetRoleInfo(string roleIdList)
        {
            try
            {
                List<RoleInfo> list = new List<RoleInfo>();

                DataSet ds = this.GetRole(roleIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得用户角色信息
        public List<RoleInfo> GetRoleInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<RoleInfo> list = new List<RoleInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<RoleInfo> GetRoleInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetRoleInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}