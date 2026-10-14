using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// بيشفّر الباسورد باستخدام BCrypt
        /// </summary>
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// بيتحقق إن الباسورد يطابق الـ Hash
        /// </summary>
        public static bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
