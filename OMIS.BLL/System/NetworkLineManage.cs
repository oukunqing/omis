using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.BLL.System
{
    public class NetworkLineManage : OMIS.DAL.System.NetworkLineDBA
    {

        #region  获得单个网络线路信息
        public NetworkLineInfo GetNetworkLineInfo(int lineId)
        {
            try
            {
                DataSet ds = this.GetNetworkLine(lineId).DataSet;

                return CheckTable(ds, 0) ? this.FillNetworkLineInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个网络线路信息
        public List<NetworkLineInfo> GetNetworkLineInfo(string lineIdList)
        {
            try
            {
                List<NetworkLineInfo> list = new List<NetworkLineInfo>();

                DataSet ds = this.GetNetworkLine(lineIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillNetworkLineInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得网络线路信息
        public List<NetworkLineInfo> GetNetworkLineInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<NetworkLineInfo> list = new List<NetworkLineInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillNetworkLineInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<NetworkLineInfo> GetNetworkLineInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetNetworkLineInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}