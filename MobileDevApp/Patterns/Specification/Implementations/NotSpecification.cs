using Patterns.Specification.Interfaces;
using System;

namespace Patterns.Specification.Implementations
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        public override bool IsSatisfiedBy(T obj)
        {
            return !_specification.IsSatisfiedBy(obj);
        }
    }
}
