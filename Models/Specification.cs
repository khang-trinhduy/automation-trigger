using System;
using System.Linq.Expressions;

namespace Specification
{
    public interface ISpecification
    {
    }

    public interface ISpecification<T> : ISpecification
    {
        bool IsSatisfied(T candidate);
        Expression<Func<T, bool>> Expression { get; }

    }

    public interface Specification<TEntity> : ISpecification
    {
        Expression<Func<TEntity, bool>> SatisfiedBy();
        Expression<Func<TEntity, bool>> Expression { get; }
    }
}