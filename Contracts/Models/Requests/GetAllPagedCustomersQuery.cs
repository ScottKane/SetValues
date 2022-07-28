using MediatR;
using ProtoBuf;
using SetValues.Contracts.Models.Responses;
using SetValues.Contracts.Models.Wrapper;

namespace SetValues.Contracts.Models.Requests;

[ProtoContract]
public class GetAllPagedCustomersQuery : IRequest<PaginatedResult<GetAllPagedCustomersResponse>>
{
    [ProtoMember(1)]
    public int PageNumber { get; set; }
    [ProtoMember(2)]
    public int PageSize { get; set; }
    [ProtoMember(3)]
    public string SearchString { get; set; } = string.Empty;
    [ProtoMember(4)]
    public string[] OrderBy { get; set; }
}