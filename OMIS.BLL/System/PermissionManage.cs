using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class PermissionManage : OMIS.DAL.System.PermissionDBA
    {

        #region  获得单个操作权限信息
        public PermissionInfo GetPermissionInfo(int permissionId)
        {
            try
            {
                DataSet ds = this.GetPermission(permissionId).DataSet;

                return CheckTable(ds, 0) ? this.FillPermissionInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个操作权限信息
        public List<PermissionInfo> GetPermissionInfo(string permissionIdList)
        {
            try
            {
                List<PermissionInfo> list = new List<PermissionInfo>();

                DataSet ds = this.GetPermission(permissionIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillPermissionInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得操作权限信息
        public List<PermissionInfo> GetPermissionInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<PermissionInfo> list = new List<PermissionInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillPermissionInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<PermissionInfo> GetPermissionInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetPermissionInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}