using System;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

public class PrinterController(AppDbContext context) : BaseApiController
{
    // GET /api/Printer
    [HttpGet]
    public async Task<ActionResult<List<Printer>>> GetPrinters(
        [FromQuery] string? brand = null,
        [FromQuery] string? location = null)
    {
        var query = context.Printers.AsQueryable();
        
        if (!string.IsNullOrEmpty(brand))
            query = query.Where(p => p.Brand.Contains(brand));
            
        if (!string.IsNullOrEmpty(location))
            query = query.Where(p => p.Location.LocationName.Contains(location));
            
        return await query.Include(p => p.Location).ToListAsync();
    }

    // GET /api/Printer/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Printer>> GetPrinter(string id)
    {
        var printer = await context.Printers
            .Include(p => p.Location)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (printer == null) return NotFound();

        return printer;
    }
    
    // GET /api/Printer/{id}/transfers
    [HttpGet("{id}/transfers")]
    public async Task<ActionResult<List<TransferLog>>> GetTransfers(string id)
    {
        var transfers = await context.TransferLog
            .Where(t => t.PrinterID == id)
            .Include(t => t.Printer)
            .Include(t => t.FromLocation)
            .Include(t => t.ToLocation)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
            
        return transfers;
    }
    
}
