// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.Model;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{T}"/> that implements the sort and filter options.
    /// </summary>
    public static class QueryableSortAndFilterExtensions
    {
        /// <summary>
        /// Applies sort and filter options to a specific query.
        /// </summary>
        /// <typeparam name="T">The model type associated with the query.</typeparam>
        /// <param name="query">The reference query.</param>
        /// <param name="sortAndFilter">The sort and filter options, if any.</param>
        /// <param name="definition">The definition associated with the filtered elements.</param>
        /// <returns>The updated query.</returns>
        public static IQueryable<T> ApplySortAndFilter<T>(this IQueryable<T> query, SortAndFilterModel sortAndFilter, Definition definition)
            where T : BaseModel
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            else if (definition is null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            var result = query;
            if (sortAndFilter is null)
            {
                return result;
            }

            if (!string.IsNullOrWhiteSpace(sortAndFilter.Filter))
            {
                // TODO: Apply filter
            }

            if (!string.IsNullOrEmpty(sortAndFilter.SortKey))
            {
                // Apply sorting
                var field = definition.Fields
                    .SingleOrDefault(d => string.Equals(d.Name, sortAndFilter.SortKey, StringComparison.OrdinalIgnoreCase));
                if (field == null)
                {
                    // The named field is not part of the definition
                    // Ignore the option to avoid potential injection
                    return result;
                }

                var prop = typeof(T).GetProperties().FirstOrDefault(p => string.Equals(p.Name, field.Name, StringComparison.OrdinalIgnoreCase));
                if (prop == null)
                {
                    // The named field is not part of the entity class
                    // Ignore the option to avoid potential injection
                    return result;
                }

                if (!field.Sortable)
                {
                    var modelState = new ModelStateDictionary();
                    modelState.AddModelError("SortKey", $"The field {field.DisplayName} can't be used for sorting");
                    throw new InvalidModelStateException(modelState);
                }

                if (!prop.CanWrite)
                {
                    // The field is computed
                    // Ensure that the Entity Framework query is executed before sorting on this field
                    result = result.ToList().AsQueryable();
                }

                bool ascending = !string.Equals(sortAndFilter.SortValue, "desc", StringComparison.OrdinalIgnoreCase);
                ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
                MemberExpression property = Expression.Property(parameter, sortAndFilter.SortKey);

                switch (field.Type)
                {
                    case "reference":
                    case "ref/category":
                    case "ref/client":
                    case "ref/user":
                        var refSelector =
                            Expression.Lambda<Func<T, string>>(
                                Expression.Property(property, "DisplayName"),
                                parameter);
                        result = result.ApplySort(refSelector, ascending);
                        break;

                    case "ref/eventtimemapping":
                        var mappingSelector = Expression.Lambda<Func<T, EventTimeMapping>>(property, parameter);
                        result = result.ApplySort(mappingSelector, ascending);
                        break;

                    case "ref/status":
                        var statusSelector = Expression.Lambda<Func<T, Status>>(property, parameter);
                        result = result.ApplySort(statusSelector, ascending);
                        break;

                    case "boolean":
                        var boolSelector = Expression.Lambda<Func<T, bool>>(property, parameter);
                        result = result.ApplySort(boolSelector, ascending);
                        break;

                    case "int":
                    case "hours":
                        var intSelector = Expression.Lambda<Func<T, int>>(property, parameter);
                        result = result.ApplySort(intSelector, ascending);
                        break;

                    case "datetime":
                        var datetimeSelector = Expression.Lambda<Func<T, DateTime>>(property, parameter);
                        result = result.ApplySort(datetimeSelector, ascending);
                        break;

                    case "date":
                        if (field.Required)
                        {
                            var dateSelector = Expression.Lambda<Func<T, DateOnly>>(property, parameter);
                            result = result.ApplySort(dateSelector, ascending);
                            break;
                        }
                        else
                        {
                            var dateSelector = Expression.Lambda<Func<T, DateOnly?>>(property, parameter);
                            result = result.ApplySort(dateSelector, ascending);
                            break;
                        }

                    case "string":
                        var stringSelector = Expression.Lambda<Func<T, string>>(property, parameter);
                        result = result.ApplySort(stringSelector, ascending);
                        break;

                    default:
                        var defaultSelector = Expression.Lambda<Func<T, object>>(property, parameter);
                        result = result.ApplySort(defaultSelector, ascending);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Applies sort options to a specific LINQ query.
        /// </summary>
        /// <typeparam name="T">The type of the query elements.</typeparam>
        /// <typeparam name="TKey">The type of the sorting property.</typeparam>
        /// <param name="query">The updated query.</param>
        /// <param name="keySelector">The property selection expression.</param>
        /// <param name="ascending">A value indicating whether the order is ascending; or descending.</param>
        /// <returns>The updated LINQ query.</returns>
        public static IQueryable<T> ApplySort<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, bool ascending)
        {
            if (ascending)
            {
                return query.OrderBy<T, TKey>(keySelector);
            }
            else
            {
                return query.OrderByDescending<T, TKey>(keySelector);
            }
        }
    }
}
