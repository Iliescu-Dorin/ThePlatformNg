namespace FoodAnalysis.Domain.Entities;
public class Food
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string EffectsOnDreams { get; set; }
    public bool isRecomendedBeforeSleep { get; set; }
    public bool isProvenByScientificData { get; set; }
    public string ScientificData { get; set; } = string.Empty;
    public string ScientificDataLink { get; set; } = string.Empty;

}