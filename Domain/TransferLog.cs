using Domain;
using System.Text.Json.Serialization;
namespace Domain;

public class TransferLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string PrinterID { get; set; }
    public required string FromLocationID { get; set; }
    public required string ToLocationID { get; set; }
    public required DateTime TransferDate { get; set; }
    public required string TransferredBy { get; set; }
    // 转移原因：故障维修、门店调配、库存补充
    public required string TransferReason { get; set; }
    public string? Notes { get; set; }

    [JsonIgnore]
    public Printer Printer { get; set; }=null!;
    public Location FromLocation { get; set; }=null!;
    public Location ToLocation { get; set; }=null!;
}