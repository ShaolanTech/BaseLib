using System.Collections.Generic;

namespace ShaolanTech.Extensions.Web.Auth
{
    public class LocalConfigUserIdentityInfo
    {
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 用户显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public UserState State { get; set; } = UserState.Normal;
        /// <summary>
        /// ApiID
        /// </summary>
        public string ApiKey { get; set; }
    }
    public class LocalConfigUserIdentity
    { 
        public string UserID { get; set; }
       
        public LocalConfigUserIdentityInfo Identity { get; set; }
    }

    public class LocalConfigKeyStore
    {
        public string UserID { get; set; }
        public string Password { get; set; }
    }
    public class LocalConfigLoginNameUserIDMapping
    {
        public string UserID { get; set; }
        public string LoginName { get; set; }
    }
}
