using System.ServiceModel;
using SetValues.Contracts.Models.Requests;
using SetValues.Contracts.Models.Responses;
using SetValues.Contracts.Models.Wrapper;

namespace SetValues.Contracts.Services;

[ServiceContract]
public interface ICustomerService
{
    [OperationContract]
    public Task<Result<int>> AddEdit(AddEditCustomerCommand command);
    
    [OperationContract]
    public Task<PaginatedResult<GetAllPagedCustomersResponse>> GetAllPaged(GetAllPagedCustomersQuery query);
}
