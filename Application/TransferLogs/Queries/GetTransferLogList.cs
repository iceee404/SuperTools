using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.TransferLogs.Queries;

public class GetTransferLogList
{
    public class Query : IRequest<List<TransferLog>> { }

    public class Handler(AppDbContext context) : IRequestHandler<Query, List<TransferLog>>
    {
        public async Task<List<TransferLog>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.TransferLog
                .Include(t => t.Printer)
                .Include(t => t.FromLocation)
                .Include(t => t.ToLocation)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }
    }
}