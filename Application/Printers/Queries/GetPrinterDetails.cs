using System;
using Domain;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Printers.Queries;

public class GetPrinterDetails
{
    public class Query : IRequest<Printer>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Query, Printer>
    {
        public async Task<Printer> Handle(Query request, CancellationToken cancellationToken)
        {
            var printer = await context.Printers
                .Include(p => p.Location)
                .Include(p => p.TransferLogs)
                    .ThenInclude(t => t.FromLocation)
                .Include(p => p.TransferLogs)
                    .ThenInclude(t => t.ToLocation)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (printer == null) throw new Exception("Printer not found");

            return printer;
        }
    }
}
