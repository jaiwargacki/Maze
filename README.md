# Maze
This repository contains a maze program which can be used to demonstrate different path finding algorithms.

## Usage

Run the main game file `maze.py` 
which takes 2 or 3 addition commandline arguments.

`Usage : maze.py <width-of-maze> <height-of-maze> [<starting-maze-file>]`

The width and height should be integers greater than 0.
The optional starting maze should contain the maze in the form of 'X' for walls, 
' ' for empty space, 'S' for start, and 'E' for end space; newlines are ignored.

Once program is running you can edit the maze by right-clicking to add empty space,
right-click while holding `space` to add walls,
press 's' to add a start where your mouse currently is, press 'e' 
to add a start where your mouse currently is. 

To exit without saving press 'esc' or press the x. To save and quit
press 'q'. The maze will be saved to `maze_temp.txt`.


## Future Elaborations
* Maze solving functionality.
* More intuitive initiation of the program.

## Licence

MIT License\
See LICENSE for details.