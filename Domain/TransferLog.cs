using Domain;

namespace Domain;

public class TransferLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string PrinterID { get; set; }
    public required string FromLocationID { get; set; }
    public required string ToLocationID { get; set; }
    public required DateTime TransferDate { get; set; }
    public required string TransferredBy { get; set; }
    public string? Notes { get; set; }

    public Printer Printer { get; set; }=null!;
    public Location FromLocation { get; set; }=null!;
    public Location ToLocation { get; set; }=null!;
}