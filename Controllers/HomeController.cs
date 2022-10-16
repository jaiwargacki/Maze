using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Maze.Models;

namespace Maze.Controllers;

/// <summary>
/// The home controller.
/// </summary>
public class HomeController : Controller
{
    // Logger for the HomeController
    private readonly ILogger<HomeController> _logger;
    // Dictionary of mazes (should be a service)
    private static readonly Dictionary<string, Models.Maze> _mazes = new();
    // Counter for the number of mazes created (used as an id)
    private static int _mazeId = 0;
    // Key used in the session to store the current maze
    private static readonly string _mazeIdKey = "mazeId";

    /// <summary>
    /// Constructor for the HomeController
    /// </summary>
    /// <param name="logger">Logger for the HomeController</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Index page for the application
    /// </summary>
    /// <returns>Index view</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Generates a new maze for the provided dimensions
    /// </summary>
    /// <param name="width">Width of the maze</param>
    /// <param name="height">Height of the maze</param>
    /// <returns>Index view</returns>
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

    /// <summary>
    /// Updates the user's maze for the given selection
    /// </summary>
    /// <param name="x">X coordinate of the selection</param>
    /// <param name="y">Y coordinate of the selection</param>
    /// <param name="squareTypeInt">Type of the selection</param>
    /// <returns>True if the maze was updated with the square to update, false otherwise</returns>
    [HttpPost]
    public Object SelectCell(int x, int y, int squareTypeInt)
    {
        string? mazeId = HttpContext.Session.GetString(_mazeIdKey);
        if (mazeId == null) {
            return new {success = false};
        }
        Square.SquareType squareType = (Square.SquareType) squareTypeInt;
        _mazes[mazeId].SetSquare(x, y, squareType);
        HashSet<Square> update = new HashSet<Square>();
        update.Add(_mazes[mazeId].GetSquare(x, y));
        return new {success = true, update = update};
    }

    /// <summary>
    /// Clears any paths displayed on the maze
    /// </summary>
    /// <returns>True if the maze was updated with the squares to update, false otherwise</returns>
    [HttpPost]
    public Object ClearPath() {
        string? mazeId = HttpContext.Session.GetString(_mazeIdKey);
        if (mazeId == null) {
            return new {success = false};
        }
        Models.Maze maze = _mazes[mazeId];
        HashSet<Square> clear = maze.ClearPath();
        return new {success = true, update = clear};
    }

    /// <summary>
    /// Finds a path from the start to the end of the maze for the provided solve type
    /// </summary>
    /// <param name="solveType">Type of solve to perform</param>
    /// <returns>True if the maze was updated with the squares to update, false otherwise</returns>
    [HttpPost]
    public Object Solve(Solver.SolverTypes solveType)
    {
        string? mazeId = HttpContext.Session.GetString(_mazeIdKey);
        if (mazeId == null) {
            return new {success = false, message = "No maze created"};
        }
        Models.Maze maze = _mazes[mazeId];
        if (maze.Start == null || maze.End == null) {
            return new {success = false, message = "No start or end selected"};
        }
        // First clear any existing path
        Stack<Square> update =  new Stack<Square>(maze.ClearPath().ToList());
        Solver solver = new Solver(maze);
        Stack<Square>? path = solver.Solve(solveType);
        if (path == null) {
            return new {success = false, message = "No path found"};
        } else {
            path.Reverse().ToList().ForEach(s => update.Push(s));
            return new {success = true, update = path};
        }
    }
    
    /// <summary>
    /// Error page for the application
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
