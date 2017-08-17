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
using OMIS.BLL.Common;
using OMIS.Model;
using OMIS.Model.System;
using OMIS.Model.Common;

public partial class ajax_system_system : System.Web.UI.Page
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
                #region  导航菜单
                case "editMenu":
                    Response.Write(this.EditMenu(Public.Request("data", true)));
                    break;
                case "getMenu":
                    Response.Write(this.GetMenuInfo(Public.Request("menuId|id", 0)));
                    break;
                case "getMenuList":
                    Response.Write(this.GetMenuList(Public.Request("data", true, "{}")));
                    break;
                case "deleteMenu":
                    Response.Write(this.DeleteMenu(Public.Request("menuId|id", 0)));
                    break;
                case "getMenuTree":
                    Response.Write(this.GetMenuTree(Public.Request("data", true, "{}")));
                    break;
                case "updateMenuPhoto":
                    Response.Write(this.UpdateMenuPhoto(
                        Public.Request("path"), Public.Request("menuId|id", 0)
                    ));
                    break;
                #endregion

                #region  模块菜单
                case "editModuleMenu":
                    Response.Write(this.EditModuleMenu(Public.Request("data", true)));
                    break;
                case "getModuleMenu":
                    Response.Write(this.GetModuleMenuInfo(Public.Request("menuId|id", 0)));
                    break;
                case "getModuleMenuList":
                    Response.Write(this.GetModuleMenuList(Public.Request("data", true, "{}")));
                    break;
                case "deleteModuleMenu":
                    Response.Write(this.DeleteModuleMenu(Public.Request("menuId|id", 0)));
                    break;

                case "getModuleMenuToParent":
                    Response.Write(this.GetModuleMenuListToParent(Public.Request("data", true, "{}")));
                    break;
                case "getModuleMenuTree":
                    Response.Write(this.GetModuleMenuTree(Public.Request("data", true, "{}")));
                    break;
                case "updateModuleMenuPhoto":
                    Response.Write(this.UpdateModuleMenuPhoto(
                        Public.Request("path"), Public.Request("menuId|id", 0)
                    ));
                    break;
                #endregion

                #region  字典分类
                case "editDictionaryType":
                    Response.Write(this.EditDictionaryType(Public.Request("data", true)));
                    break;
                case "getDictionaryType":
                    Response.Write(this.GetDictionaryTypeInfo(Public.Request("typeId|id", 0)));
                    break;
                case "getDictionaryTypeList":
                    Response.Write(this.GetDictionaryTypeList(Public.Request("data", true, "{}")));
                    break;
                case "getDictionaryTypeToParent":
                case "getDictionaryTypeTree":
                    Response.Write(this.GetDictionaryTypeToParent(Public.Request("data", true, "{}")));
                    break;
                case "deleteDictionaryType":
                    Response.Write(this.DeleteDictionaryType(Public.Request("typeId|id", 0)));
                    break;
                #endregion

                #region  字典
                case "editDictionary":
                    Response.Write(this.EditDictionary(Public.Request("data", true)));
                    break;
                case "getDictionary":
                    Response.Write(this.GetDictionaryInfo(Public.Request("dictoinaryId|id", 0)));
                    break;
                case "getDictionaryList":
                    Response.Write(this.GetDictionaryList(Public.Request("data", true, "{}")));
                    break;
                case "deleteDictionary":
                    Response.Write(this.DeleteDictionary(Public.Request("dictoinaryId|id", 0)));
                    break;
                #endregion

                #region  权限分类
                case "editPermissionType":
                    Response.Write(this.EditPermissionType(Public.Request("data", true)));
                    break;
                case "getPermissionType":
                    Response.Write(this.GetPermissionTypeInfo(Public.Request("typeId|id", 0)));
                    break;
                case "getPermissionTypeList":
                    Response.Write(this.GetPermissionTypeList(Public.Request("data", true, "{}")));
                    break;
                case "getPermissionTypeTree":
                    Response.Write(this.GetPermissionTypeTree(Public.Request("data", true, "{}")));
                    break;
                case "deletePermissionType":
                    Response.Write(this.DeletePermissionType(Public.Request("typeId|id", 0)));
                    break;
                #endregion

                #region  权限
                case "editPermission":
                    Response.Write(this.EditPermission(Public.Request("data", true)));
                    break;
                case "getPermission":
                    Response.Write(this.GetPermissionInfo(Public.Request("permissionId|id", 0)));
                    break;
                case "getPermissionList":
                    Response.Write(this.GetPermissionList(Public.Request("data", true, "{}")));
                    break;
                case "deletePermission":
                    Response.Write(this.DeletePermission(Public.Request("permissionId|id", 0)));
                    break;
                #endregion

                #region  模块
                case "editModule":
                    Response.Write(this.EditModule(Public.Request("data", true)));
                    break;
                case "getModule":
                    Response.Write(this.GetModuleInfo(Public.Request("moduleId|id", 0)));
                    break;
                case "getModuleList":
                    Response.Write(this.GetModuleList(Public.Request("data", true, "{}")));
                    break;
                case "getModuleToParent":
                    Response.Write(this.GetModuleListToParent(Public.Request("data", true, "{}")));
                    break;
                case "getModuleTree":
                    Response.Write(this.GetModuleTree(Public.Request("data", true, "{}")));
                    break;
                case "deleteModule":
                    Response.Write(this.DeleteModule(Public.Request("moduleId|id", 0)));
                    break;
                #endregion

                #region  模块-权限
                case "getModulePermissionConfig":
                    Response.Write(this.GetModulePermissionConfig(Public.Request("data", true, "{}")));
                    break;
                case "editModulePermission":
                    Response.Write(this.EditModulePermission(Public.Request("data", true, "{}")));
                    break;
                #endregion

                #region  角色组别
                case "getRoleGroup":
                    Response.Write(this.GetRoleGroup(Public.Request("data", true, "{}")));
                    break;

                #endregion

                #region  角色
                case "editRole":
                    Response.Write(this.EditRole(Public.Request("data", true)));
                    break;
                case "getRole":
                    Response.Write(this.GetRoleInfo(Public.Request("roleId|id", 0)));
                    break;
                case "getRoleList":
                    Response.Write(this.GetRoleList(Public.Request("data", true, "{}")));
                    break;
                case "deleteRole":
                    Response.Write(this.DeleteRole(Public.Request("roleId|id", 0)));
                    break;
                #endregion

                #region  角色-模块-权限
                case "editRoleModulePermission":
                    Response.Write(this.EditRoleModulePermission(Public.Request("data", true, "{}")));
                    break;
                case "getRoleModulePermissionConfig":
                    Response.Write(this.GetRoleModulePermissionConfig(Public.Request("data", true, "{}")));
                    break;
                #endregion

                #region  角色-导航菜单
                case "editRoleMenu":
                    Response.Write(this.EditRoleMenu(Public.Request("data", true, "{}")));
                    break;
                case "getRoleMenuConfig":
                    Response.Write(this.GetRoleMenuConfig(Public.Request("data", true, "{}")));
                    break;
                #endregion

                #region  角色-模块菜单
                case "editRoleModuleMenu":
                    Response.Write(this.EditRoleModuleMenu(Public.Request("data", true, "{}")));
                    break;
                case "getRoleModuleMenuConfig":
                    Response.Write(this.GetRoleModuleMenuConfig(Public.Request("data", true, "{}")));
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

    #region  导航菜单

    #region  编辑导航菜单
    public string EditMenu(string data)
    {
        try
        {
            MenuInfo o = Public.Json.Deserialize<MenuInfo>(data);
            MenuManage dm = new MenuManage();

            if (dm.CheckNameIsExist(o.MenuName, o.MenuId))
            {
                return Public.ToJsonMessage("对不起，已存在相同的菜单名称");
            }
            else if(dm.CheckCodeIsExist(o.MenuCode,o.MenuId))
            {
                return Public.ToJsonMessage("对不起，已存在相同的菜单编码");
            }
            int result = 0;
            if (o.MenuId > 0)
            {
                result = dm.UpdateMenu(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddMenu(o).Result;
                if (result > 0)
                {
                    o.MenuId = result;
                }
            }

            return Public.ToJsonId(o.MenuId);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得导航菜单
    public string GetMenuInfo(int menuId)
    {
        try
        {
            MenuInfo o = new MenuManage().GetMenuInfo(menuId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的导航菜单");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得导航菜单列表
    public string GetMenuList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            MenuManage dm = new MenuManage();

            int dataCount = 0;
            List<MenuInfo> list = dm.GetMenuInfo(dm.GetMenu(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除导航菜单
    public string DeleteMenu(int id)
    {
        try
        {
            MenuManage dm = new MenuManage();

            MenuInfo o = dm.GetMenuInfo(id);
            if (o != null)
            {
                if (o.MenuUrl.Trim().Length > 0)
                {
                    return Public.ToJsonMessage(String.Format("对不起，要删除菜单，请先清除菜单URL地址。"));
                }
            }

            int result = dm.DeleteMenu(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得导航菜单(树菜单)
    public string GetMenuTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            if (!par.ContainsKey("Enabled"))
            {
                par.Add("Enabled", 1);
            }
            MenuManage dm = new MenuManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"MenuId", "id"},{"MenuName","name"},{"MenuCode","code"},{"Enabled","use"},{"MenuUrl","url"},{"MenuPic","pic"}
            };

            List<Dictionary<string, object>> list = dm.GetMenuInfo(dm.GetMenu(par).DataSet, dicField);
            
            return Public.ToJsonTree(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  更新菜单图片
    public string UpdateMenuPhoto(string path, int menuId)
    {
        try
        {
            int result = new MenuManage().UpdateMenuPhoto(path, menuId).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  模块菜单

    #region  编辑模块菜单
    public string EditModuleMenu(string data)
    {
        try
        {
            ModuleMenuManage dm = new ModuleMenuManage();

            ModuleMenuInfo o = Public.Json.Deserialize<ModuleMenuInfo>(data);

            //设置层级
            o.Level = (dm.GetLevel(o.ParentId) + 1);

            int result = 0;
            if (o.MenuId > 0)
            {
                result = dm.UpdateModuleMenu(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddModuleMenu(o).Result;
                if (result > 0)
                {
                    o.MenuId = result;
                }
            }
            if (result > 0)
            {
                dm.UpdateParentTree(o.MenuId);
            }

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得模块菜单
    public string GetModuleMenuInfo(int menuId)
    {
        try
        {
            ModuleMenuInfo o = new ModuleMenuManage().GetModuleMenuInfo(menuId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的模块菜单");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得模块菜单列表
    public string GetModuleMenuList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            ModuleMenuManage dm = new ModuleMenuManage();

            int dataCount = 0;
            List<ModuleMenuInfo> list = dm.GetModuleMenuInfo(dm.GetModuleMenu(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除模块菜单
    public string DeleteModuleMenu(int id)
    {
        try
        {
            ModuleMenuManage dm = new ModuleMenuManage();

            int cc = dm.GetMenuChildCount(id);
            if (cc > 0)
            {
                return Public.ToJsonMessage(String.Format("对不起，该菜单包含{0}个子菜单，不能删除。", cc));
            }

            ModuleMenuInfo o = dm.GetModuleMenuInfo(id);
            if (o != null)
            {
                if (o.MenuUrl.Trim().Length > 0)
                {
                    return Public.ToJsonMessage(String.Format("对不起，要删除菜单，请先清除菜单URL地址。"));
                }
            }

            int result = dm.DeleteModuleMenu(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得模块菜单（上级菜单列表）
    public string GetModuleMenuListToParent(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            ModuleMenuManage dm = new ModuleMenuManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"MenuId", "id"},{"MenuName","name"},{"ParentId","pid"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetModuleMenuInfo(dm.GetModuleMenu(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  获得模块菜单（树菜单）
    public string GetModuleMenuTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            if (!par.ContainsKey("Enabled"))
            {
                par.Add("Enabled", 1);
            }

            ModuleMenuManage dm = new ModuleMenuManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"MenuId", "id"},{"MenuName","name"},{"ParentId","pid"},{"Enabled","use"},{"MenuUrl","url"},{"MenuPic","pic"},
                {"OpenType","open"},{"MenuType","type"},{"MenuCode","code"}
            };

            int dc =0;

            List<Dictionary<string, object>> list = dm.GetModuleMenuInfo(dm.GetModuleMenu(par).DataSet, dicField, out dc, true);

            return Public.ToJsonTree(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  更新模块菜单图片
    public string UpdateModuleMenuPhoto(string path, int menuId)
    {
        try
        {
            int result = new ModuleMenuManage().UpdateMenuPhoto(path, menuId).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion


    #region  字典分类

    #region  编辑字典分类
    public string EditDictionaryType(string data)
    {
        try
        {
            DictionaryTypeManage dm = new DictionaryTypeManage();

            DictionaryTypeInfo o = Public.Json.Deserialize<DictionaryTypeInfo>(data);

            //设置层级
            o.Level = (dm.GetLevel(o.ParentId) + 1);

            int result = 0;
            if (o.TypeId > 0)
            {
                result = dm.UpdateDictionaryType(o).Result;
            }
            else
            {
                o.OperatorId = new UserCenter().LoginUserId;

                result = dm.AddDictionaryType(o).Result;
                if (result > 0)
                {
                    o.TypeId = result;
                }
            }
            if (result > 0)
            {
                dm.UpdateParentTree(o.TypeId);
            }

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得字典分类
    public string GetDictionaryTypeInfo(int typeId)
    {
        try
        {
            DictionaryTypeInfo o = new DictionaryTypeManage().GetDictionaryTypeInfo(typeId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的字典分类");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得字典分类列表
    public string GetDictionaryTypeList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            DictionaryTypeManage dm = new DictionaryTypeManage();

            int dataCount = 0;
            List<DictionaryTypeInfo> list = dm.GetDictionaryTypeInfo(dm.GetDictionaryType(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得字典分类(上级分类列表)
    public string GetDictionaryTypeToParent(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            DictionaryTypeManage dm = new DictionaryTypeManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"TypeId", "id"},{"TypeName","name"},{"ParentId","pid"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetDictionaryTypeInfo(dm.GetDictionaryType(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  删除字典分类
    public string DeleteDictionaryType(int id)
    {
        try
        {
            DictionaryTypeManage dm = new DictionaryTypeManage();

            int[] cc = dm.GetDictionaryDataCount(id);
            if (cc[0] > 0 || cc[1] > 0)
            {
                string msg = String.Format("{0}{1}", cc[0] > 0 ? cc[0] + "个子分类，" : "", cc[1] > 0 ? cc[1] + "个字典，" : "");
                return Public.ToJsonMessage(String.Format("对不起，该分类包含{0}不能删除。", msg));
            }
            int result = dm.DeleteDictionaryType(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #endregion

    #region  字典

    #region  编辑字典
    public string EditDictionary(string data)
    {
        try
        {
            DictionaryManage dm = new DictionaryManage();
            DictionaryTypeManage dtm = new DictionaryTypeManage();

            DictionaryInfo o = Public.Json.Deserialize<DictionaryInfo>(data);
            
            int result = 0;
            if (o.DictionaryId > 0)
            {
                result = dm.UpdateDictionary(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;
                o.DictionaryNumber = dtm.GetMaxNumber(o.TypeId) + 1;

                result = dm.AddDictionary(o).Result;
                if (result > 0)
                {
                    o.DictionaryId = result;

                    dtm.UpdateMaxNumber(o.TypeId);
                }
            }

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得字典
    public string GetDictionaryInfo(int dictionaryId)
    {
        try
        {
            DictionaryInfo o = new DictionaryManage().GetDictionaryInfo(dictionaryId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的字典");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得字典列表
    public string GetDictionaryList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            DictionaryManage dm = new DictionaryManage();

            int dataCount = 0;
            List<DictionaryInfo> list = dm.GetDictionaryInfo(dm.GetDictionary(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  删除字典
    public string DeleteDictionary(int id)
    {
        try
        {
            DictionaryManage dm = new DictionaryManage();

            int result = dm.DeleteDictionary(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion


    #region  权限分类

    #region  编辑权限分类
    public string EditPermissionType(string data)
    {
        try
        {
            PermissionTypeManage dm = new PermissionTypeManage();

            PermissionTypeInfo o = Public.Json.Deserialize<PermissionTypeInfo>(data);

            int result = 0;
            if (o.TypeId > 0)
            {
                result = dm.UpdatePermissionType(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddPermissionType(o).Result;
                if (result > 0)
                {
                    o.TypeId = result;
                }
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得权限分类
    public string GetPermissionTypeInfo(int typeId)
    {
        try
        {
            PermissionTypeInfo o = new PermissionTypeManage().GetPermissionTypeInfo(typeId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的权限分类");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得权限分类列表
    public string GetPermissionTypeList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            PermissionTypeManage dm = new PermissionTypeManage();

            int dataCount = 0;
            List<PermissionTypeInfo> list = dm.GetPermissionTypeInfo(dm.GetPermissionType(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得权限分类树
    public string GetPermissionTypeTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            PermissionTypeManage dm = new PermissionTypeManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"TypeId", "id"},{"TypeName","name"},{"ParentId","pid"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetPermissionTypeInfo(dm.GetPermissionType(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除权限分类
    public string DeletePermissionType(int id)
    {
        try
        {
            PermissionTypeManage dm = new PermissionTypeManage();

            int cc = dm.GetPermissionDataCount(id);
            if (cc > 0)
            {
                string msg = String.Format("{0}", cc > 0 ? cc + "个权限，" : "");
                return Public.ToJsonMessage(String.Format("对不起，该分类包含{0}不能删除。", msg));
            }
            int result = dm.DeletePermissionType(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion
    
    #region  权限

    #region  编辑权限
    public string EditPermission(string data)
    {
        try
        {
            PermissionManage dm = new PermissionManage();

            PermissionInfo o = Public.Json.Deserialize<PermissionInfo>(data);

            int result = 0;
            if (o.PermissionId > 0)
            {
                result = dm.UpdatePermission(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddPermission(o).Result;
                if (result > 0)
                {
                    o.PermissionId = result;
                }
            }
            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得权限
    public string GetPermissionInfo(int permissionId)
    {
        try
        {
            PermissionInfo o = new PermissionManage().GetPermissionInfo(permissionId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的权限");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得权限列表
    public string GetPermissionList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            PermissionManage dm = new PermissionManage();

            int dataCount = 0;
            List<PermissionInfo> list = dm.GetPermissionInfo(dm.GetPermission(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除权限
    public string DeletePermission(int id)
    {
        try
        {
            PermissionManage dm = new PermissionManage();

            int cc = dm.GetPermissionDataCount(id);
            if (cc > 0)
            {
                string msg = String.Format("{0}", cc > 0 ? cc + "个模块应用，" : "");
                return Public.ToJsonMessage(String.Format("对不起，该权限已被{0}不能删除。", msg));
            }
            int result = dm.DeletePermission(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion


    #region  模块

    #region  编辑模块
    public string EditModule(string data)
    {
        try
        {
            ModuleManage dm = new ModuleManage();

            ModuleInfo o = Public.Json.Deserialize<ModuleInfo>(data);

            //设置层级
            o.Level = (dm.GetLevel(o.ParentId) + 1);

            int result = 0;
            if (o.ModuleId > 0)
            {
                result = dm.UpdateModule(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddModule(o).Result;
                if (result > 0)
                {
                    o.ModuleId = result;
                }
            }
            if (result > 0)
            {
                dm.UpdateParentTree(o.ModuleId);
            }

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得模块
    public string GetModuleInfo(int moduleId)
    {
        try
        {
            ModuleInfo o = new ModuleManage().GetModuleInfo(moduleId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的模块");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得模块列表
    public string GetModuleList(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            ModuleManage dm = new ModuleManage();

            int dataCount = 0;
            List<ModuleInfo> list = dm.GetModuleInfo(dm.GetModule(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除模块
    public string DeleteModule(int id)
    {
        try
        {
            ModuleManage dm = new ModuleManage();

            int cc = dm.GetModuleChildCount(id);
            if (cc > 0)
            {
                return Public.ToJsonMessage(String.Format("对不起，该菜单包含{0}个子模块，不能删除。", cc));
            }
            
            int result = dm.DeleteModule(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得模块（上级模块列表）
    public string GetModuleListToParent(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            ModuleManage dm = new ModuleManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"ModuleId", "id"},{"ModuleName","name"},{"ParentId","pid"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetModuleInfo(dm.GetModule(par).DataSet, dicField);

            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得模块（树菜单）
    public string GetModuleTree(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            if (!par.ContainsKey("Enabled"))
            {
                par.Add("Enabled", 1);
            }

            ModuleManage dm = new ModuleManage();

            Dictionary<string, string> dicField = new Dictionary<string, string>()
            {
                {"ModuleId", "id"},{"ModuleName","name"},{"ParentId","pid"},{"Enabled","use"}
            };

            List<Dictionary<string, object>> list = dm.GetModuleInfo(dm.GetModule(par).DataSet, dicField);

            return Public.ToJsonTree(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  模块-权限

    #region  获得模块-权限配置数据
    public string GetModulePermissionConfig(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            ModulePermissionManage dm = new ModulePermissionManage();

            if (Public.ConvertValue(par, "ModuleId", 0) <= 0)
            {
                return Public.ToJsonMessage("未指定模块");
            }

            Dictionary<string, object> module = new Dictionary<string, object>();
            List<Dictionary<string, object>> type = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> config = new List<Dictionary<string, object>>();

            DataSet ds = dm.GetModulePermissionConfig(par).DataSet;
            //模块信息
            if (Public.CheckTable(ds, 0))
            {
                module = Public.FillDataValue(ds.Tables[0].Rows[0]);
            }
            //权限分类
            if (Public.CheckTable(ds, 1))
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    type.Add(Public.FillDataValue(dr, true));
                }
            }
            //权限
            if (Public.CheckTable(ds, 2))
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    list.Add(Public.FillDataValue(dr, true));
                }
            }

            //模块-权限
            if (Public.CheckTable(ds, 3))
            {
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    config.Add(Public.FillDataValue(dr, true));
                }
            }

            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                {"module",module},{"type", type},{"list",list},{"config",config}
            };
            
            return Public.ToJsonData(d);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  编辑模块-权限配置
    public string EditModulePermission(string data)
    {
        try
        {
            ModulePermissionManage dm = new ModulePermissionManage();

            Dictionary<string, object> dic = Public.Json.Deserialize<Dictionary<string, object>>(data);

            int moduleId = Public.ConvertValue(dic, "ModuleId", 0);
            string permissionIdList = Public.ConvertValue(dic, "PermissionIdList");
            int operatorId = uc.LoginUserId;
            string createTime = Public.GetDateTime();

            if (moduleId <= 0)
            {
                return Public.ToJsonMessage("未指定模块");
            }

            dm.DeleteModulePermission(moduleId, -1);
            dm.BatchAddModulePermission(moduleId, permissionIdList, operatorId, createTime);            

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  角色组别
    public string GetRoleGroup(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            
            RoleGroupManage dm = new RoleGroupManage();

            int dataCount = 0;
            List<RoleGroupInfo> list = dm.GetRoleGroupInfo(dm.GetRoleGroup(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  角色

    #region  编辑角色
    public string EditRole(string data)
    {
        try
        {
            RoleManage dm = new RoleManage();

            RoleInfo o = Public.Json.Deserialize<RoleInfo>(data);

            int result = 0;
            if (o.RoleId > 0)
            {
                result = dm.UpdateRole(o).Result;
            }
            else
            {
                o.OperatorId = uc.LoginUserId;

                result = dm.AddRole(o).Result;
                if (result > 0)
                {
                    o.RoleId = result;
                }
            }

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得角色
    public string GetRoleInfo(int roleId)
    {
        try
        {
            RoleInfo o = new RoleManage().GetRoleInfo(roleId);

            return o != null ? Public.ToJsonData(o) : Public.ToJsonMessage("没有找到相关的角色");
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得角色列表
    public string GetRoleList(string data)
    {
        try
        {
            RoleManage dm = new RoleManage();
            Dictionary<string, object> par = Public.Deserialize(data);

            int dataCount = 0;
            List<RoleInfo> list = dm.GetRoleInfo(dm.GetRole(par).DataSet, out dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  删除角色
    public string DeleteRole(int id)
    {
        try
        {
            RoleManage dm = new RoleManage();

            //int cc = dm.GetModuleChildCount(id);
            //if (cc > 0)
            //{
            //    return Public.ToJsonMessage(String.Format("对不起，该菜单包含{0}个子模块，不能删除。", cc));
            //}

            int result = dm.DeleteRole(id).Result;

            return Public.ToJsonResult(result > 0 ? 1 : 0);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  角色-模块-权限

    #region  获取角色-模块-权限配置数据
    public string GetRoleModulePermissionConfig(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            RoleModulePermissionManage dm = new RoleModulePermissionManage();

            if (Public.ConvertValue(par, "RoleId", 0) <= 0)
            {
                return Public.ToJsonMessage("未指定角色");
            }

            Dictionary<string, object> role = new Dictionary<string, object>();
            List<Dictionary<string, object>> module = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> config = new List<Dictionary<string, object>>();

            DataSet ds = dm.GetRoleModulePermissionConfig(par).DataSet;
            //角色信息
            if (Public.CheckTable(ds, 0))
            {
                role = Public.FillDataValue(ds.Tables[0].Rows[0]);
            }
            //模块
            if (Public.CheckTable(ds, 1))
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    module.Add(Public.FillDataValue(dr, true));
                }
            }
            
            //模块-权限
            if (Public.CheckTable(ds, 2))
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    list.Add(Public.FillDataValue(dr, true));
                }
            }

            //角色-模块-权限
            if (Public.CheckTable(ds, 3))
            {
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    config.Add(Public.FillDataValue(dr, true));
                }
            }
            
            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                {"role", role},{"module",module},{"list",list},{"config",config}
            };

            return Public.ToJsonData(d);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  编辑角色-模块-权限配置
    public string EditRoleModulePermission(string data)
    {
        try
        {
            RoleModulePermissionManage dm = new RoleModulePermissionManage();

            Dictionary<string, object> dic = Public.Json.Deserialize<Dictionary<string, object>>(data);

            int roleId = Public.ConvertValue(dic, "RoleId", 0);
            string mpids = Public.ConvertValue(dic, "PermissionIdList");

            string[] arr = mpids.Split(',');
            StringBuilder moduleIdList = new StringBuilder();
            StringBuilder permissionIdList = new StringBuilder();
            int n = 0;
            foreach (string str in arr)
            {
                if (!str.Equals(string.Empty))
                {
                    string[] tmp = str.Split('_');
                    if (n > 0)
                    {
                        moduleIdList.Append(",");
                        permissionIdList.Append(",");
                    }
                    moduleIdList.Append(tmp[0]);
                    permissionIdList.Append(tmp[1]);
                    n++;
                }
            }
            
            int operatorId = uc.LoginUserId;
            string createTime = Public.GetDateTime();

            if (roleId <= 0)
            {
                return Public.ToJsonMessage("未指定角色");
            }

            dm.DeleteRoleModulePermission(roleId, -1, -1);
            dm.BatchAddRoleModulePermission(roleId, moduleIdList.ToString(), permissionIdList.ToString(), operatorId, createTime);

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion


    #region  角色-导航菜单

    #region  获取角色-导航菜单
    public string GetRoleMenuConfig(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            RoleMenuManage dm = new RoleMenuManage();

            if (Public.ConvertValue(par, "RoleId", 0) <= 0)
            {
                return Public.ToJsonMessage("未指定角色");
            }

            Dictionary<string, object> role = new Dictionary<string, object>();
            List<Dictionary<string, object>> type = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> config = new List<Dictionary<string, object>>();

            DataSet ds = dm.GetRoleMenuConfig(par).DataSet;
            //角色信息
            if (Public.CheckTable(ds, 0))
            {
                role = Public.FillDataValue(ds.Tables[0].Rows[0]);
            }
            //菜单分类
            if (Public.CheckTable(ds, 1))
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    type.Add(Public.FillDataValue(dr, true));
                }
            }
            //菜单列表
            if (Public.CheckTable(ds, 2))
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    list.Add(Public.FillDataValue(dr, true));
                }
            }

            //角色-菜单
            if (Public.CheckTable(ds, 3))
            {
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    config.Add(Public.FillDataValue(dr, true));
                }
            }

            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                {"role", role},{"type", type},{"list",list},{"config",config}
            };

            return Public.ToJsonData(d);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  编辑角色-导航菜单
    public string EditRoleMenu(string data)
    {
        try
        {
            RoleMenuManage dm = new RoleMenuManage();

            Dictionary<string, object> dic = Public.Json.Deserialize<Dictionary<string, object>>(data);

            int roleId = Public.ConvertValue(dic, "RoleId", 0);
            string menuIdList = Public.ConvertValue(dic, "MenuIdList");
            int operatorId = uc.LoginUserId;
            string createTime = Public.GetDateTime();
            
            if (roleId <= 0)
            {
                return Public.ToJsonMessage("未指定角色");
            }

            dm.DeleteRoleMenu(roleId, -1);
            dm.BatchAddRoleMenu(roleId, menuIdList, operatorId, createTime);

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

    #region  角色-模块菜单

    #region  获取角色-模块菜单
    public string GetRoleModuleMenuConfig(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            RoleModuleMenuManage dm = new RoleModuleMenuManage();

            if (Public.ConvertValue(par, "RoleId", 0) <= 0)
            {
                return Public.ToJsonMessage("未指定角色");
            }

            Dictionary<string, object> role = new Dictionary<string, object>();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> config = new List<Dictionary<string, object>>();

            DataSet ds = dm.GetRoleModuleMenuConfig(par).DataSet;
            //角色信息
            if (Public.CheckTable(ds, 0))
            {
                role = Public.FillDataValue(ds.Tables[0].Rows[0]);
            }
            //菜单列表
            if (Public.CheckTable(ds, 1))
            {
                list = dm.GetRoleModuleMenuInfo(ds.Tables[1], true);

                foreach (Dictionary<string,object> dic in list)
                {
                    dic["MenuUrl"] = dic["MenuUrl"].ToString().Length > 0 ? "1" : "";
                }
            }

            //角色-菜单
            if (Public.CheckTable(ds, 2))
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    config.Add(Public.FillDataValue(dr, true));
                }
            }

            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                {"role", role},{"list",list},{"config",config}
            };

            return Public.ToJsonData(d);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  编辑角色-导航菜单
    public string EditRoleModuleMenu(string data)
    {
        try
        {
            RoleModuleMenuManage dm = new RoleModuleMenuManage();

            Dictionary<string, object> dic = Public.Json.Deserialize<Dictionary<string, object>>(data);

            int roleId = Public.ConvertValue(dic, "RoleId", 0);
            string menuIdList = Public.ConvertValue(dic, "MenuIdList");
            int operatorId = uc.LoginUserId;
            string createTime = Public.GetDateTime();

            if (roleId <= 0)
            {
                return Public.ToJsonMessage("未指定角色");
            }

            dm.DeleteRoleModuleMenu(roleId, -1);
            dm.BatchAddRoleModuleMenu(roleId, menuIdList, operatorId, createTime);

            return Public.ToJsonResult();
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #endregion

}