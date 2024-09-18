using System.Runtime.Serialization;

namespace ProjectManagementSystem.Data.Entities.Enums
{
    public enum UserStatus
    {
        [EnumMember(Value = "Active")]
        Active,

        [EnumMember(Value = "NotActive")]
        NotActive
    }
}
