using System.Collections;
using System.Collections.Generic;

namespace SoftwareFullComponent.UserComponent.DTO
{
    public class UserLicensesRead
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public bool Blocked { get; set; }
        public IEnumerable<LicenseRead> Licenses { get; set; }
    }
}