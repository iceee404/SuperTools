using MediatR;
using Domain;
using Persistence;

namespace Application.TransferLogs.Commands;

public class CreateTransferLog
{
    public class Command : IRequest<string>
    {
        public required TransferLog TransferLog { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            context.TransferLog.Add(request.TransferLog);
            await context.SaveChangesAsync(cancellationToken);
            return request.TransferLog.Id;
        }
    }
}