
namespace ShaolanTech.Extensions.Web.Auth
{

    public class LocalConfigAuthManager
    {
        /// <summary>
        /// 对请求进行用户名密码验证
        /// </summary>
        /// <param name="identity">用户登录ID（用户名，Email，手机号）</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static async Task<ResultInfo<LocalConfigUserIdentity>> CheckPasswordAuthAsync(string identity, string password)
        {
            ResultInfo<LocalConfigUserIdentity> result = new ResultInfo<LocalConfigUserIdentity>();
            await Task.Delay(0);
            try
            {

                var encryptedPassword = SecurityUtil.GetMD5String(password);
                var userID = LocalConfigManager.GetCollection<LocalConfigLoginNameUserIDMapping>(identity);
                if (userID == null)
                {
                    result.OperationDone = false;
                    result.Message = "Error Password";
                }
                else
                {
                    var keyStore = LocalConfigManager.GetCollection<LocalConfigKeyStore>(userID.UserID);
                    if (keyStore == null)
                    {
                        result.OperationDone = false;
                        result.Message = "Error Password";
                    }
                    else if (keyStore.Password != encryptedPassword)
                    {
                        result.OperationDone = false;
                        result.Message = "Error Password";
                    }
                    else
                    {
                        result.AdditionalData = LocalConfigManager.GetCollection<LocalConfigUserIdentity>(userID.UserID);
                    }
                }


            }
            catch (Exception ex)
            {

                result.OperationDone = false;
                result.Message = ex.Message + ex.StackTrace;
            }

            return result;
        }



        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="password">新密码</param>
        /// <returns></returns>
        public static async Task<ResultInfo> ChangePassword(string userid, string password)
        {
            ResultInfo result = new ResultInfo();
            await Task.Delay(0);
            LocalConfigManager.SetCollection<LocalConfigKeyStore>(userid, new LocalConfigKeyStore { UserID = userid, Password = SecurityUtil.GetMD5String(password) });
            return result;
        }
        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="identity">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static async Task<ResultInfo<LocalConfigKeyStore>> RegisterAsync(LocalConfigUserIdentityInfo identity, string password)
        {
            ResultInfo<LocalConfigKeyStore> result = new ResultInfo<LocalConfigKeyStore>();

            await Task.Delay(0);


            try
            {
                if (identity.LoginName.IsNotNullOrEmpty() && identity.LoginName.IsMatch("$(root|admin|administrator)"))
                {
                    result.OperationDone = false;
                    result.Message = "Invalid Login Name";
                    return result;
                }
                if (LocalConfigManager.CollectionHasValue<LocalConfigLoginNameUserIDMapping>(identity.LoginName))
                {
                    result.OperationDone = false;
                    result.Message = "Invalid Login Name";
                    return result;
                }
                var userID = Guid.NewGuid().ToString();
                var id = new LocalConfigUserIdentity
                {
                    UserID = userID,
                    Identity = identity
                };
                LocalConfigLoginNameUserIDMapping mapping = new LocalConfigLoginNameUserIDMapping()
                {
                    LoginName = identity.LoginName,
                    UserID = userID
                };
                var localKeyStore = new LocalConfigKeyStore()
                {
                    UserID = userID,
                    Password = SecurityUtil.GetMD5String(password)
                };
                result.AdditionalData = localKeyStore;
                LocalConfigManager.SetCollection<LocalConfigUserIdentity>(userID, id);
                LocalConfigManager.SetCollection<LocalConfigLoginNameUserIDMapping>(mapping.LoginName, mapping);
                LocalConfigManager.SetCollection<LocalConfigKeyStore>(userID, localKeyStore);

            }
            catch (Exception ex)
            {

                result.OperationDone = false;
                result.Message = ex.Message;
            }


            return result;

        }
    }
}
