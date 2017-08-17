using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Common;

namespace OMIS.BLL.Common
{
    public class DictionaryTypeManage : OMIS.DAL.Common.DictionaryTypeDBA
    {

        #region  获得单个字典分类信息
        public DictionaryTypeInfo GetDictionaryTypeInfo(int typeId)
        {
            try
            {
                DataSet ds = this.GetDictionaryType(typeId).DataSet;

                return CheckTable(ds, 0) ? this.FillDictionaryTypeInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个字典分类信息
        public List<DictionaryTypeInfo> GetDictionaryTypeInfo(string typeIdList)
        {
            try
            {
                List<DictionaryTypeInfo> list = new List<DictionaryTypeInfo>();

                DataSet ds = this.GetDictionaryType(typeIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillDictionaryTypeInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得字典分类信息
        public List<Dictionary<string, object>> GetDictionaryTypeInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DictionaryTypeInfo info = this.FillDictionaryTypeInfo(dr);
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

        public List<Dictionary<string, object>> GetDictionaryTypeInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;

                return GetDictionaryTypeInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<DictionaryTypeInfo> GetDictionaryTypeInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<DictionaryTypeInfo> list = new List<DictionaryTypeInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillDictionaryTypeInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<DictionaryTypeInfo> GetDictionaryTypeInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetDictionaryTypeInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        
    }
}