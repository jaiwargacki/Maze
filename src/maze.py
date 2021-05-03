import sys
import display as d

""" Main file for the maze program. """
__author__ = "Jai Wargacki"

""" Usage message for running program from commandline. """
USAGE = "Usage : maze.py <width-of-maze> <height-of-maze> [<starting-maze-file>]"


def main():
    """
    Main function for the maze program.
    """
    # Parse commandline arguments
    args = sys.argv
    if len(args) < 3:
        print(USAGE, file=sys.stderr)
        exit(1)
    width = int(args[1])
    height = int(args[2])
    if len(args) == 4:
        display = d.Display(width, height, args[3])
    else:
        display = d.Display(width, height)

    # Call display program
    display.run()


if __name__ == '__main__':
    main()
