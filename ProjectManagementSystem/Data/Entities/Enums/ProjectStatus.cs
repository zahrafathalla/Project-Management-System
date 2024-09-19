using System.Runtime.Serialization;

namespace ProjectManagementSystem.Data.Entities.Enums;

public enum ProjectStatus
{
    [EnumMember(Value = "Public")]
    Public,

    [EnumMember(Value = "Private")]
    Private
}
