using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Automation.API.Models
{
    public class LinqExpression<T>
    {
        public Func<T, bool> GetDynamicQueryWithExpresionTrees(string propertyName, string val, string op)
        {
            //x =>
            var param = Expression.Parameter(typeof(T), "x");

            #region Convert to specific data type
            MemberExpression member = Expression.Property(param, propertyName);
            UnaryExpression valueExpression = GetValueExpression(propertyName, val, param);
            #endregion
            Expression body = Expression.Default(typeof(string));
            //val ("Curry")
            switch (op.ToLower())
            {
                case "equal":
                    body = Expression.Equal(member, valueExpression);
                    break;
                case "greater":
                    body = Expression.GreaterThan(member, valueExpression);
                    break;
                case "less":
                    body = Expression.LessThan(member, valueExpression);
                    break;
                case "greaterequal":
                    body = Expression.GreaterThanOrEqual(member, valueExpression);
                    break;
                case "lessequal":
                    body = Expression.LessThanOrEqual(member, valueExpression);
                    break;
            }
            //x => x.LastName == "Curry"
            var final = Expression.Lambda<Func<T, bool>>(body: body, parameters: param);
            return final.Compile();
        }

        private static UnaryExpression GetValueExpression(string propertyName, string val, ParameterExpression param)
        {
            var member = Expression.Property(param, propertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);

            if (!converter.CanConvertFrom(typeof(string)))
                throw new NotSupportedException();

            var propertyValue = converter.ConvertFromInvariantString(val);
            var constant = Expression.Constant(propertyValue);
            return Expression.Convert(constant, propertyType);
        }
    }
}