using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Helpers
{
    public static class PaginationHelper
    {
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 100;

        /// <summary>
        /// بيتحقق إن قيم الـ Pagination صح ويعدلها لو غلط
        /// </summary>
        public static (int Page, int PageSize) Normalize(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = DefaultPageSize;
            if (pageSize > MaxPageSize) pageSize = MaxPageSize;

            return (page, pageSize);
        }

        /// <summary>
        /// بيعمل Pagination على أي IQueryable
        /// </summary>
        public static IQueryable<T> Paginate<T>(IQueryable<T> query, int page, int pageSize)
        {
            var (normalizedPage, normalizedSize) = Normalize(page, pageSize);
            return query
                .Skip((normalizedPage - 1) * normalizedSize)
                .Take(normalizedSize);
        }
    }
}