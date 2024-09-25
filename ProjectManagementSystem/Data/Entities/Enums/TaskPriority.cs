using System.Runtime.Serialization;

namespace ProjectManagementSystem.Data.Entities.Enums;

public enum TaskPriority
{
    [EnumMember(Value = "High")]
    High,

    [EnumMember(Value = "Medium")]
    Medium,

    [EnumMember(Value = "Low")]
    Low
}
