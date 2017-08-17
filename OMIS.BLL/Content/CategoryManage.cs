using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Content;

namespace OMIS.BLL.Content
{
    public class CategoryManage : OMIS.DAL.Content.CategoryDBA
    {

        #region  获得单个内容类别信息
        public CategoryInfo GetCategoryInfo(int categoryId)
        {
            try
            {
                DataSet ds = this.GetCategory(categoryId).DataSet;

                return CheckTable(ds, 0) ? this.FillCategoryInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个内容类别信息
        public List<CategoryInfo> GetCategoryInfo(string categoryIdList)
        {
            try
            {
                List<CategoryInfo> list = new List<CategoryInfo>();

                DataSet ds = this.GetCategory(categoryIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillCategoryInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得内容类别信息
        public List<CategoryInfo> GetCategoryInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<CategoryInfo> list = new List<CategoryInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillCategoryInfo(dr));
                    }
                }
                dataCount = CheckTable(ds, 1) ? ConvertValue(ds.Tables[1].Rows[0]["dataCount"], list.Count) : list.Count;

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<CategoryInfo> GetCategoryInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetCategoryInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}