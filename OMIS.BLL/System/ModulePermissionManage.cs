using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class ModulePermissionManage : OMIS.DAL.System.ModulePermissionDBA
    {

        #region  获得单个模块-权限信息
        public ModulePermissionInfo GetModulePermissionInfo(int id)
        {
            try
            {
                DataSet ds = this.GetModulePermission(id).DataSet;

                return CheckTable(ds, 0) ? this.FillModulePermissionInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个模块-权限信息
        public List<ModulePermissionInfo> GetModulePermissionInfo(string idList)
        {
            try
            {
                List<ModulePermissionInfo> list = new List<ModulePermissionInfo>();

                DataSet ds = this.GetModulePermission(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillModulePermissionInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块-权限信息
        public List<ModulePermissionInfo> GetModulePermissionInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<ModulePermissionInfo> list = new List<ModulePermissionInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillModulePermissionInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<ModulePermissionInfo> GetModulePermissionInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetModulePermissionInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}