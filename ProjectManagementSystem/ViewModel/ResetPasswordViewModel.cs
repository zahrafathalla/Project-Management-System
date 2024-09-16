namespace ProjectManagementSystem.ViewModel;

public record ResetPasswordViewModel(string Email, string ResetCode, string NewPassword);