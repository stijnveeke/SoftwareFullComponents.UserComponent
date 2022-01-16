using System;

namespace SoftwareFullComponent.UserComponent.DTO
{
    public class LicenseRead
    {
        public int Id { get; set; }
        public Guid LicenseKey { get; set; }
        public int TimesActivated { get; set; }
        public int ActivateableAmount { get; set; }
    }
}