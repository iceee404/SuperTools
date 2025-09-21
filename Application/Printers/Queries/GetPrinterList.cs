namespace Application.Printers.Queries;
using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;

public class GetPrinterList
{
    public class Query : IRequest<List<Printer>> { }
    

    public class Handler(AppDbContext context) : IRequestHandler<Query, List<Printer>>
    {
        public async Task<List<Printer>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Printers
                .Include(p => p.Location)
                .ToListAsync(cancellationToken);
        }
    }
}