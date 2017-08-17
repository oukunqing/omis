using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  用户-人员信息

    /// <summary>
    /// 用户-人员信息
    /// </summary>
    public class UserPersonInfo
    {

        #region  字段属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public UserPersonInfo()
        {
            this.Id = 0;
            this.UserId = 0;
            this.PersonId = 0;
            this.CreateTime = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}