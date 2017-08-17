using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  模块菜单信息

    /// <summary>
    /// 模块菜单信息
    /// </summary>
    public class ModuleMenuInfo
    {

        #region  字段属性

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// 菜单网址
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuPic { get; set; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string MenuDesc { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        /// 打开方式（是否默认打开）
        /// </summary>
        public int OpenType { get; set; }

        /// <summary>
        /// 菜单层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 父级目录树
        /// </summary>
        public string ParentTree { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 排序次序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// CRC校验码
        /// </summary>
        public string CrcCode { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public ModuleMenuInfo()
        {
            this.MenuId = 0;
            this.MenuName = string.Empty;
            this.MenuCode = string.Empty;
            this.MenuUrl = string.Empty;
            this.MenuPic = string.Empty;
            this.MenuDesc = string.Empty;
            this.MenuType = 0;
            this.OpenType = 0;
            this.Level = 0;
            this.ParentId = 0;

            this.ParentTree = string.Empty;
            this.Enabled = 0;
            this.SortOrder = 0;
            this.OperatorId = 0;
            this.CreateTime = string.Empty;
            this.UpdateTime = string.Empty;
            this.CrcCode = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion


}