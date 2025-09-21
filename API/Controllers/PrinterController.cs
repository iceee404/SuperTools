using System;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Application.Printers.Queries;
using Application.Printers.Commands;
using Application.Core;
using MediatR;
namespace API.Controllers;


public class PrinterController : BaseApiController
{
    // GET /api/Printer
    [HttpGet]
    public async Task<ActionResult<List<Printer>>> GetPrinters(
        [FromQuery] string? brand = null,
        [FromQuery] string? location = null,
        [FromQuery] string? status = null,
        [FromQuery] string? locationType = null)
    {
        return await Mediator.Send(new GetPrinterList.Query());
    }

    // GET /api/Printer/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Printer>> GetPrinter(string id)
    {
        return await Mediator.Send(new GetPrinterDetails.Query { Id = id });


    }

    // POST /api/Printer
    [HttpPost]
    public async Task<ActionResult<string>> CreatePrinter([FromBody] Printer printer)
    {
        return await Mediator.Send(new CreatePrinter.Command { Printer = printer });
    }

    // PUT /api/Printer/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePrinter(string id, [FromBody] Printer printer)
    {
        printer.Id = id;
        await Mediator.Send(new EditPrinter.Command { Printer = printer });
        return Ok();
    }

    // DELETE /api/Printer/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePrinter(string id)
    {
        await Mediator.Send(new DeletePrinter.Command { Id = id });
        return Ok();
    }
}
