using System;
using Domain;
using MediatR;
using Persistence;
using AutoMapper;
namespace Application.Printers.Commands;

public class EditPrinter
{
    public class Command : IRequest
    {
        public required Printer Printer { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var printer = await context.Printers.FindAsync([request.Printer.Id], cancellationToken);

            if (printer == null) throw new Exception("Printer not found");
            mapper.Map(request.Printer, printer);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
