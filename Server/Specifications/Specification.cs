using System.Linq.Expressions;
using SetValues.Server.Repositories;

namespace SetValues.Server.Specifications
{
    public abstract class Specification<T> : ISpecification<T> where T : class, IEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();
    }
}