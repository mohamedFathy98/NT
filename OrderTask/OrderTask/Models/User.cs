﻿namespace OrderTask.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ResetToken { get; set; } 
        public DateTime? ResetTokenExpiry { get; set; } // Already nullable
    }
}