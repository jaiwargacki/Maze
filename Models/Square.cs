namespace Maze.Models;

public class Square
{
    private readonly int _x;
    public int X => _x;
    private readonly int _y;
    public int Y => _y;
    public SquareType Type {get; set; }
    
    public Square(int x, int y, SquareType type)
    {
        _x = x;
        _y = y;
        Type = type;
    }

    public override int GetHashCode()
    {
        return _x * 1000 + _y;
    }

    public override bool Equals(Object? o) {
        if (o == null || GetType() != o.GetType()) {
            return false;
        } else {
            Square s = (Square) o;
            return _x == s._x && _y == s._y;
        }
    }

    public override string ToString()
    {
        return $"{X}:{Y}";
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
