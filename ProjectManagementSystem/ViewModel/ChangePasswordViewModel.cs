using MediatR;
using ProjectManagementSystem.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace ProjectManagementSystem.ViewModel;

public record ChangePasswordViewModel(
    string Email,
    string CurrentPassword,
    string NewPassword);
