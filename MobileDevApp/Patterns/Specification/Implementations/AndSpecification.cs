using Patterns.Specification.Interfaces;
using System;

namespace Patterns.Specification.Implementations
{
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override bool IsSatisfiedBy(T obj)
        {
            return _left.IsSatisfiedBy(obj) && _right.IsSatisfiedBy(obj);
        }
    }
}
