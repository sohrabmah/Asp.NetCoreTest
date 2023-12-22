using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.UpdateTodoList;

public record UpdateProductCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new Exception("موردی یافت نشد");
        }
        
        entity.Name = request.Name;
        entity.ManufacturePhone = request.ManufacturePhone;
        entity.ManufactureEmail = request.ManufactureEmail;
        entity.ProduceDate = request.ProduceDate;
        entity.IsAvailable = request.IsAvailable;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
