using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class UserRoleManage : OMIS.DAL.System.UserRoleDBA
    {

        #region  获得单个用户-角色信息
        public UserRoleInfo GetUserRoleInfo(int id)
        {
            try
            {
                DataSet ds = this.GetUserRole(id).DataSet;

                return CheckTable(ds, 0) ? this.FillUserRoleInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个用户-角色信息
        public List<UserRoleInfo> GetUserRoleInfo(string idList)
        {
            try
            {
                List<UserRoleInfo> list = new List<UserRoleInfo>();

                DataSet ds = this.GetUserRole(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillUserRoleInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得用户-角色信息
        public List<UserRoleInfo> GetUserRoleInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<UserRoleInfo> list = new List<UserRoleInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillUserRoleInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<UserRoleInfo> GetUserRoleInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetUserRoleInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}