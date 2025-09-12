using System.Text.Json.Serialization;

namespace Domain;

public class Location
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string LocationName { get; set; }
    public required string LocationType { get; set; }
    public string? Address { get; set; }
    
    // ignore json serialization to avoid circular reference
    [JsonIgnore]
    public ICollection<Printer> Printers { get; set; } = [];
}