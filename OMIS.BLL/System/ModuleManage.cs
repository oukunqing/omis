using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class ModuleManage : OMIS.DAL.System.ModuleDBA
    {

        #region  获得单个模块配置信息
        public ModuleInfo GetModuleInfo(int moduleId)
        {
            try
            {
                DataSet ds = this.GetModule(moduleId).DataSet;

                return CheckTable(ds, 0) ? this.FillModuleInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个模块配置信息
        public List<ModuleInfo> GetModuleInfo(string moduleIdList)
        {
            try
            {
                List<ModuleInfo> list = new List<ModuleInfo>();

                DataSet ds = this.GetModule(moduleIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillModuleInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块配置信息
        public List<Dictionary<string, object>> GetModuleInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ModuleInfo info = this.FillModuleInfo(dr);
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

        public List<Dictionary<string, object>> GetModuleInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;

                return GetModuleInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<ModuleInfo> GetModuleInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<ModuleInfo> list = new List<ModuleInfo>();

                DataTable dtp = CheckTable(ds, 2) ? ds.Tables[2] : null;

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.ParseModulePermission(dtp, this.FillModuleInfo(dr)));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<ModuleInfo> GetModuleInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetModuleInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}