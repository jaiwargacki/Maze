import math
import constants

""" Used to solve mazes. """
__author__ = "Jai Wargacki"


class Solver:

    __slots__ = "maze"

    def __init__(self, filename):
        self.maze = Maze()
        with open(filename, constants.READ) as f:
            row = 0
            line = f.readline()
            while line != constants.EMPTY:
                col = 0
                for c in line:
                    if c != constants.WALL and c != constants.NEW_LINE:
                        node = Node(row, col)
                        self.maze.add(node)
                        if self.maze.exists(row-1, col):
                            self.maze.connect(node, Node(row-1, col))
                        if self.maze.exists(row, col-1):
                            self.maze.connect(node, Node(row, col-1))
                        if c == constants.START:
                            self.maze.start = node
                        elif c == constants.END:
                            self.maze.end = node
                    col += 1
                row += 1
                line = f.readline()

    def solve(self, display, type_search="dfs"):
        """
        Solves the maze, printing result to the provided display.
        :param display: The display to print result.
        :param type_search: Type of search to be user.
        """
        if type_search == "dfs":
            solution = self.maze.dfs(display, self.maze.start)
        # TODO - Implement more searches

        for n in solution:
            display.draw_square(constants.FOUND_PATH_COLOR, n.col, n.row, False)
        display.draw_square(constants.START_COLOR, self.maze.start.col, self.maze.start.row)
        display.draw_square(constants.END_COLOR, self.maze.end.col, self.maze.end.row)


class Maze:

    __slots__ = "nodes", "connections", "start", "end"

    def __init__(self):
        """
        Constructor for Maze object.
        """
        self.nodes = set()
        self.connections = dict()
        self.start = None
        self.end = None

    def __str__(self):
        """
        String method for Maze object.
        :return: Return string with information of Nodes, start, and end.
        """
        s = "Nodes: "
        for n in self.nodes:
            s += "\t" + str(n) + "\t{" + str(self.connections[n]) + "}\n"
        s += "Start: " + str(self.start) + "\n"
        s += "End: " + str(self.end) + "\n"
        return s

    def add(self, node):
        """
        Add a Node to the Maze.
        :param node: The Node to add.
        """
        self.nodes.add(node)
        self.connections[node] = set()

    def exists(self, row, col):
        """
        Checks if the Node at a row and col exists.
        :param row: Row of Node.
        :param col: Column of Node.
        :return: True if the Node exists, False otherwise.
        """
        n = Node(row, col)
        if n in self.nodes:
            return True
        else:
            return False

    def connect(self, node1, node2):
        """
        Connect two nodes together.
        :param node1: First Node to connect.
        :param node2: Second Node to connect.
        """
        self.connections[node1].add(node2)
        self.connections[node2].add(node1)

    def dfs(self, display, current, visited=None):
        """
        Solves maze using dfs, printing results to GUI.
        :param display: Display to print results.
        :param current: Node considering.
        :param visited: Set of already visited nodes.
        :return: List of steps, empty if not solvable.
        """
        if visited is None:
            visited = set()
            visited.add(current)
        for n in self.connections[current]:
            if n == self.end:
                return [n]
            if n not in visited:
                visited.add(n)
                display.draw_square(constants.CONSIDERING_PATH_COLOR, n.col, n.row)
                a = self.dfs(display, n, visited)
                if a:
                    return [n] + a
        return []


class Node:

    __slots__ = "row", "col"

    def __init__(self, row, col):
        """
        Constructor for Node object.
        :param row: the Node's row.
        :param col: the Node's column.
        """
        self.row = row
        self.col = col

    def __eq__(self, other):
        """
        Equals method for Node object.
        :param other: The other object.
        :return: True if row and col are equal. False otherwise.
        """
        return other.row == self.row and other.col == self.col

    def __hash__(self):
        """
        Hash method for Node.
        :return: col raised to the row.
        """
        return int(math.pow(self.col, self.row))

    def __str__(self):
        """
        String method for a Node.
        :return: String in the form {r:<row>,c:<col>}.
        """
        return "{r:" + str(self.row) + ",c:" + str(self.col) + "}"
