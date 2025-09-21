using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.TransferLogs.Queries;

public class GetTransferLog
{
    public class Query : IRequest<TransferLog>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Query, TransferLog>
    {
        public async Task<TransferLog> Handle(Query request, CancellationToken cancellationToken)
        {
            var transferLog = await context.TransferLog
                .Include(t => t.Printer)
                .Include(t => t.FromLocation)
                .Include(t => t.ToLocation)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (transferLog == null) throw new Exception("TransferLog not found");

            return transferLog;
        }
    }
}