using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using SetValues.Contracts.Models.Requests;
using SetValues.Contracts.Models.Responses;
using SetValues.Contracts.Models.Wrapper;
using SetValues.Server.Entities;
using SetValues.Server.Extensions;
using SetValues.Server.Repositories;
using SetValues.Server.Specifications;

namespace SetValues.Server.Handlers;

public class GetAllPagedCustomersQueryHandler : IRequestHandler<GetAllPagedCustomersQuery, PaginatedResult<GetAllPagedCustomersResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<int> _unitOfWork;
    
    public GetAllPagedCustomersQueryHandler(IMapper mapper, IUnitOfWork<int> unitOfWork)
    {
        _mapper = mapper;
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PaginatedResult<GetAllPagedCustomersResponse>> Handle(GetAllPagedCustomersQuery query, CancellationToken cancellationToken)
    {
        var filter = new CustomerFilterSpecification(query.SearchString);
        var customers = _unitOfWork.Repository<Customer>().Entities.Specify(filter);
        
        if (query.OrderBy is null || !query.OrderBy.Any())
            return await customers.OrderBy(c => c).Select(c => _mapper.Map<GetAllPagedCustomersResponse>(c)).ToPaginatedListAsync(query.PageNumber, query.PageSize);
        
        return await customers.OrderBy(string.Join(",", query.OrderBy)).Select(c => _mapper.Map<GetAllPagedCustomersResponse>(c)).ToPaginatedListAsync(query.PageNumber, query.PageSize);
    }
}