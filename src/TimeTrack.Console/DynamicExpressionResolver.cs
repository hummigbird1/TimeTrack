using System;
using System.Linq.Expressions;
using System.Reflection;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console
{
    public class DynamicExpressionResolver : IDynamicExpressionResolver
    {
        public string GetPropertyNameFromExpression(Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Member is PropertyInfo propertyInfo)
                {
                    return propertyInfo.Name;
                }

                throw new NotSupportedException($"MemberExpression with Member of type {memberExpression.Member.GetType()} is not supported!");
            }

            if (expression is ConditionalExpression conditionalExpression)
            {
                try
                {
                    return GetPropertyNameFromExpression(conditionalExpression.IfTrue);
                }
                catch (NotSupportedException)
                {
                    return GetPropertyNameFromExpression(conditionalExpression.IfFalse);
                }
            }

            if (expression is UnaryExpression unaryExpression)
            {
                return GetPropertyNameFromExpression(unaryExpression.Operand);
            }

            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Object != null)
                {
                    return GetPropertyNameFromExpression(methodCallExpression.Object);
                }

                foreach (var arg in methodCallExpression.Arguments)
                {
                    try
                    {
                        return GetPropertyNameFromExpression(arg);
                    }
                    catch (NotSupportedException)
                    {
                    }
                }

                throw new NotSupportedException("MethodCallexpression with no valid Arguments is not supported!");
            }

            throw new NotSupportedException($"Expression body type: {expression.GetType()} not supported!");
        }
    }
}