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
    public class ModuleMenuManage : OMIS.DAL.System.ModuleMenuDBA
    {


        #region  获得单个模块菜单信息
        public ModuleMenuInfo GetModuleMenuInfo(int menuId)
        {
            try
            {
                DataSet ds = this.GetModuleMenu(menuId).DataSet;

                return CheckTable(ds, 0) ? this.FillModuleMenuInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region  获得多个模块菜单信息
        public List<ModuleMenuInfo> GetModuleMenuInfo(string menuIdList)
        {
            try
            {
                List<ModuleMenuInfo> list = new List<ModuleMenuInfo>();

                DataSet ds = this.GetModuleMenu(menuIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ModuleMenuInfo info = this.FillModuleMenuInfo(dr);
                        if (info != null)
                        {
                            list.Add(info);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块菜单信息
        public List<Dictionary<string, object>> GetModuleMenuInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount, bool filterShow)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                dataCount = 0;

                if (CheckTable(ds, 0))
                {
                    if (filterShow)
                    {
                        int minLevel = ConvertValue(ds.Tables[0].Rows[0]["level"].ToString(), 0);

                        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

                        Hashtable htShow = new Hashtable();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ModuleMenuInfo info = this.FillModuleMenuInfo(dr);
                            if (info != null)
                            {
                                if (info.Level > minLevel)
                                {
                                    if (htShow.ContainsKey(info.ParentId) && info.Enabled == 1)
                                    {
                                        htShow.Add(info.MenuId, info.Enabled);
                                        list.Add(ConvertClassValue(info, dicField, true));
                                    }
                                }
                                else if (info.Enabled == 1)
                                {
                                    htShow.Add(info.MenuId, info.Enabled);
                                    list.Add(ConvertClassValue(info, dicField, true));
                                }
                            }
                        }
                        dataCount = list.Count;
                    }
                    else
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ModuleMenuInfo info = this.FillModuleMenuInfo(dr);
                            if (info != null)
                            {
                                list.Add(ConvertClassValue(info, dicField, true));
                            }
                        }
                        dataCount = ConvertFieldValue(ds, 1, list.Count);
                    }
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<Dictionary<string, object>> GetModuleMenuInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;

                return GetModuleMenuInfo(ds, dicField, out dataCount, false);
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<ModuleMenuInfo> GetModuleMenuInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<ModuleMenuInfo> list = new List<ModuleMenuInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ModuleMenuInfo info = this.FillModuleMenuInfo(dr);
                        if (info != null)
                        {
                            list.Add(info);
                        }
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<ModuleMenuInfo> GetModuleMenuInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetModuleMenuInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}