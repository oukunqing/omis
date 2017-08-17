using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Content;

namespace OMIS.BLL.Content
{
    public class TypeManage : OMIS.DAL.Content.TypeDBA
    {

        #region  获得单个内容分类信息
        public TypeInfo GetTypeInfo(int typeId)
        {
            try
            {
                DataSet ds = this.GetType(typeId).DataSet;

                return CheckTable(ds, 0) ? this.FillTypeInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个内容分类信息
        public List<TypeInfo> GetTypeInfo(string typeIdList)
        {
            try
            {
                List<TypeInfo> list = new List<TypeInfo>();

                DataSet ds = this.GetType(typeIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillTypeInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}