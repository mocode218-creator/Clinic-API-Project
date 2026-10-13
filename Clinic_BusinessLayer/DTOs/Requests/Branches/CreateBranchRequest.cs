using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.Branches
{
    public class CreateBranchRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? WorkingHours { get; set; }
    }
}