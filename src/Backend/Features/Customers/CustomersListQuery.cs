using MediatR;
namespace Backend.Features.Customers;

<<<<<<< Updated upstream
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
=======
public class Customer
>>>>>>> Stashed changes
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Iban { get; set; } = "";

    public int? CustomerCategoryId { get; set; }
    public CustomerCategory? CustomerCategory { get; set; }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Address).HasMaxLength(200);
        builder.Property(s => s.Email).HasMaxLength(50);
        builder.Property(s => s.Phone).HasMaxLength(20);
        builder.Property(s => s.Iban).HasMaxLength(34);
        builder.HasOne(q => q.CustomerCategory).WithMany().HasForeignKey(q => q.CustomerCategoryId);
    }
}

class CustomerSeeding : SeedEntity<BackendContext, Customer>
{
    readonly List<int?> CustomerCategoryIdList;

    public CustomerSeeding(BackendContext context) : base(context)
    {
        CustomerCategoryIdList = context.CustomerCategories.Select(q => (int?)q.Id).ToList();
        CustomerCategoryIdList.Add(null);
    }

    protected override IEnumerable<Customer> GetSeedItems()
    {
        for (var i = 0; i < 500; i++)
        {
            var faker = new Faker("it");
            yield return new Customer
            {
                Name = faker.Company.CompanyName(),
                Address = faker.Address.FullAddress(),
                Email = faker.Internet.Email(),
                Phone = faker.Phone.PhoneNumber(),
                Iban = faker.Finance.Iban(),
                CustomerCategoryId = faker.PickRandom(CustomerCategoryIdList),
            };
        }
    }
}
