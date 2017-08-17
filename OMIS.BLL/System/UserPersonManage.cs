using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class UserPersonManage : OMIS.DAL.System.UserPersonDBA
    {

        #region  获得单个用户-人员信息
        public UserPersonInfo GetUserPersonInfo(int id)
        {
            try
            {
                DataSet ds = this.GetUserPerson(id).DataSet;

                return CheckTable(ds, 0) ? this.FillUserPersonInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个用户-人员信息
        public List<UserPersonInfo> GetUserPersonInfo(string idList)
        {
            try
            {
                List<UserPersonInfo> list = new List<UserPersonInfo>();

                DataSet ds = this.GetUserPerson(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillUserPersonInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得用户-人员信息
        public List<UserPersonInfo> GetUserPersonInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<UserPersonInfo> list = new List<UserPersonInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillUserPersonInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<UserPersonInfo> GetUserPersonInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetUserPersonInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}