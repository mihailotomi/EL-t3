namespace EL_t3.Application.Grid.Helpers;

internal enum GridItemConstraintType
{
    EUROLEAGUE_CLUB,
    NBA_CLUB,
    COUNTRY,
    TEAMMATE
}

internal static class GridConstraintTypeHelper
{
    private static GridItemConstraintType GetGridItemConstraintY()
    {
        var rnd = new Random();
        var rndValue = rnd.NextDouble();

        if (rndValue < 0.675)
        {
            return GridItemConstraintType.EUROLEAGUE_CLUB;
        }

        return GridItemConstraintType.NBA_CLUB;
    }

    private static GridItemConstraintType GetGridItemConstraintX()
    {
        var rnd = new Random();
        var rndValue = rnd.NextDouble();

        if (rndValue < 0.75)
        {
            return GridItemConstraintType.EUROLEAGUE_CLUB;
        }

        return GridItemConstraintType.COUNTRY;
    }

    internal static IEnumerable<GridItemConstraintType> GetGridItemConstraintsY() =>
        [GridItemConstraintType.EUROLEAGUE_CLUB, GridItemConstraintType.EUROLEAGUE_CLUB, GetGridItemConstraintY()];

    internal static IEnumerable<GridItemConstraintType> GetGridItemConstraintsX() =>
        [GridItemConstraintType.EUROLEAGUE_CLUB, GetGridItemConstraintX(), GetGridItemConstraintX()];

}

