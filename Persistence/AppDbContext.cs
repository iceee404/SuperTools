using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Printer> Printers { get; set; }
    public required DbSet<Location> Location { get; set; }
    public required DbSet<TransferLog> TransferLog { get; set; }
}
