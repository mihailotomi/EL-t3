namespace EL_t3.Core.Entities;

public class Grid
{
    public required IEnumerable<GridItem> X { get; set; }
    public required IEnumerable<GridItem> Y { get; set; }

}