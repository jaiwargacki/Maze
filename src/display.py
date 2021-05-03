import pygame

""" File used to display mazes. Capable of loading/saving mazes. """
__author__ = "Jai Wargacki"

# Display constants
UNIT = 10
WINDOW_TITLE = "Maze"

# Character constants
WALL = "X"
BLANK = " "
START = "S"
END = "E"

# Color constants
WALL_COLOR = (0, 0, 0)
BLANK_COLOR = (255, 255, 255)
START_COLOR = (0, 200, 0)
END_COLOR = (0, 0, 200)

# File stuff
DEFAULT_FILE_NAME = "temp_maze.txt"


class Display:
    __slots__ = "maze", "width", "height", "screen", "file"

    def __init__(self, width, height, filename=None):
        """
        Initiate Display object.
        :param width: width of the maze.
        :param height: height of the maze.
        :param filename: filename containing maze info (optional).
        """
        self.width = width
        self.height = height
        self.maze = list()
        pygame.init()
        dimensions = (UNIT * self.width, UNIT * self.height)
        self.screen = pygame.display.set_mode(dimensions)
        pygame.display.set_caption(WINDOW_TITLE)
        f = None
        if filename is not None:
            self.file = filename
            f = open(filename, "r")
        else:
            self.file = DEFAULT_FILE_NAME
        for col in range(0, self.height):
            self.maze.append(list())
            for row in range(0, self.width):
                self.maze[col].append(0)
                if filename is None:
                    self.update_square(row, col, WALL)
                else:
                    while True:
                        n = f.read(1)
                        if n != "\n":
                            self.update_square(row, col, n)
                            break
        if filename is not None:
            f.close()
        pygame.display.update()

    def __str__(self):
        """
        String method used for saving maze to file.
        :return: String version of current state of the display maze.
        """
        final = ""
        for col in range(0, self.height):
            for row in range(0, self.width):
                final += str(self.maze[col][row])
            final += "\n"
        return final

    def get_square(self, row, col):
        """
        Get the state of a square in the maze.
        :param row: row of the square.
        :param col: column of the square.
        :return: the state of the given square.
        """
        return self.maze[col][row]

    def draw_square(self, color, x, y):
        """
        Used to draw a square to the screen.
        :param color: color of square.
        :param x: x-coordinate of maze square.
        :param y: y-coordinate of maze square.
        """
        pygame.draw.rect(self.screen, color, pygame.Rect(x * UNIT, y * UNIT, UNIT, UNIT))
        pygame.display.update()

    def update_square(self, row, col, state):
        """
        Update a square of the maze with a new state
        :param row: row of the square.
        :param col: column of the square.
        :param state: the state of the given square.
        """
        self.maze[col][row] = state
        color = None
        if state == WALL:
            color = WALL_COLOR
        elif state == BLANK:
            color = BLANK_COLOR
        elif state == START:
            color = START_COLOR
        elif state == END:
            color = END_COLOR
        self.draw_square(color, row, col)

    def save_maze(self):
        """
        Saves the current maze to a file.
        """
        f = open(self.file, "w")
        s = str(self)
        f.write(s)
        f.close()
        exit(0)

    def run(self):
        """
        Run the maze program.
        """
        while True:
            for event in pygame.event.get():
                pos = pygame.mouse.get_pos()
                x = pos[0] // UNIT
                y = pos[1] // UNIT
                pressed = pygame.key.get_pressed()
                if pygame.mouse.get_pressed(3)[0] and pressed[pygame.K_SPACE]:
                    self.update_square(x, y, WALL)
                elif pygame.mouse.get_pressed(3)[0]:
                    self.update_square(x, y, BLANK)
                elif pressed[pygame.K_s]:
                    self.update_square(x, y, START)
                elif pressed[pygame.K_e]:
                    self.update_square(x, y, END)
                elif pressed[pygame.K_m]:
                    continue
                    # TODO - Save maze state and call solver
                elif pressed[pygame.K_q]:
                    self.save_maze()
                elif pressed[pygame.K_ESCAPE] or event.type == pygame.QUIT:
                    pygame.display.quit()
                    return
