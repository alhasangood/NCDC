using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Extentions
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static DayOfWeek DayOfWeekTurbo(this DateTime date)
        {
            return (DayOfWeek)(((date.Ticks >> 14) + 1) % 7);
        }
    }
}
