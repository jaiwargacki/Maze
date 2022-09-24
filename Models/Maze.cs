namespace Maze.Models;

public class Maze
{
    private readonly int _width;
    public int Width => _width;
    private readonly int _height;
    public int Height => _height;

    // Top left is (0, 0)
    private readonly Square[,] _squares;
    private Square? _start;
    public Square? Start => _start;
    private Square? _end;
    public Square? End => _end;

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
                _squares[x, y] = new Square(x, y, Square.SquareType.EMPTY);
            }
        }
    }

    public Square GetSquare(int x, int y)
    {
        return _squares[x, y];
    }

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

    private bool CheckInBounds(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    private bool CheckInBoundsAndValid(int x, int y)
    {
        return CheckInBounds(x, y) && _squares[x, y].Type != Square.SquareType.WALL;
    }

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
