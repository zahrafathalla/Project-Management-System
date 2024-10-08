﻿using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Entities;

public class UserProject :BaseEntity
{
    public bool IsCreator { get; set; } = false; 
    public int UserId { get; set; }
    public User User { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
}
