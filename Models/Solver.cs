namespace Maze.Models;

public class Solver
{
    private readonly Maze _maze;

    public Solver(Maze maze)
    {
        _maze = maze;
    }

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
}
