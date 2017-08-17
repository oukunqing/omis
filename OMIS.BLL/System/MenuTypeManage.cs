using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class MenuTypeManage : OMIS.DAL.System.MenuTypeDBA
    {

        #region  获得单个导航菜单类型信息
        public MenuTypeInfo GetMenuTypeInfo(int typeId)
        {
            try
            {
                DataSet ds = this.GetMenuType(typeId).DataSet;

                return CheckTable(ds, 0) ? this.FillMenuTypeInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个导航菜单分类信息
        public List<MenuTypeInfo> GetMenuTypeInfo(string typeIdList)
        {
            try
            {
                List<MenuTypeInfo> list = new List<MenuTypeInfo>();

                DataSet ds = this.GetMenuType(typeIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillMenuTypeInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得导航菜单分类信息
        public List<MenuTypeInfo> GetMenuTypeInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<MenuTypeInfo> list = new List<MenuTypeInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillMenuTypeInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<MenuTypeInfo> GetMenuTypeInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetMenuTypeInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
