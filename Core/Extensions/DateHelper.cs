using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class DateHelper
    {
        public static DateTime ToUtcDate(this DateTime dt)
            => DateTime.SpecifyKind(dt.Date, DateTimeKind.Utc);
    }
}
