namespace SetValues.Server.Repositories;

public interface IEntity { }
    
public interface IEntity<TId> : IEntity
{
    public TId Id { get; set; }
}