using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Maze.Models;

namespace Maze.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static readonly Dictionary<string, Models.Maze> _mazes = new();
    private static int _mazeId = 0;
    private static readonly string _mazeIdKey = "mazeId";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ResizeMaze(int width, int height)
    {
        string? mazeId = HttpContext.Session.GetString(_mazeIdKey);
        if (mazeId == null)
        {
            mazeId = _mazeId.ToString();
            _mazeId++;
            HttpContext.Session.SetString(_mazeIdKey, mazeId);
        } else {
            _mazes.Remove(mazeId);
        }
        _mazes.Add(mazeId, new Models.Maze(width, height));
        return View(_mazes[mazeId]);
    }

    [HttpPost]
    public bool SelectCell(int x, int y, int squareTypeInt)
    {
        string? mazeId = HttpContext.Session.GetString(_mazeIdKey);
        if (mazeId == null) {
            return false;
        }
        Square.SquareType squareType = (Square.SquareType) squareTypeInt;
        _mazes[mazeId].SetSquare(x, y, squareType);
        return true;
    }

    [HttpPost]
    public Object ClearPath() {
        string? mazeId = HttpContext.Session.GetString(_mazeIdKey);
        if (mazeId == null) {
            return new {success = false};
        }
        Models.Maze maze = _mazes[mazeId];
        HashSet<Square> clear = maze.ClearPath();
        return new {success = true, clear = clear};
    }

    [HttpPost]
    public Object Solve(string solveType)
    {
        string? mazeId = HttpContext.Session.GetString(_mazeIdKey);
        if (mazeId == null) {
            return new {success = false, message = "No maze created"};
        }
        Models.Maze maze = _mazes[mazeId];
        if (maze.Start == null || maze.End == null) {
            return new {success = false, message = "No start or end selected"};
        }
        Solver solver = new Solver(maze);
        switch (solveType) {
            case "BFS":
                Stack<Square>? path = solver.BFS();
                if (path == null) {
                    return new {success = false, message = "No path found"};
                } else {
                    return new {success = true, path = path};
                }
            default:
                return new {success = false, message = "Invalid solve type"};
        }

    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
