using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class PermissionTypeManage : OMIS.DAL.System.PermissionTypeDBA
    {

        #region  获得单个权限分类信息
        public PermissionTypeInfo GetPermissionTypeInfo(int typeId)
        {
            try
            {
                DataSet ds = this.GetPermissionType(typeId).DataSet;

                return CheckTable(ds, 0) ? this.FillPermissionTypeInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个权限分类信息
        public List<PermissionTypeInfo> GetPermissionTypeInfo(string typeIdList)
        {
            try
            {
                List<PermissionTypeInfo> list = new List<PermissionTypeInfo>();

                DataSet ds = this.GetPermissionType(typeIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillPermissionTypeInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得权限分类信息
        public List<Dictionary<string, object>> GetPermissionTypeInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PermissionTypeInfo info = this.FillPermissionTypeInfo(dr);
                        if (info != null)
                        {
                            list.Add(ConvertClassValue(info, dicField, true));
                        }
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<Dictionary<string, object>> GetPermissionTypeInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;

                return GetPermissionTypeInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<PermissionTypeInfo> GetPermissionTypeInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<PermissionTypeInfo> list = new List<PermissionTypeInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillPermissionTypeInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<PermissionTypeInfo> GetPermissionTypeInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetPermissionTypeInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}