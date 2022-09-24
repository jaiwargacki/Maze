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
        EMPTY = 0,
        WALL = 1,
        START = 2,
        END = 3,
        PATH = 4
    }
}
