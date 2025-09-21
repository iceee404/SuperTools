using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.TransferLogs.Commands;

public class DeleteTransferLog
{
    public class Command : IRequest<bool>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var transferLog = await context.TransferLog.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (transferLog == null) return false;

            context.TransferLog.Remove(transferLog);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}