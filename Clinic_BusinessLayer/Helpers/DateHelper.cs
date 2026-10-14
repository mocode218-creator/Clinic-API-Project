using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Helpers
{
    public static class DateHelper
    {
        /// <summary>
        /// بيرجّع بداية اليوم (00:00:00)
        /// </summary>
        public static DateTime GetDayStart(DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// بيرجّع نهاية اليوم (23:59:59.999)
        /// </summary>
        public static DateTime GetDayEnd(DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// بيحسب العمر بالسنين
        /// </summary>
        public static int CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// بيرجّع اسم اليوم من الرقم (0 = Sunday ... 6 = Saturday)
        /// </summary>
        public static string GetDayName(byte dayOfWeek)
        {
            return dayOfWeek switch
            {
                0 => "Sunday",
                1 => "Monday",
                2 => "Tuesday",
                3 => "Wednesday",
                4 => "Thursday",
                5 => "Friday",
                6 => "Saturday",
                _ => "Unknown"
            };
        }
    }
}
