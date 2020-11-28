using System;
using System.ComponentModel.DataAnnotations;

namespace LabsWebApp3.Areas.Chat.Models
{
    public class ChatModel
    {
        [Display(Name = "Логин")]
        public string UserName { get; set; }
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool IsModerator { get; set; }

        public DateTime BlockUpTo { get; set; }

        public bool IsBlocked => DateTime.Now >= BlockUpTo;
    }
}
