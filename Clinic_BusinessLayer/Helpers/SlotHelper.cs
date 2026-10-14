using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Helpers
{
    public static class SlotHelper
    {
        /// <summary>
        /// مدة الـ Slot الواحد بالدقائق
        /// </summary>
        public const int SlotDurationMinutes = 15;

        /// <summary>
        /// بيتحقق إن الوقت على slot صحيح (15 دقيقة)
        /// مثال: 14:00 ✅ | 14:15 ✅ | 14:07 ❌
        /// </summary>
        public static bool IsValidSlot(DateTime dateTime)
        {
            return dateTime.Minute % SlotDurationMinutes == 0
                && dateTime.Second == 0
                && dateTime.Millisecond == 0;
        }

        /// <summary>
        /// بيحسب وقت النهاية = وقت البداية + 15 دقيقة
        /// </summary>
        public static DateTime CalculateEndTime(DateTime startTime)
        {
            return startTime.AddMinutes(SlotDurationMinutes);
        }

        /// <summary>
        /// بيرجّع أقرب slot صحيح للوقت اللي مبعوت
        /// مثال: 14:07 → 14:15
        /// </summary>
        public static DateTime GetNearestSlot(DateTime dateTime)
        {
            var minutesToAdd = SlotDurationMinutes - (dateTime.Minute % SlotDurationMinutes);
            return dateTime.AddMinutes(minutesToAdd).AddSeconds(-dateTime.Second).AddMilliseconds(-dateTime.Millisecond);
        }

        /// <summary>
        /// بيولّد كل الـ Slots المتاحة في يوم معين بناءً على start/end
        /// </summary>
        public static IEnumerable<DateTime> GenerateSlots(DateTime dayStart, DateTime dayEnd)
        {
            var current = dayStart;
            while (current < dayEnd)
            {
                yield return current;
                current = current.AddMinutes(SlotDurationMinutes);
            }
        }
    }
}
