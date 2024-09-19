using System.Runtime.Serialization;

namespace ProjectManagementSystem.Data.Entities.Enums;

public enum WorkTaskStatus
{
    [EnumMember(Value = "ToDo")]
    ToDo,

    [EnumMember(Value = "InProgress")]
    InProgress,

    [EnumMember(Value = "Done")]
    Done
}
