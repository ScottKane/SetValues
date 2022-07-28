using AutoMapper;
using MediatR;
using SetValues.Contracts.Models.Requests;
using SetValues.Contracts.Models.Wrapper;
using SetValues.Server.Entities;
using SetValues.Server.Repositories;

namespace SetValues.Server.Handlers;

public class AddEditCustomerCommandHandler : IRequestHandler<AddEditCustomerCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<int> _unitOfWork;
    
    public AddEditCustomerCommandHandler(IMapper mapper, IUnitOfWork<int> unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<int>> Handle(AddEditCustomerCommand command, CancellationToken cancellationToken)
    {
        if (command.Id is not 0)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(command.Id);
            if (customer is not null)
            {
                customer = _mapper.Map<Customer>(command);
                await _unitOfWork.Repository<Customer>().UpdateAsync(customer);
                await _unitOfWork.Commit(cancellationToken);
                
                return await Result<int>.SuccessAsync(customer.Id, "Customer Updated");
            }
            
            return await Result<int>.FailAsync("Customer Not Found!");
        }
        else
        {
            var customer = _mapper.Map<Customer>(command);
            await _unitOfWork.Repository<Customer>().AddAsync(customer);
            await _unitOfWork.Commit(cancellationToken);
            
            return await Result<int>.SuccessAsync(customer.Id, "Customer Saved");
        }
    }
}