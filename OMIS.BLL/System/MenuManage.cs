using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class MenuManage : OMIS.DAL.System.MenuDBA
    {

        #region  获得单个导航菜单信息
        public MenuInfo GetMenuInfo(int menuId)
        {
            try
            {
                DataSet ds = this.GetMenu(menuId).DataSet;
                return CheckTable(ds, 0) ? this.FillMenuInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个导航菜单信息
        public List<MenuInfo> GetMenuInfo(string menuIdList)
        {
            try
            {
                List<MenuInfo> list = new List<MenuInfo>();

                DataSet ds = this.GetMenu(menuIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MenuInfo info = this.FillMenuInfo(dr);
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

        #region  获得导航菜单信息
        public List<Dictionary<string, object>> GetMenuInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MenuInfo info = this.FillMenuInfo(dr);
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

        public List<Dictionary<string, object>> GetMenuInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;

                return GetMenuInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<MenuInfo> GetMenuInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<MenuInfo> list = new List<MenuInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MenuInfo info = this.FillMenuInfo(dr);
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

        public List<MenuInfo> GetMenuInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetMenuInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
