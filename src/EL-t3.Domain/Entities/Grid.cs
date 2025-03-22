namespace EL_t3.Domain.Entities;

public class Grid: BaseEntity
{
    public IEnumerable<GridItem> X { get; set; } = null!;
    public IEnumerable<GridItem> Y { get; set; }= null!;

    private Grid(){}

    private Grid(IEnumerable<GridItem> x, IEnumerable<GridItem> y){
        if (x.Intersect(y).Any())
        {
            throw new ArgumentException("Grid items cannot be duplicated between X and Y!");
        }

        X = x;
        Y = y;
    }

    public static Grid Grid3X3(IEnumerable<GridItem> x, IEnumerable<GridItem> y){
        if(x.Count() != 3 || y.Count() != 3){
            throw new ArgumentException("Grid must be 3x3!");
        }

        return new Grid(x, y);
    }
}