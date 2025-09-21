using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.TransferLogs.Commands;

public class EditTransferLog
{
    public class Command : IRequest<TransferLog>
    {
        public required TransferLog TransferLog { get; set; }
    }

    public class Handler(AppDbContext context,IMapper mapper) : IRequestHandler<Command, TransferLog>
    {
        public async Task<TransferLog> Handle(Command request, CancellationToken cancellationToken)
        {
            var transferLog = await context.TransferLog.FirstOrDefaultAsync(x => x.Id == request.TransferLog.Id, cancellationToken);

            if (transferLog == null) throw new Exception("TransferLog not found");

            // transferLog.PrinterID = request.TransferLog.PrinterID;
            // transferLog.FromLocationID = request.TransferLog.FromLocationID;
            // transferLog.ToLocationID = request.TransferLog.ToLocationID;
            // transferLog.TransferDate = request.TransferLog.TransferDate;
            // transferLog.TransferredBy = request.TransferLog.TransferredBy;
            // transferLog.TransferReason = request.TransferLog.TransferReason;
            // transferLog.TransferStatus = request.TransferLog.TransferStatus;
            // transferLog.Notes = request.TransferLog.Notes;

            mapper.Map(request.TransferLog, transferLog);

            await context.SaveChangesAsync(cancellationToken);
            return transferLog;
        }
    }
}