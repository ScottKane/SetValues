using SetValues.Server.Repositories;

namespace SetValues.Server.Entities;

public class Customer : AuditableEntity<int>
{
    public int Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Address Address { get; set; }
    public Contact Contact { get; set; }
}