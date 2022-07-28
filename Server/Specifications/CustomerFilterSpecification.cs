using SetValues.Server.Entities;

namespace SetValues.Server.Specifications;

public class CustomerFilterSpecification : Specification<Customer>
{
    public CustomerFilterSpecification(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
            Criteria = c =>
                c.Id.ToString() == searchString ||
                c.Title.ToString() == searchString ||
                c.FirstName.Contains(searchString) ||
                c.LastName.Contains(searchString);
        else
            Criteria = c => true;
    }
}