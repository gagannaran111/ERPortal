using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPortal.Core.Models;
namespace ERPortal.WebUI.Models
{
    public class Users_in_Role_ViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganisationId { get; set; }
        //public virtual Organisation Organisation{get;set;}
    }
}