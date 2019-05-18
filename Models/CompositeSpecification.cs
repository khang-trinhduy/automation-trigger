using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Specification
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfied(T candidate);

        public abstract Expression<Func<T, bool>> Expression { get; }

        Func<T, bool> _compiledFunc;

        public virtual bool IsSatistifed(T candidate)
        {
            _compiledFunc = _compiledFunc ?? this.Expression.Compile();
            return _compiledFunc(candidate);
        }
    }
}