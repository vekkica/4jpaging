using _4JTools.Extensions.Linq;
using _4JTools.Extensions.String;
using System.Collections;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace _4JPaging.Filtering
{
    public static class FilterExtensions
    {
        public const string Letters = "abcdefghijklmnopqrstuvwxyz";
        public const string Numbers = "0123456789";
        public const string Special = "#$^+=!*()@%&";

        public static IQueryable<TModel> Filter<TModel>(this IQueryable<TModel> source,
            IDictionary<string, IEnumerable<string>> filters) where TModel : class
        {
            return source.FilterBase(filters, FilterConditions.Equal);
        }
        public static IQueryable<TModel> LessThan<TModel>(this IQueryable<TModel> source,
            IDictionary<string, IEnumerable<string>> filters, bool orEqual = true) where TModel : class
        {
            return source.FilterBase(filters, orEqual ? FilterConditions.LessThanOrEqual : FilterConditions.LessThan);
        }
        public static IQueryable<TModel> GreaterThan<TModel>(this IQueryable<TModel> source,
            IDictionary<string, IEnumerable<string>> filters, bool orEqual = true) where TModel : class
        {
            return source.FilterBase(filters, orEqual ? FilterConditions.GreaterThanOrEqual : FilterConditions.GreaterThan);
        }

        private static string FormatFilter(Type baseType, string propertyName, string condition, out Type valueType)
        {
            valueType = null;
            var filterTemplate = "";

            if (propertyName == null)
            {
                return null;
            }

            var parts = propertyName.Split('.');

            var propertyInfo = baseType.GetProperty(parts[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                return null;
            }

            var realName = propertyInfo.Name;
            valueType = propertyInfo.PropertyType;

            if (parts.Length == 1)
            {
                filterTemplate = $"{realName} {condition} {{0}}";
            }
            else
            {
                if (valueType.IsGenericType && (valueType.GetInterface(nameof(IEnumerable)) != null
                        || valueType.GetInterface(nameof(IQueryable)) != null))
                {
                    valueType = valueType.GetGenericArguments().FirstOrDefault();

                    filterTemplate += $"{realName}.Any([name] => [name].{{0}})";
                }
                else
                {
                    filterTemplate += $"{realName}.{{0}}";
                }

                filterTemplate = string.Format(filterTemplate, FormatFilter(valueType, parts.Skip(1).Aggregate((a, i) => a + "." + i), condition, out valueType));
            }

            return filterTemplate;
        }

        private static IQueryable<TModel> FilterBase<TModel>(this IQueryable<TModel> source, IDictionary<string, IEnumerable<string>> filters, string condition) where TModel : class
        {
            if (filters?.Any() != true)
            {
                return source;
            }

            // make keys case insensitive
            filters = new Dictionary<string, IEnumerable<string>>(filters, StringComparer.OrdinalIgnoreCase);

            var listOfConditions = new List<string>();

            filters.ForEach(filter =>
            {
                var filterTemplate = FormatFilter(typeof(TModel), filter.Key, condition, out var valueType);

                var values = filters[filter.Key].ToStringTypeValues(valueType, out var valid);

                if (!valid || !values.Any())
                {
                    listOfConditions.Add("false"); // return nothing because filter is invalid
                }
                else
                {
                    // replace name with random string so there is no conflict in Linq.Dynamic lib
                    values = values.Select(i => string.Format(filterTemplate, i).Replace("[name]", GenerateNew(5, 5, 0, 0)));

                    listOfConditions.Add(string.Join(" || ", values));
                }
            });

            return !listOfConditions.Any()
                ? source
                : source.Where(string.Join(" && ", listOfConditions.Select(i => $"({i})")));
        }

        private static string GenerateNew(int lowerCaseLetters = 5, int upperCaseLetters = 1, int numbers = 1, int special = 1)
        {
            var password = new List<char>();

            Enumerable.Range(0, lowerCaseLetters).ForEach(i => password.Add(Letters.ToLower().GetRandom()));
            Enumerable.Range(0, upperCaseLetters).ForEach(i => password.Add(Letters.ToUpper().GetRandom()));
            Enumerable.Range(0, numbers).ForEach(i => password.Add(Numbers.GetRandom()));
            Enumerable.Range(0, special).ForEach(i => password.Add(Special.GetRandom()));

            return password.Any() ? string.Join(null, password.OrderBy(i => Guid.NewGuid())) : null;
        }
    }
}
