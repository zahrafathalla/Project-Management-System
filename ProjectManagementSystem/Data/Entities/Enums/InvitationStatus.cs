using System.Runtime.Serialization;

namespace ProjectManagementSystem.Data.Entities.Enums
{
    public enum InvitationStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Accepted")]
        Accepted,

        [EnumMember(Value = "Rejected")]
        Rejected
    }
}
