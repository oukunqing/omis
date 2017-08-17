using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class RoleGroupManage : OMIS.DAL.System.RoleGroupDBA
    {

        #region  获得单个角色组别信息
        public RoleGroupInfo GetRoleGroupInfo(int groupId)
        {
            try
            {
                DataSet ds = this.GetRoleGroup(groupId).DataSet;

                return CheckTable(ds, 0) ? this.FillRoleGroupInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个角色组别信息
        public List<RoleGroupInfo> GetRoleGroupInfo(string groupIdList)
        {
            try
            {
                List<RoleGroupInfo> list = new List<RoleGroupInfo>();

                DataSet ds = this.GetRoleGroup(groupIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleGroupInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色组别信息
        public List<RoleGroupInfo> GetRoleGroupInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<RoleGroupInfo> list = new List<RoleGroupInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillRoleGroupInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<RoleGroupInfo> GetRoleGroupInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetRoleGroupInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}