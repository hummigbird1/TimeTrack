using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TimeTrack.Console.OutputFormatting
{
    public interface IOutputPropertyFactory
    {
        IEnumerable<OutputProperty<T>> BuildOutputProperties<T>(IEnumerable<Expression<Func<T, string>>> propertyExpressions);

        OutputProperty<T> BuildOutputPropertyByExpression<T>(Expression<Func<T, string>> propertyExpression);

        OutputProperty<T> BuildOutputPropertyByPropertyName<T>(string propertyName, Func<T, string> valueConversionFunction);
    }
}