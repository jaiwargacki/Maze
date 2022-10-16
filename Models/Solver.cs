namespace Maze.Models;

/// <summary>
/// The solver for the maze.
/// </summary>
public class Solver
{
    // The maze to solve
    private readonly Maze _maze;

    /// <summary>
    /// Constructor for the solver.
    /// </summary>
    /// <param name="maze">The maze to solve.</param>
    public Solver(Maze maze)
    {
        _maze = maze;
    }

    /// <summary>
    /// Solves the maze with the given function.
    /// </summary>
    /// <param name="solve">The function to solve the maze.</param>
    /// <returns>The solution to the maze or null.</returns>
    public Stack<Square>? Solve(SolverTypes type)
    {
        switch (type)
        {
            case SolverTypes.BFS:
                return BFS();
            default:
                return null;
        }
    }

    /// <summary>
    /// Solves the maze using a breadth-first search.
    /// </summary>
    /// <returns>The solution to the maze as a stack or null if it does not exist.</returns>
    public Stack<Square>? BFS()
    {
        HashSet<Square> visited = new HashSet<Square>();
        Queue<Square> queue = new Queue<Square>();
        Dictionary<Square, Square> parent = new Dictionary<Square, Square>();
        
        queue.Enqueue(_maze.Start!);
        visited.Add(_maze.Start!);
        parent.Add(_maze.Start!, _maze.Start!);
        while (queue.Count > 0) {
            Square visiting = queue.Dequeue();
            foreach(Square square in _maze.GetNeighbours(visiting.X, visiting.Y).Where(s => !visited.Contains(s))) {
                visited.Add(square);
                parent.Add(square, visiting);
                queue.Enqueue(square);
            }
        }

        if (!visited.Contains(_maze.End!)) {
            return null;
        } else {
            Stack<Square> path = new Stack<Square>();
            Square current = parent[_maze.End!]; // Do not include end square or start square in path
            while (current != _maze.Start!) {
                path.Push(current);
                current.Type = Square.SquareType.PATH;
                current = parent[current];
            }
            return path;
        }
    }

    /// <summary>
    /// Options for solver algorithms.
    /// </summary>
    public enum SolverTypes
    {
        BFS
    }
}
