namespace Maze.Models;

/// <summary>
/// Square model used in the maze.
/// </summary>
public class Square
{
    // The x coordinate of the square
    private readonly int _x;
    public int X => _x;
    // The y coordinate of the square
    private readonly int _y;
    public int Y => _y;
    // The current type of the square
    public SquareType Type {get; set; }
    
    /// <summary>
    /// Constructor for the square.
    /// </summary>
    /// <param name="x">The x coordinate of the square.</param>
    /// <param name="y">The y coordinate of the square.</param>
    /// <param name="type">The type of the square.</param>
    public Square(int x, int y, SquareType type)
    {
        _x = x;
        _y = y;
        Type = type;
    }

    /// <summary>
    /// Hash code for the square.
    /// </summary>
    /// <returns>The hash code for the square.</returns>
    public override int GetHashCode()
    {
        return _x * 1000 + _y;
    }

    /// <summary>
    /// Equality for the square. Only compares x and y (not type).
    /// </summary>
    /// <param name="obj">The object to compare to.</param>
    /// <returns>True if the objects are equal, false otherwise.</returns>
    public override bool Equals(Object? o) {
        if (o == null || GetType() != o.GetType()) {
            return false;
        } else {
            Square s = (Square) o;
            return _x == s._x && _y == s._y;
        }
    }

    /// <summary>
    /// String representation of the square. In the form '{x}:{y}'.
    /// </summary>
    /// <returns>The string representation of the square.</returns>
    public override string ToString()
    {
        return $"{X}:{Y}";
    }

    /// <summary>
    /// Types of squares.
    /// </summary>
    public enum SquareType
    {
        EMPTY = 0,
        WALL = 1,
        START = 2,
        END = 3,
        PATH = 4
    }
}
