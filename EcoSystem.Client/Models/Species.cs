namespace EcoSystem.Client.Models;

public class Species
{
    public int Id { get; set; }
    public string CommonName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public int EcosystemId { get; set; }
}