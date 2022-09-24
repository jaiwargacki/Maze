namespace Maze.Models;

public class Maze
{
    private readonly int _width;
    public int Width => _width;
    private readonly int _height;
    public int Height => _height;

    // Bottom left is (0, 0)
    private readonly Square[,] _squares;

    public Maze(int width, int height)
    {
        _width = width;
        _height = height;
        _squares = new Square[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                _squares[x, y] = new Square(x, y, Square.SquareType.EMPTY);
            }
        }
    }

    public Square GetSquare(int width, int height)
    {
        return _squares[width, height];
    }
}
