﻿namespace Banking.Services.DTOs
{
    public class PasswordResetDto
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
