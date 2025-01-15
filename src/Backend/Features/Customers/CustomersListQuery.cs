using MediatR;
namespace Backend.Features.Customers;

public class CustomersListQuery : IRequest<List<CustomerDto>>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}

public class CustomersListQueryHandler : IRequestHandler<CustomersListQuery, List<CustomerDto>>
{
    private readonly BackendContext _context;

    public CustomersListQueryHandler(BackendContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> Handle(CustomersListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Customers.Include(c => c.CustomerCategory).AsQueryable();

        if (!string.IsNullOrEmpty(request.Name))
            query = query.Where(c => c.Name.ToLower().Contains(request.Name.ToLower()));

        if (!string.IsNullOrEmpty(request.Email))
            query = query.Where(c => c.Email.Contains(request.Email));

        return await query.Select(c => new CustomerDto
{
    Id = c.Id,
    Name = c.Name,
    Address = c.Address,
    Email = c.Email,
    Phone = c.Phone,
    Iban = c.Iban,
    CategoryCode = c.CustomerCategory != null ? c.CustomerCategory.Code : string.Empty,
    CategoryDescription = c.CustomerCategory != null ? c.CustomerCategory.Description : string.Empty
}).ToListAsync(cancellationToken);
    }
}

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Iban { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
}