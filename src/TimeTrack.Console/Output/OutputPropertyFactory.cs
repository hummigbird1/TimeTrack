using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Output.Attributes;
using TimeTrack.Console.OutputFormatting;

namespace TimeTrack.Console.Output
{
    public class OutputPropertyFactory : IOutputPropertyFactory
    {
        private readonly IDynamicExpressionResolver _dynamicExpressionResolver;

        public OutputPropertyFactory(IDynamicExpressionResolver dynamicExpressionResolver)
        {
            _dynamicExpressionResolver = dynamicExpressionResolver;
        }

        public IEnumerable<OutputProperty<T>> BuildOutputProperties<T>(IEnumerable<Expression<Func<T, string>>> properties)
        {
            foreach (var ex in properties)
            {
                yield return BuildOutputPropertyByExpression(ex);
            }
        }

        public OutputProperty<T> BuildOutputPropertyByExpression<T>(Expression<Func<T, string>> propertyExpression)
        {
            return BuildOutputPropertyByPropertyName(_dynamicExpressionResolver.GetPropertyNameFromExpression(propertyExpression.Body), propertyExpression.Compile());
        }

        public OutputProperty<T> BuildOutputPropertyByPropertyName<T>(string propertyName, Func<T, string> valueConversionFunction = null)
        {
            var property = typeof(T).GetProperty(propertyName);
            var headerAttribute = property.GetCustomAttribute<ListHeaderTextAttribute>();
            var alignmentAttribtue = property.GetCustomAttribute<ColumnAlignmentAttribute>();

            return new OutputProperty<T>
            {
                PropertyName = propertyName,
                HeaderText = headerAttribute == null ? propertyName : headerAttribute.HeaderText,
                RightAligned = alignmentAttribtue == null ? false : alignmentAttribtue.RightAlign,
                ValueConversionFunction = valueConversionFunction
            };
        }
    }
}