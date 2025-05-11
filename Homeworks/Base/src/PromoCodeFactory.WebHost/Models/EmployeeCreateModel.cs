using System.Collections.Generic;
using System;

namespace PromoCodeFactory.WebHost.Models
{
    public class EmployeeCreateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public List<RoleItemResponse> Roles { get; set; }

    }
}
