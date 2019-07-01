//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using ToolGood.Bedrock;

//namespace System.Linq
//{
//    public static class ExpressionExtension
//    {
//        /// <summary>
//        ///     Used for paging.  
//        /// </summary>
//        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int pageIndex, int pageSize)
//        {
//            AssertUtil.IsNotNull(query, nameof(query) + "is null");

//            return query.Skip((pageIndex-1)* pageSize).Take(pageSize);
//        }

//        /// <summary>
//        ///     Filters a <see cref="IQueryable{T}" /> by given predicate if given condition is true.
//        /// </summary>
//        /// <param name="query">Queryable to apply filtering</param>
//        /// <param name="condition">A boolean value</param>
//        /// <param name="predicate">Predicate to filter the query</param>
//        /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
//        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
//        {
//            AssertUtil.IsNotNull(query, nameof(query) + "is null");
//            AssertUtil.IsNotNull(predicate, nameof(predicate) + "is null");

//            return condition
//                ? query.Where(predicate)
//                : query;
//        }

//        /// <summary>
//        ///     Filters a <see cref="IQueryable{T}" /> by given predicate if given condition is true.
//        /// </summary>
//        /// <param name="query">Queryable to apply filtering</param>
//        /// <param name="condition">A boolean value</param>
//        /// <param name="predicate">Predicate to filter the query</param>
//        /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
//        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
//        {
//            AssertUtil.IsNotNull(query, nameof(query) + "is null");
//            AssertUtil.IsNotNull(predicate, nameof(predicate) + "is null");

//            return condition
//                ? query.Where(predicate)
//                : query;
//        }

//        public static Expression<Func<T, bool>> True<T>()
//        {
//            return f => true;
//        }

//        public static Expression<Func<T, bool>> False<T>()
//        {
//            return f => false;
//        }


//        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
//        {
//            InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

//            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
//        }

//        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
//        {
//            InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

//            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
//        }


//        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering)
//        {
//            var type = typeof(T);
//            var property = type.GetProperty(ordering);
//            var parameter = Expression.Parameter(type, "p");
//            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
//            var orderByExp = Expression.Lambda(propertyAccess, parameter);
//            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
//            return source.Provider.CreateQuery<T>(resultExp);
//        }
//        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering)
//        {
//            var type = typeof(T);
//            var property = type.GetProperty(ordering);
//            var parameter = Expression.Parameter(type, "p");
//            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
//            var orderByExp = Expression.Lambda(propertyAccess, parameter);
//            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
//            return source.Provider.CreateQuery<T>(resultExp);
//        }

//        public static IQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string ordering)
//        {
//            var type = typeof(T);
//            var property = type.GetProperty(ordering);
//            var parameter = Expression.Parameter(type, "p");
//            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
//            var orderByExp = Expression.Lambda(propertyAccess, parameter);
//            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "ThenBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
//            return source.Provider.CreateQuery<T>(resultExp);
//        }
//        public static IQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string ordering)
//        {
//            var type = typeof(T);
//            var property = type.GetProperty(ordering);
//            var parameter = Expression.Parameter(type, "p");
//            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
//            var orderByExp = Expression.Lambda(propertyAccess, parameter);
//            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "ThenByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
//            return source.Provider.CreateQuery<T>(resultExp);
//        }
//    }
//}
