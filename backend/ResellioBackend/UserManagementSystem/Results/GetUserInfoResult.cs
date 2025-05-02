using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.DTOs.Base;

namespace ResellioBackend.UserManagementSystem.Results
{
    public class GetUserInfoResult : ResultBase
    {
        public UserInfoDto userInfo {  get; set; }
    }
}
