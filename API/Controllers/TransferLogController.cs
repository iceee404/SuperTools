using Domain;
using Microsoft.AspNetCore.Mvc;
using Application.TransferLogs.Queries;
using Application.TransferLogs.Commands;

namespace API.Controllers;

public class TransferLogController : BaseApiController
{
    // GET /api/TransferLog
    [HttpGet]
    public async Task<ActionResult<List<TransferLog>>> GetTransferLogs()
    {
        return await Mediator.Send(new GetTransferLogList.Query());
    }

    // GET /api/TransferLog/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TransferLog>> GetTransferLog(string id)
    {
        return await Mediator.Send(new GetTransferLog.Query { Id = id });
    }

    // POST /api/TransferLog
    [HttpPost]
    public async Task<ActionResult<string>> CreateTransferLog([FromBody] TransferLog transferLog)
    {
        return await Mediator.Send(new CreateTransferLog.Command { TransferLog = transferLog });
    }

    // PUT /api/TransferLog/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTransferLog(string id, [FromBody] TransferLog transferLog)
    {
        transferLog.Id = id;
        await Mediator.Send(new EditTransferLog.Command { TransferLog = transferLog });
        return Ok();
    }

    // DELETE /api/TransferLog/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTransferLog(string id)
    {
        await Mediator.Send(new DeleteTransferLog.Command { Id = id });
        return Ok();
    }

    // GET /api/TransferLog/printer/{printerId}
    [HttpGet("printer/{printerId}")]
    public async Task<ActionResult<List<TransferLog>>> GetTransferLogsByPrinter(string printerId)
    {
        var allTransferLogs = await Mediator.Send(new GetTransferLogList.Query());
        var printerTransferLogs = allTransferLogs.Where(t => t.PrinterID == printerId).ToList();
        return printerTransferLogs;
    }

    // GET /api/TransferLog/location/{locationId}
    [HttpGet("location/{locationId}")]
    public async Task<ActionResult<List<TransferLog>>> GetTransferLogsByLocation(string locationId)
    {
        var allTransferLogs = await Mediator.Send(new GetTransferLogList.Query());
        var locationTransferLogs = allTransferLogs.Where(t =>
            t.FromLocationID == locationId || t.ToLocationID == locationId).ToList();
        return locationTransferLogs;
    }
}