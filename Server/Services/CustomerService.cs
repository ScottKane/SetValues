using MediatR;
using SetValues.Contracts.Models.Requests;
using SetValues.Contracts.Models.Responses;
using SetValues.Contracts.Models.Wrapper;
using SetValues.Contracts.Services;

namespace SetValues.Server.Services;

public class CustomerService : ICustomerService
{
    private readonly IMediator _mediator;
    
    public CustomerService(IMediator mediator) => _mediator = mediator;

    public async Task<Result<int>> AddEdit(AddEditCustomerCommand command) => await _mediator.Send(command);
    public async Task<PaginatedResult<GetAllPagedCustomersResponse>> GetAllPaged(GetAllPagedCustomersQuery query) => await _mediator.Send(query);
}