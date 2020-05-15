using System;

namespace Patterns.Specification.Implementations
{
    public class ExpressionSpecification<T> : CompositeSpecification<T>
    {
        private readonly Func<T, bool> _expression;

        public ExpressionSpecification(Func<T, bool> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override bool IsSatisfiedBy(T obj)
        {
            return _expression(obj);
        }
    }
}