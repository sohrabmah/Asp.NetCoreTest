using Application.Common.Interfaces;
using Application.Products.Commands.UpdateTodoList;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.TodoLists.Commands.UpdateTodoList;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(v => v.Name)
           .NotEmpty().WithMessage("عنوان نمیتواند خالی باشد.")
           .MaximumLength(200).WithMessage("تعداد حروف عنوان میبایست بین 2 تا 100 حرف باشد");

        RuleFor(v => v.ManufactureEmail)
            .MustAsync(BeUniqueProductEmail).WithMessage("ایمیل نمیتواند تکراری باشد");

        RuleFor(v => v.ProduceDate)
            .MustAsync(BeUniqueProductDate).WithMessage("تاریخ نمیتواند تکراری باشد");
    }
    public async Task<bool> BeUniqueProductEmail(string value, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AllAsync(l => l.ManufactureEmail != value, cancellationToken);
    }

    public async Task<bool> BeUniqueProductDate(DateTime value, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AllAsync(l => l.ProduceDate != value, cancellationToken);
    }
}
