using Entities.Enums;

namespace Entities.RequestModels
{
    public class AddOrUpdateSettingRequest
    {
        public List<UserAppSettingRequest> Settings { get; set; }
    }

    public class UserAppSettingRequest
    {
        public UserAppSettingKeyEnum Key { get; set; }
        public string Value { get; set; }
    }
}
