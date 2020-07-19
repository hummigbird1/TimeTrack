using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Output.Attributes;

namespace TimeTrack.Console
{
    public class DynamicPropertySorter : IDynamicPropertySorter
    {
        public IOrderedEnumerable<T> Sort<T>(IEnumerable<T> enumerable, Sorting sorting)
        {
            var orderBySelector = GetKeySelector<T>(sorting.SortByPropertyName);
            if (orderBySelector == null)
            {
                throw new ArgumentException($"The sort property '{sorting.SortByPropertyName}' is invalid!");
            }
            return OrderByOption(enumerable, orderBySelector, sorting.SortDescending);
        }

        private static Func<TSource, object> GetKeySelector<TSource>(string propertyName)
        {
            if (propertyName == null)
            {
                return null;
            }

            var propertyInfo = typeof(TSource).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return new Func<TSource, object>(e => propertyInfo.GetValue(e));
            }

            var propertyByHeaderText = typeof(TSource).GetProperties().SingleOrDefault(x => x.GetCustomAttribute<ListHeaderTextAttribute>() != null && string.Equals(x.GetCustomAttribute<ListHeaderTextAttribute>().HeaderText, propertyName, StringComparison.OrdinalIgnoreCase));
            if (propertyByHeaderText != null)
            {
                return new Func<TSource, object>(e => propertyByHeaderText.GetValue(e));
            }

            return null;
        }

        private static IOrderedEnumerable<TSource> OrderByOption<TSource, TKey>(IEnumerable<TSource> entries, Func<TSource, TKey> keySelector, bool descending)
        {
            if (descending)
            {
                return entries.OrderByDescending(keySelector);
            }

            return entries.OrderBy(keySelector);
        }
    }
}