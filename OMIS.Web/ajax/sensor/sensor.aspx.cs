using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using OMIS.BLL;
using OMIS.BLL.System;
using OMIS.BLL.Sensor;
using OMIS.BLL.Common;
using OMIS.Model;
using OMIS.Model.Sensor;
using OMIS.Model.Common;

public partial class ajax_sensor_sensor : System.Web.UI.Page
{
    protected UserCenter uc = new UserCenter();
    protected string action = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";

        this.InitialData();
    }

    #region  初始化数据
    protected void InitialData()
    {
        try
        {
            this.action = Public.Request("action");
            switch (this.action)
            {
                #region  传感器分类
                case "editSensorType":
                    Response.Write(this.EditSensorType(Public.Request("data", "{}")));
                    break;
                case "getSensorType":
                    Response.Write(this.GetSensorType(Public.Request("typeId|id", 0)));
                    break;
                case "getSensorTypeList":
                    Response.Write(this.GetSensorTypeList(Public.Request("data", "{}")));
                    break;
                case "getSensorTypeToParent":
                case "getSensorTypeTree":
                    Response.Write(this.GetSensorTypeTree(Public.Request("data", "{}")));
                    break;
                case "deleteSensorType":
                    Response.Write(this.DeleteSensorType(Public.Request("typeId|id", 0)));
                    break;
                #endregion

                #region  传感器通道原始值类型
                case "editSensorOriginalType":
                    Response.Write(this.EditSensorOriginalType(Public.Request("data", "{}")));
                    break;
                case "getSensorOriginalType":
                    Response.Write(this.GetSensorOriginalType(Public.Request("typeId|id", 0)));
                    break;
                case "getSensorOriginalTypeList":
                    Response.Write(this.GetSensorOriginalTypeList(Public.Request("data", "{}")));
                    break;
                case "getSensorOriginalTypeTree":
                    Response.Write(this.GetSensorOriginalTypeTree(Public.Request("data", "{}")));
                    break;
                case "deleteSensorOriginalType":
                    Response.Write(this.DeleteSensorOriginalType(Public.Request("typeId|id", 0)));
                    break;
                #endregion

                #region  传感器通道类型
                case "editSensorChannelMode":
                    Response.Write(this.EditSensorChannelMode(Public.Request("data", "{}")));
                    break;
                case "getSensorChannelMode":
                    Response.Write(this.GetSensorChannelMode(Public.Request("channelId|id", 0)));
                    break;
                case "getSensorChannelModeList":
                    Response.Write(this.GetSensorChannelModeList(Public.Request("data", "{}")));
                    break;
                case "getSensorChannelModeTree":
                    Response.Write(this.GetSensorChannelModeTree(Public.Request("data", "{}")));
                    break;
                #endregion

                #region  传感器通道
                case "editSensorChannel":
                    Response.Write(this.EditSensorChannel(Public.Request("data", "{}")));
                    break;
                case "getSensorChannel":
                    Response.Write(this.GetSensorChannel(Public.Request("channelId|id", 0)));
                    break;
                case "getSensorChannelList":
                    Response.Write(this.GetSensorChannelList(Public.Request("data", "{}")));
                    break;
                case "getSensorChannelTree":
                    Response.Write(this.GetSensorChannelTree(Public.Request("data", "{}")));
                    break;
                case "deleteSensorChannel":
                    Response.Write(this.DeleteSensorChannel(Public.Request("channelId|id", 0)));
                    break;
                #endregion

                #region  传感器参数
                case "editSensorParam":
                    Response.Write(this.EditSensorParam(Public.Request("data", "{}")));
                    break;
                case "getSensorParam":
                    Response.Write(this.GetSensorParam(Public.Request("paramId|id", 0)));
                    break;
                case "getSensorParamList":
                    Response.Write(this.GetSensorParamList(Public.Request("data", "{}")));
                    break;
                case "getSensorParamTree":
                    Response.Write(this.GetSensorParamTree(Public.Request("data", "{}")));
                    break;
                case "deleteSensorParam":
                    Response.Write(this.DeleteSensorParam(Public.Request("paramId|id", 0)));
                    break;
                #endregion

                #region  通道-参数
                case "editChannelParam":
                    Response.Write(this.EditChannelParam(Public.Request("data", true, "{}")));
                    break;
                case "getChannelParamConfig":
                    Response.Write(this.GetChannelParamConfig(Public.Request("data", true, "{}")));
                    break;
                #endregion
                
                #region  传感器设备版本
                case "editSensorDeviceVersion":
                    Response.Write(this.EditSensorDeviceVersion(Public.Request("data", "{}")));
                    break;
                case "getSensorDeviceVersion":
                    Response.Write(this.GetSensorDeviceVersion(Public.Request("versionId|id", 0)));
                    break;
                case "getSensorDeviceVersionList":
                    Response.Write(this.GetSensorDeviceVersionList(Public.Request("data", "{}")));
                    break;
                case "deleteSensorDeviceVersion":
                    Response.Write(this.DeleteSensorDeviceVersion(Public.Request("versionId|id", 0)));
                    break;
                #endregion

                #region  传感器数据
                case "getSensorData":
                    Response.Write(this.GetSensorData(Public.Request("id", 0)));
                    break;
                case "getSensorDataList":
                    Response.Write(this.GetSensorDataList(Public.Request("data", "{}")));
                    break;
                #endregion
                    
                default:
                    Response.Write(Public.ToJsonHello());
                    break;
            }
        }
        catch (Exception ex)
        {
            ServerLog.WriteErrorLog(ex);
            Response.Write(Public.ToExceptionResult(ex));
        }
    }
    #endregion


    #region  传感器分类

    #region  编辑传感器分类
    public string EditSensorType(string data)
    {
        try
        {
            SensorTypeManage dm = new SensorTypeManage();

            SensorTypeInfo o = Public.Json.Deserialize<SensorTypeInfo>(data);

            if (dm.CheckNameIsExist(o.TypeName, o.SensorTypeId))
            {
                return Public.ToJsonMessage("对不起，已存在相同的分类名称");
            }
            else if (dm.CheckCodeIsExist(o.TypeCode, o.SensorTypeId))
            {
                return Public.ToJsonMessage("对不起，已存在相同的分类编码");
            }

            //设置层级
            o.Level = (dm.GetLevel(o.ParentId) + 1);

            int result = 0;
            if (o.SensorTypeId > 0)
            {
                result = dm.UpdateSensorType(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddSensorType(o).Result;
                if (result > 0)
                {
                    o.SensorTypeId = result;
                }
            }
            if (result > 0)
            {
                dm.UpdateParentTree(o.SensorTypeId);
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器分类
    public string GetSensorType(int typeId)
    {
        try
        {
            SensorTypeManage dm = new SensorTypeManage();

            SensorTypeInfo o = dm.GetSensorTypeInfo(typeId);

            return Public.ToJsonData(o);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器分类
    public string GetSensorTypeList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);

            SensorTypeManage dm = new SensorTypeManage();

            int dataCount = 0;
            List<SensorTypeInfo> list = dm.GetSensorTypeInfo(dm.GetSensorType(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  获得传感器分类树
    public string GetSensorTypeTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            SensorTypeManage dm = new SensorTypeManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"SensorTypeId", "id"},{"TypeName","name"},{"ParentId","pid"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetSensorTypeInfo(dm.GetSensorType(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除传感器分类
    public string DeleteSensorType(int id)
    {
        try
        {
            SensorTypeManage dm = new SensorTypeManage();

            int[] cc = dm.GetSensorDataCount(id);
            if (cc[0] > 0 || cc[1] > 0)
            {
                string msg = String.Format("{0}{1}", cc[0] > 0 ? cc + "个分类，" : "", cc[1] > 0 ? cc[1] + "个传感器，" : "");
                return Public.ToJsonMessage(String.Format("对不起，该分类包含{0}不能删除。", msg));
            }
            int result = dm.DeleteSensorType(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  传感器通道原始值类型

    #region  编辑传感器通道原始值类型
    public string EditSensorOriginalType(string data)
    {
        try
        {
            SensorOriginalTypeManage dm = new SensorOriginalTypeManage();

            SensorOriginalTypeInfo o = Public.Json.Deserialize<SensorOriginalTypeInfo>(data);

            int result = 0;
            if (o.OriTypeId > 0)
            {
                result = dm.UpdateSensorOriginalType(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddSensorOriginalType(o).Result;
                if (result > 0)
                {
                    o.OriTypeId = result;
                }
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道原始值类型
    public string GetSensorOriginalType(int typeId)
    {
        try
        {
            SensorOriginalTypeManage dm = new SensorOriginalTypeManage();

            SensorOriginalTypeInfo o = dm.GetSensorOriginalTypeInfo(typeId);

            return Public.ToJsonData(o);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道原始值类型
    public string GetSensorOriginalTypeList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);

            SensorOriginalTypeManage dm = new SensorOriginalTypeManage();

            int dataCount = 0;
            List<SensorOriginalTypeInfo> list = dm.GetSensorOriginalTypeInfo(dm.GetSensorOriginalType(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道原始值类型树
    public string GetSensorOriginalTypeTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            SensorOriginalTypeManage dm = new SensorOriginalTypeManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"OriTypeId", "id"},{"OriTypeName","name"},{"OriTypeCode","code"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetSensorOriginalTypeInfo(dm.GetSensorOriginalType(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除传感器通道原始值类型
    public string DeleteSensorOriginalType(int id)
    {
        try
        {
            SensorOriginalTypeManage dm = new SensorOriginalTypeManage();
            
            int cc = dm.GetSensorOriginalTypeUseCount(id);
            if (cc > 0)
            {
                return Public.ToJsonMessage(String.Format("对不起，该原始值已被使用，不能删除。"));
            }
            int result = dm.DeleteSensorOriginalType(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  传感器通道

    #region  编辑传感器通道
    public string EditSensorChannel(string data)
    {
        try
        {
            SensorChannelManage dm = new SensorChannelManage();

            SensorChannelInfo o = Public.Json.Deserialize<SensorChannelInfo>(data);

            int result = 0;
            if (o.ChannelId > 0)
            {
                result = dm.UpdateSensorChannel(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddSensorChannel(o).Result;
                if (result > 0)
                {
                    o.ChannelId = result;
                }
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道
    public string GetSensorChannel(int channelId)
    {
        try
        {
            SensorChannelManage dm = new SensorChannelManage();

            SensorChannelInfo o = dm.GetSensorChannelInfo(channelId);

            return Public.ToJsonData(o);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道
    public string GetSensorChannelList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);

            SensorChannelManage dm = new SensorChannelManage();

            int dataCount = 0;
            List<SensorChannelInfo> list = dm.GetSensorChannelInfo(dm.GetSensorChannel(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道树
    public string GetSensorChannelTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            SensorChannelManage dm = new SensorChannelManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"ChannelId", "id"},{"ChannelNo","cn"},{"ChannelType","type"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetSensorChannelInfo(dm.GetSensorChannel(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除传感器通道
    public string DeleteSensorChannel(int id)
    {
        try
        {
            SensorChannelManage dm = new SensorChannelManage();
            /*
            int[] cc = dm.GetSensorDataCount(id);
            if (cc[0] > 0 || cc[1] > 0)
            {
                string msg = String.Format("{0}{1}", cc[0] > 0 ? cc + "个分类，" : "", cc[1] > 0 ? cc[1] + "个传感器，" : "");
                return Public.ToJsonMessage(String.Format("对不起，该分类包含{0}不能删除。", msg));
            }*/
            int result = dm.DeleteSensorChannel(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion
    
    #region  传感器参数

    #region  编辑传感器参数
    public string EditSensorParam(string data)
    {
        try
        {
            SensorParamManage dm = new SensorParamManage();

            SensorParamInfo o = Public.Json.Deserialize<SensorParamInfo>(data);

            int result = 0;
            if (o.ParamId > 0)
            {
                result = dm.UpdateSensorParam(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddSensorParam(o).Result;
                if (result > 0)
                {
                    o.ParamId = result;
                }
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器参数
    public string GetSensorParam(int channelId)
    {
        try
        {
            SensorParamManage dm = new SensorParamManage();

            SensorParamInfo o = dm.GetSensorParamInfo(channelId);

            return Public.ToJsonData(o);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器参数
    public string GetSensorParamList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);

            SensorParamManage dm = new SensorParamManage();

            int dataCount = 0;
            List<SensorParamInfo> list = dm.GetSensorParamInfo(dm.GetSensorParam(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器参数树
    public string GetSensorParamTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            SensorParamManage dm = new SensorParamManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"ParamId", "id"},{"ParamName","name"},{"ParamCode","code"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetSensorParamInfo(dm.GetSensorParam(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除传感器参数
    public string DeleteSensorParam(int id)
    {
        try
        {
            SensorParamManage dm = new SensorParamManage();
            
            int cc = dm.GetSensorParamUseCount(id);
            if (cc > 0)
            {
                return Public.ToJsonMessage(String.Format("对不起，该参数已被使用，不能删除。"));
            }
            int result = dm.DeleteSensorParam(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion
    
    #region  传感器通道类型

    #region  编辑传感器通道类型
    public string EditSensorChannelMode(string data)
    {
        try
        {
            SensorChannelModeManage dm = new SensorChannelModeManage();

            SensorChannelModeInfo o = Public.Json.Deserialize<SensorChannelModeInfo>(data);

            int result = 0;
            if (o.ModeId > 0)
            {
                result = dm.UpdateSensorChannelMode(o).Result;
            }
            else
            {
                result = dm.AddSensorChannelMode(o).Result;
                if (result > 0)
                {
                    o.ModeId = result;
                }
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道类型
    public string GetSensorChannelMode(int modeId)
    {
        try
        {
            SensorChannelModeManage dm = new SensorChannelModeManage();

            SensorChannelModeInfo o = dm.GetSensorChannelModeInfo(modeId);

            return Public.ToJsonData(o);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道类型
    public string GetSensorChannelModeList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);

            SensorChannelModeManage dm = new SensorChannelModeManage();

            int dataCount = 0;
            List<SensorChannelModeInfo> list = dm.GetSensorChannelModeInfo(dm.GetSensorChannelMode(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器通道类型树
    public string GetSensorChannelModeTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            SensorChannelModeManage dm = new SensorChannelModeManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"ModeId", "id"},{"ModeName","name"},{"ModeCode","code"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetSensorChannelModeInfo(dm.GetSensorChannelMode(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #endregion
    
    #region  通道-参数

    #region  获取通道-参数
    public string GetChannelParamConfig(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            SensorChannelParamManage dm = new SensorChannelParamManage();

            if (Public.ConvertValue(par, "ChannelNo", 0) <= 0)
            {
                return Public.ToJsonMessage("未指定通道");
            }

            Dictionary<string, object> channel = new Dictionary<string, object>();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> config = new List<Dictionary<string, object>>();

            DataSet ds = dm.GetChannelParamConfig(par).DataSet;
            //通道信息
            if (Public.CheckTable(ds, 0))
            {
                channel = Public.FillDataValue(ds.Tables[0].Rows[0]);
            }
            //参数列表
            if (Public.CheckTable(ds, 1))
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    list.Add(Public.FillDataValue(dr, true));
                }
            }
            //通道-参数
            if (Public.CheckTable(ds, 2))
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    config.Add(Public.FillDataValue(dr, true));
                }
            }

            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                {"channel", channel},{"list",list},{"config",config}
            };

            return Public.ToJsonData(d);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  编辑通道-参数
    public string EditChannelParam(string data)
    {
        try
        {
            SensorChannelParamManage dm = new SensorChannelParamManage();

            Dictionary<string, object> dic = Public.Json.Deserialize<Dictionary<string, object>>(data);

            int channelNo = Public.ConvertValue(dic, "ChannelNo", 0);
            string paramIdList = Public.ConvertValue(dic, "ParamIdList");
            int channelGroup = Public.ConvertValue(dic, "ChannelGroup", 1);

            string createTime = Public.GetDateTime();

            if (channelNo <= 0)
            {
                return Public.ToJsonMessage("未指定通道");
            }

            dm.DeleteSensorChannelParam(channelNo, -1);
            dm.BatchAddChannelParam(channelNo, paramIdList, channelGroup, createTime);

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion
    
    #region  传感器设备版本

    #region  编辑传感器设备版本
    public string EditSensorDeviceVersion(string data)
    {
        try
        {
            SensorDeviceVersionManage dm = new SensorDeviceVersionManage();

            SensorDeviceVersionInfo o = Public.Json.Deserialize<SensorDeviceVersionInfo>(data);

            int result = 0;
            if (o.VersionId > 0)
            {
                result = dm.UpdateSensorDeviceVersion(o).Result;
            }
            else
            {
                result = dm.AddSensorDeviceVersion(o).Result;
                if (result > 0)
                {
                    o.VersionId = result;
                }
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器设备版本
    public string GetSensorDeviceVersion(int versionId)
    {
        try
        {
            SensorDeviceVersionManage dm = new SensorDeviceVersionManage();

            SensorDeviceVersionInfo o = dm.GetSensorDeviceVersionInfo(versionId);

            return Public.ToJsonData(o);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器设备版本
    public string GetSensorDeviceVersionList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);

            SensorDeviceVersionManage dm = new SensorDeviceVersionManage();

            int dataCount = 0;
            List<SensorDeviceVersionInfo> list = dm.GetSensorDeviceVersionInfo(dm.GetSensorDeviceVersion(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器设备版本树
    public string GetSensorDeviceVersionTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            SensorDeviceVersionManage dm = new SensorDeviceVersionManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"VersionId", "id"},{"VersionCode","code"},{"VersionConfig","config"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetSensorDeviceVersionInfo(dm.GetSensorDeviceVersion(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除传感器设备版本
    public string DeleteSensorDeviceVersion(int id)
    {
        try
        {
            SensorDeviceVersionManage dm = new SensorDeviceVersionManage();

            int result = dm.DeleteSensorDeviceVersion(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  传感器数据

    #region  获得传感器数据
    public string GetSensorData(int id)
    {
        try
        {
            SensorDataManage dm = new SensorDataManage();

            SensorDataInfo o = dm.GetSensorDataInfo(id);

            return Public.ToJsonData(o);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得传感器数据
    public string GetSensorDataList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);

            SensorDataManage dm = new SensorDataManage();

            int dataCount = 0;
            List<SensorDataInfo> list = dm.GetSensorDataInfo(dm.GetSensorData(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #endregion


}