using Auis.StackOverflow.Models.ApiModels;

using System.Linq.Expressions;

namespace Auis.StackOverflow.BusinessLogic.Extensions;

public static class RequestExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, BasePagingRequest request) => query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, BaseSortablePagingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.SortBy))
            return query;

        var property = typeof(T).GetProperty(request.SortBy);
        if (property == null)
            return query;

        var parameter = Expression.Parameter(typeof(T));
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);
        var resultExp = Expression.Call(typeof(Queryable), request.IsDescending ? "OrderByDescending" : "OrderBy", new[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));

        return query.Provider.CreateQuery<T>(resultExp);
    }
}