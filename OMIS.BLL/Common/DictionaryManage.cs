using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Common;

namespace OMIS.BLL.Common
{
    public class DictionaryManage : OMIS.DAL.Common.DictionaryDBA
    {

        #region  获得单个分类字典信息
        public DictionaryInfo GetDictionaryInfo(int dictionaryId)
        {
            try
            {
                DataSet ds = this.GetDictionary(dictionaryId).DataSet;

                return CheckTable(ds, 0) ? this.FillDictionaryInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个分类字典信息
        public List<DictionaryInfo> GetDictionaryInfo(string dictionaryIdList)
        {
            try
            {
                List<DictionaryInfo> list = new List<DictionaryInfo>();

                DataSet ds = this.GetDictionary(dictionaryIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillDictionaryInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得分类字典信息
        public List<DictionaryInfo> GetDictionaryInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<DictionaryInfo> list = new List<DictionaryInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillDictionaryInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<DictionaryInfo> GetDictionaryInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetDictionaryInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}