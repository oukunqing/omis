using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class UserManage : OMIS.DAL.System.UserDBA
    {

        #region  获得单个用户信息
        public UserInfo GetUserInfo(int userId)
        {
            try
            {
                DataSet ds = this.GetUser(userId).DataSet;

                return CheckTable(ds, 0) ? this.FillUserInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个用户信息
        public List<UserInfo> GetUserInfo(string userIdList)
        {
            try
            {
                List<UserInfo> list = new List<UserInfo>();

                DataSet ds = this.GetUser(userIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillUserInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得用户信息
        public List<UserInfo> GetUserInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<UserInfo> list = new List<UserInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillUserInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<UserInfo> GetUserInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetUserInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}