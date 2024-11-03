using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagmentApp.Infrastructure.Data;

public class ApplicationDbContextFactory
: IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("DefaultConnection");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
