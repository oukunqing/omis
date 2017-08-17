using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class NetworkLineMenuManage : OMIS.DAL.System.NetworkLineMenuDBA
    {

        #region  获得单个网络线路-菜单信息
        public NetworkLineMenuInfo GetNetworkLineMenuInfo(int id)
        {
            try
            {
                DataSet ds = this.GetNetworkLineMenu(id).DataSet;

                return CheckTable(ds, 0) ? this.FillNetworkLineMenuInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个网络线路-菜单信息
        public List<NetworkLineMenuInfo> GetNetworkLineMenuInfo(string idList)
        {
            try
            {
                List<NetworkLineMenuInfo> list = new List<NetworkLineMenuInfo>();

                DataSet ds = this.GetNetworkLineMenu(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillNetworkLineMenuInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得网络线路-菜单信息
        public List<NetworkLineMenuInfo> GetNetworkLineMenuInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<NetworkLineMenuInfo> list = new List<NetworkLineMenuInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillNetworkLineMenuInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<NetworkLineMenuInfo> GetNetworkLineMenuInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetNetworkLineMenuInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}