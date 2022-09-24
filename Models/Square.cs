namespace Maze.Models;

public class Square
{
    private readonly int _x;
    private readonly int _y;
    private SquareType _type;
    public SquareType Type => _type;
    
    public Square(int x, int y, SquareType type)
    {
        _x = x;
        _y = y;
        _type = type;
    }

    public enum SquareType
    {
        WALL,
        EMPTY,
        START,
        END
    }
}
