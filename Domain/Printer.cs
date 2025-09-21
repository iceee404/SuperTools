using System.Diagnostics.Contracts;

namespace Domain;

public class Printer
{
    // uniqe id
    public string Id { get; set; } = Guid.NewGuid().ToString();
    // brand
    public required string Brand { get; set; }
    // model type
    public required string ModelType { get; set; }
    // toner model
    public required string TonerModel { get; set; }
    // serial number
    public required string SerialNumber { get; set; }

    // print specification a4 or a5 or both
    public required string Specification { get; set; }
    // location id (foreign key)
    public required string LocationID { get; set; }

    // 打印机状态：正常、故障、维修中、停用
    public required string Status { get; set; }

    // notes
    public string? Notes { get; set; }
    // warranty expiry date
    public DateTime? WarrantyExpiryDate { get; set; }
    // purchase date
    public DateTime? PurchaseDate { get; set; }


    // navigation property
    public Location Location { get; set; } = null!;
    public ICollection<TransferLog> TransferLogs { get; set; } = [];

}
