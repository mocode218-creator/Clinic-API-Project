using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.DoctorSchedules
{
    public class CreateDoctorScheduleRequest
    {
        public int DoctorId { get; set; }
        public byte DayOfWeek { get; set; }   // 0 = Sunday ... 6 = Saturday
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}