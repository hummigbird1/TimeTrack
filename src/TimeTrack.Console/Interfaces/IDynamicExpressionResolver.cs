using System.Linq.Expressions;

namespace TimeTrack.Console.Interfaces
{
    public interface IDynamicExpressionResolver
    {
        string GetPropertyNameFromExpression(Expression expression);
    }
}