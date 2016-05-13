using System.Linq.Expressions;

namespace BitStrap
{
    /// <summary>
    /// Bunch of static reflection helper methods. These are generaly used to get some class member's name
    /// without the use of magic strings. I.e. obj.GetType().GetMember( "MemeberName" );
    /// </summary>
    public static class StaticReflectionHelper
    {
        /// <summary>
        /// Get a class member's name without the use of magic strings.
        /// Static member and method version.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetMemberName( Expression<System.Func<object>> expression )
        {
            return GetMemberName( expression.Body );
        }

        /// <summary>
        /// Get a class member's name without the use of magic strings.
        /// Instance member and method version.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetMemberName<T>( Expression<System.Func<T, object>> expression )
        {
            return GetMemberName( expression.Body );
        }

        /// <summary>
        /// Get a class member's name without the use of magic strings.
        /// Static method with no return version.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetMemberName( Expression<System.Action> expression )
        {
            return GetMemberName( expression.Body );
        }

        /// <summary>
        /// Get a class member's name without the use of magic strings.
        /// Instance method with no return version.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetMemberName<T>( Expression<System.Action<T>> expression )
        {
            return GetMemberName( expression.Body );
        }

        private static string GetMemberName( Expression expression )
        {
            MemberExpression memberExpression = expression as MemberExpression;
            if( memberExpression != null )
                return memberExpression.Member.Name;

            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if( methodCallExpression != null )
                return methodCallExpression.Method.Name;

            UnaryExpression unaryExpression = expression as UnaryExpression;
            if( unaryExpression != null )
                return GetMemberName( unaryExpression.Operand );

            return null;
        }
    }
}
