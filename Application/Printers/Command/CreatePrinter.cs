using MediatR;
using Domain;
using Persistence;

namespace Application.Printers.Commands;

public class CreatePrinter
{
    public class Command : IRequest<string>
    {
        public required Printer Printer { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            context.Printers.Add(request.Printer);
            await context.SaveChangesAsync(cancellationToken);
            return request.Printer.Id;
        }
    }
}