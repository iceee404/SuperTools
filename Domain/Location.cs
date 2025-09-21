using System.Text.Json.Serialization;

namespace Domain;

public class Location
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string LocationName { get; set; }
    // 位置类型：总部、门店、修理厂
    public required string LocationType { get; set; }
    // 如果是门店，门店编号
    public string? StoreNumber { get; set; }
    public string? Address { get; set; }
    
    // ignore json serialization to avoid circular reference
    [JsonIgnore]
    public ICollection<Printer> Printers { get; set; } = [];
}