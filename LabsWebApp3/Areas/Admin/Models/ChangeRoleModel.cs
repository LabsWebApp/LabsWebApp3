using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LabsWebApp3.Areas.Admin.Models
{
    public class ChangeRoleModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
 //       public string Email { get; set; }
        public IEnumerable<IdentityRole> AllRoles { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public ChangeRoleModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
