﻿namespace ProjectManagementSystem.Contract;

public record AuthResponse(
    int Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn
);
