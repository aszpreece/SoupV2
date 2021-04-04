using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SoupV2.util
{
    public static class GetterSetterPointers
    {
        // returns property getter
        public static Func<TObject, TProperty> GetPropGetter<TObject, TProperty>(string propertyName, Type componentType)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(TObject), "value");

            var cast = Expression.TypeAs(paramExpression, componentType);

            Expression propertyGetterExpression = Expression.Property(cast, propertyName);

            Func<TObject, TProperty> result =
                Expression.Lambda<Func<TObject, TProperty>>(propertyGetterExpression, paramExpression).Compile();

            return result;
        }

        // returns property setter:
        public static Action<TObject, TProperty> GetPropSetter<TObject, TProperty>(string propertyName, Type componentType)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(TObject));

            ParameterExpression paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);
            
            var cast = Expression.TypeAs(paramExpression, componentType);
            
            MemberExpression propertyGetterExpression = Expression.Property(cast, propertyName);

            Action<TObject, TProperty> result = Expression.Lambda<Action<TObject, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            ).Compile();

            return result;
        }
    }
}
