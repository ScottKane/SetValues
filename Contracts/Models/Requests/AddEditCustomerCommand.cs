using MediatR;
using ProtoBuf;
using SetValues.Contracts.Models.Wrapper;

namespace SetValues.Contracts.Models.Requests;

[ProtoContract]
public class AddEditCustomerCommand : IRequest<Result<int>>
{
    [ProtoMember(1)] public int Id { get; set; }
    [ProtoMember(2)] public int Title { get; set; }
    [ProtoMember(3)] public string FirstName { get; set; }
    [ProtoMember(4)] public string LastName { get; set; }
    [ProtoMember(5)] public string Number { get; set; }
    [ProtoMember(6)] public string Street { get; set; }
    [ProtoMember(7)] public string Email { get; set; }
}