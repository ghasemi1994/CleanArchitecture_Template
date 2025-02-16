using System.Linq.Expressions;
using System.Reflection;

namespace SystemBase.Framework;

public static class QueryableExtensions
{
   
    public static IQueryable<T> SortByClause<T>(this IQueryable<T> enumerable, string sortBy)
    {
        return enumerable.SortBy(sortBy);
    }
    public static IQueryable<T> SortBy<T>(this IQueryable<T> collection, string sortBy)
    {
        foreach (SortByInfo sortByInfo in ParseOrderBy(sortBy))
            collection = ApplyOrderBy<T>(collection, sortByInfo);

        return collection;
    }
    public static IQueryable<T> Paginated<T>(this IQueryable<T> entity, int pageNumber, int pageSize)
    {
        return entity.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    #region Utilities
    private static IEnumerable<SortByInfo> ParseOrderBy(string sortBy)
    {
        if (String.IsNullOrEmpty(sortBy))
            yield break;

        string[] items = sortBy.Split(',');
        bool initial = true;
        foreach (string item in items)
        {
            string[] pair = item.Trim().Split('-');

            if (pair.Length > 2)
                throw new ArgumentException(String.Format("Invalid OrderBy string '{0}'. Order By Format:Property,  Property2 - DESC, Property2 - ASC", item));

            string prop = pair[0].Trim();

            if (String.IsNullOrEmpty(prop))
                throw new ArgumentException("Invalid Property.Order By Format: Property, Property2 - DESC, Property2 - ASC");


            SortDirection dir = SortDirection.Ascending;

            if (pair.Length == 2)
                dir = ("desc".Equals(pair[1].Trim(), StringComparison.OrdinalIgnoreCase) ? SortDirection.Descending : SortDirection.Ascending);

            yield return new SortByInfo()
            {
                PropertyName = prop,
                Direction = dir,
                Initial = initial
            };

            initial = false;
        }
    }
    private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> collection, SortByInfo sortByInfo)
    {
        string[] props = sortByInfo.PropertyName.Split('.');
        Type type = typeof(T);

        ParameterExpression arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach (string prop in props)
        {
            PropertyInfo pi = type.GetProperty(prop);
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }
        Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
        string methodName = String.Empty;

        if (!sortByInfo.Initial && collection is IOrderedQueryable<T>)
        {
            if (sortByInfo.Direction == SortDirection.Ascending)
                methodName = "ThenBy";
            else
                methodName = "ThenByDescending";
        }
        else
        {
            if (sortByInfo.Direction == SortDirection.Ascending)
                methodName = "OrderBy";
            else
                methodName = "OrderByDescending";
        }

        return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single(
            method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, new object[] { collection, lambda });
    }
    private class SortByInfo
    {
        public SortDirection Direction { get; set; }
        public string PropertyName { get; set; }
        public bool Initial { get; set; }
    }
    private enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }
    #endregion
}
