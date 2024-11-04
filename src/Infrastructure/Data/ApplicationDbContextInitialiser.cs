using System.Runtime.InteropServices;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ProjectManagmentApp.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TimeProvider _time;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
                                           ApplicationDbContext context,
                                           UserManager<ApplicationUser> userManager,
                                           RoleManager<IdentityRole> roleManager,
                                           TimeProvider time)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _time = time;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
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
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        var domainName = "projectapp.com";

        // Default roles
        var managerRole = new IdentityRole(Roles.Manager);
        if (_roleManager.Roles.All(r => r.Name != managerRole.Name))
        {
            await _roleManager.CreateAsync(managerRole);
        }

        var employeeRole = new IdentityRole(Roles.Employee);
        if (_roleManager.Roles.All(r => r.Name != employeeRole.Name))
        {
            await _roleManager.CreateAsync(employeeRole);
        }

        // Default users
        var manager1 = new ApplicationUser
        {
            UserName = $"manager1",
            Email = $"manager1@{domainName}",
            EmailConfirmed = true,
        };
        if (_userManager.Users.All(u => u.UserName != manager1.UserName))
        {
            await _userManager.CreateAsync(manager1, "Manager1!");
            if (!string.IsNullOrWhiteSpace(managerRole.Name))
            {
                await _userManager.AddToRolesAsync(manager1, [managerRole.Name]);
            }
        }
        var manager2 = new ApplicationUser
        {
            UserName = $"manager2",
            Email = $"manager2@{domainName}",
            EmailConfirmed = true,
        };
        if (_userManager.Users.All(u => u.UserName != manager2.UserName))
        {
            await _userManager.CreateAsync(manager2, "Manager2!");
            if (!string.IsNullOrWhiteSpace(managerRole.Name))
            {
                await _userManager.AddToRolesAsync(manager2, [managerRole.Name]);
            }
        }

        var employee = new ApplicationUser
        {
            UserName = $"employee1",
            Email = $"employee1@{domainName}",
            EmailConfirmed = true,
        };
        if (_userManager.Users.All(u => u.UserName != employee.UserName))
        {
            await _userManager.CreateAsync(employee, "Employee1!");
            if (!string.IsNullOrWhiteSpace(employeeRole.Name))
            {
                await _userManager.AddToRolesAsync(employee, [employeeRole.Name]);
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.Projects.Any())
        {
            _context.Projects.Add(new Project
            {
                Name = "Project 1",
                Description = "Project 1 Description",
                StartDate = _time.GetUtcNow().DateTime,
                EndDate = _time.GetUtcNow().DateTime.AddDays(30),
                Status = Domain.Enums.ProjectStatus.Active,
                Budget = 10000,
                OwnedBy = manager1.UserName,
                Tasks =
                {
                    new ProjectTask {
                        Name = "Make a todo list 1 📃",
                        Status = Domain.Enums.ProjectTaskStatus.NotStarted,
                    },
                    new ProjectTask {
                        Name = "Check off the first item 1 ✅",
                        AssignedTo = employee.UserName,
                        Status = Domain.Enums.ProjectTaskStatus.InProgress,
                        StartDate = _time.GetUtcNow().DateTime,
                        EndDate = _time.GetUtcNow().DateTime.AddDays(20),
                        Description = "Check off the first item 1 ✅",
                    },
                    new ProjectTask {
                        Name = "Realise you've already done two things on the list! 1 🤯",
                        Status = Domain.Enums.ProjectTaskStatus.Completed,
                        StartDate = _time.GetUtcNow().DateTime,
                        EndDate = _time.GetUtcNow().DateTime.AddDays(10),
                        AssignedTo = manager2.UserName,
                        Description = "Realise you've already done two things on the list! 1 🤯",
                    },
                    new ProjectTask { Name = "Reward yourself with a nice, long nap 1 🏆" },
                }
            });

            _context.Projects.Add(new Project
            {
                Name = "Project 2",
                Description = "Project 2 Description",
                Status = Domain.Enums.ProjectStatus.NotStarted,
                Budget = 20000,
                OwnedBy = manager2.UserName,
                Tasks =
                {
                    new ProjectTask { Name = "Realise you've already done two things on the list! 2 🤯" },
                    new ProjectTask { Name = "Reward yourself with a nice, long nap 2 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
