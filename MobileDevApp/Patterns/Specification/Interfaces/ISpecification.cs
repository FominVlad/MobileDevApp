namespace Patterns.Specification.Interfaces
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T obj);
        ISpecification<T> And(ISpecification<T> specification);
        ISpecification<T> Or(ISpecification<T> specification);
        ISpecification<T> Not(ISpecification<T> specification);
    }
}
