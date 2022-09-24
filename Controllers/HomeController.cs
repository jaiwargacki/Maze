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
    public IActionResult CreateMaze(int width, int height)
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
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
