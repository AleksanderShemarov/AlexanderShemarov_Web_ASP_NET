﻿using Microsoft.AspNetCore.Identity;

namespace AlexanderShemarov.UI.Data
{
    public class AppUser: IdentityUser
    {
        public byte[]? Avatar { get; set; }
        public string MimeType { get; set; } = string.Empty;
    }
}
