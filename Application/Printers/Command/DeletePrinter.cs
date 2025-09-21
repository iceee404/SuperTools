using MediatR;
using Domain;
using Persistence;

namespace Application.Printers.Commands;

public class DeletePrinter
{
    public class Command : IRequest
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var printer = await context.Printers.FindAsync([request.Id], cancellationToken);
            if (printer == null) throw new Exception("Printer not found");
            context.Printers.Remove(printer);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}