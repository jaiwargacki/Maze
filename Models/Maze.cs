namespace Maze.Models;

/// <summary>
/// The maze model.
/// </summary>
public class Maze
{
    // The width of the maze
    private readonly int _width;
    public int Width => _width;
    // The height of the maze
    private readonly int _height;
    public int Height => _height;

    // The squares of the maze
    // Top left is (0, 0)
    private readonly Square[,] _squares;
    // The current start square of the maze (can be null)
    private Square? _start;
    public Square? Start => _start;
    // The current end square of the maze (can be null)
    private Square? _end;
    public Square? End => _end;

    /// <summary>
    /// Constructor for the maze model.
    /// </summary>
    /// <param name="width">The width of the maze.</param>
    /// <param name="height">The height of the maze.</param>
    public Maze(int width, int height)
    {
        _width = width;
        _height = height;
        _squares = new Square[width, height];
        _start = null;
        _end = null;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                _squares[x, y] = new Square(x, y, Square.SquareType.WALL);
            }
        }
    }

    /// <summary>
    /// Gets the square at the given coordinates.
    /// </summary>
    /// <param name="x">The x coordinate of the square.</param>
    /// <param name="y">The y coordinate of the square.</param>
    /// <returns>The square at the given coordinates.</returns>
    public Square GetSquare(int x, int y)
    {
        return _squares[x, y];
    }

    /// <summary>
    /// Sets the square at the given coordinates to the given type.
    /// </summary>
    /// <param name="x">The x coordinate of the square.</param>
    /// <param name="y">The y coordinate of the square.</param>
    /// <param name="type">The type of the square.</param>
    /// <returns>True if the square was set, false otherwise.</returns>
    public bool SetSquare(int x, int y, Square.SquareType type)
    {
        if (!CheckInBounds(x, y))
        {
            return false;
        }
        if (type == Square.SquareType.START)
        {
            if (_start != null)
            {
                _squares[_start.X, _start.Y].Type = Square.SquareType.EMPTY;
            }
            _start = _squares[x, y];
        }
        else if (type == Square.SquareType.END)
        {
            if (_end != null)
            {
                _squares[_end.X, _end.Y].Type = Square.SquareType.EMPTY;
            }
            _end = _squares[x, y];
        }
        _squares[x, y].Type = type;
        return true;
    }

    /// <summary>
    /// Clears any paths displayed on the maze.
    /// </summary>
    /// <returns>The squares that were updated.</returns>
    public HashSet<Square> ClearPath() {
        HashSet<Square> clear = new HashSet<Square>();
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                if (_squares[x, y].Type == Square.SquareType.PATH) {
                    clear.Add(_squares[x, y]);
                    _squares[x, y].Type = Square.SquareType.EMPTY;
                }
            }
        }
        return clear;
    }

    /// <summary>
    /// Checks if the given coordinates are in the bounds of the maze.
    /// </summary>
    /// <param name="x">The x coordinate to check.</param>
    /// <param name="y">The y coordinate to check.</param>
    /// <returns>True if in bounds, false otherwise.</returns>
    private bool CheckInBounds(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    /// <summary>
    /// Checks if the given coordinates are in the bounds of the maze and are not a wall.
    /// </summary>
    /// <param name="x">The x coordinate to check.</param>
    /// <param name="y">The y coordinate to check.</param>
    /// <returns>True if valid, false otherwise.</returns>
    private bool CheckInBoundsAndValid(int x, int y)
    {
        return CheckInBounds(x, y) && _squares[x, y].Type != Square.SquareType.WALL;
    }

    /// <summary>
    /// Gets the neighbors of a coordinate.
    /// </summary>
    /// <param name="x">The x coordinate of the square.</param>
    /// <param name="y">The y coordinate of the square.</param>
    /// <returns>The neighbors of the square.</returns>
    public HashSet<Square> GetNeighbours(int x, int y) {
        HashSet<Square> neighbours = new();
        if (CheckInBoundsAndValid(x, y)) {
            int[] horizontalChecks = {-1, 0, 1, 0};
            int[] verticalChecks = {0, -1, 0, 1};
            for (int i = 0; i < 4; i++) {
                int newX = x + horizontalChecks[i];
                int newY = y + verticalChecks[i];
                if (CheckInBoundsAndValid(newX, newY)) {
                    neighbours.Add(_squares[newX, newY]);
                }
            }
        }
        return neighbours;
    }
}
