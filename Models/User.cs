using ChatNova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    public class IUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public IUser()
        {
            IsActive = true;
            CreatedAt = DateTime.Now;
        }

        public IUser(string username, string passwordHash, string email, string phoneNumber)
        {
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            PhoneNumber = phoneNumber;
            IsActive = true;
            CreatedAt = DateTime.Now;
        }

        public bool IsValid()
        {
            bool hasCredentials = !string.IsNullOrWhiteSpace(Username)
                               && !string.IsNullOrWhiteSpace(PasswordHash);

            bool hasContact = !string.IsNullOrWhiteSpace(Email)
                           || !string.IsNullOrWhiteSpace(PhoneNumber);

            return hasCredentials && hasContact;
        }

        public bool HasEmail() => !string.IsNullOrWhiteSpace(Email);
        public bool HasPhone() => !string.IsNullOrWhiteSpace(PhoneNumber);

        public override string ToString()
        {
            return $"IUser[{UserId}] @{Username} | " +
                   $"Email: {Email ?? "N/A"} | " +
                   $"Phone: {PhoneNumber ?? "N/A"} | " +
                   $"Active: {IsActive}";
        }
    }
}