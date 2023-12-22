using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "در ایجاد پایگاه داده خطایی رخ داد.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "در بارگزاری اطلاعات خطایی رخ داد.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // نقش های اولیه
        var administratorRole = new IdentityRole("Admin");
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // کاربران اولیه
        //var administrator = new ApplicationUser { UserName = "admin@yahoo.com", Email = "admin@yahoo.com" };
        //if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        //{
        //    await _userManager.CreateAsync(administrator, "admin");
        //    await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        //}

        // اطلاعات اولیه
        if (!_context.Products.Any())
        {
            _context.Products.Add(new Product
            {
                Id = 1,
                IsAvailable = true,
                ManufactureEmail = "p1@yahoo.com",
                ManufacturePhone = "09161234567",
                Name = "محصول 1",
                ProduceDate = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
    }
}
