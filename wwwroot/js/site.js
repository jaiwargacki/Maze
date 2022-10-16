// Types of squares
var TYPES = {
    'EMPTY': 0,
    'WALL': 1,
    'START': 2,
    'END': 3,
    'PATH':4
};

// Trackers for current square type and current start/end squares
var currentSquareType = TYPES['EMPTY'];
var currentStartSquare = null;
var currentEndSquare = null;

/**
 * Updates the cells in the maze with the data from the server.
 * @param {*} data data.upate contains an array of cells to update.
 */
function UpdateCells(data) {
    for (var i = 0; i < data.update.length; i++) {
        var square = document.getElementById(data.update[i].x + ":" + data.update[i].y);
        var squareType = Object.keys(TYPES).find(key => TYPES[key] === data.update[i].type);
        square.setAttribute("class", "square square" + squareType);
    }
}

/**
 * Update the html as needed for the current square type.
 * @param {*} squareType int value of the square type.
 */
function SelectSquareType(squareType) {
    document.getElementById("SQUARETYPE:" + currentSquareType).removeAttribute("class");
    document.getElementById("SQUARETYPE:" + squareType).setAttribute("class", "clicked");
    currentSquareType = squareType;
    var maze = document.getElementById('Maze');
    switch (squareType) {
        case TYPES['EMPTY']:
            maze.setAttribute("class", "hoverEMPTY");
            break;
        case TYPES['WALL']:
            maze.setAttribute("class", "hoverWALL");
            break;
        case TYPES['START']:
            maze.setAttribute("class", "hoverSTART");
            break;
        case TYPES['END']:
            maze.setAttribute("class", "hoverEND");
            break;
    }
}

/**
 * Clears the path on the maze.
 */
function ClearPath() {
    $.ajax({
        type: "POST",
        url: "/Home/ClearPath",
        success: function (data) {
            if (data.success === true) {
                UpdateCells(data);
            }
        }
    });
}

/**
 * Finds a path from the start to the end.
 */
function Solve() {
    ClearPath();

    $.ajax({
        type: "POST",
        url: "/Home/Solve",
        data: { 
            solveType : "BFS"
        },
        success: function (data) {
            if (data.success === true) {
                UpdateCells(data);
            } else {
                alert(data.message);
            }
        }
    });
}

/**
 * Selects a square on the maze and updates the server.
 * @param {*} event event that triggered the selection.
 * @param {*} square square that was selected.
 */
function SelectSquare(event, square) {
    if ( event.buttons === 1 ) {
        ClearPath();

        $.ajax({
            type: "POST",
            url: "/Home/SelectCell",
            data: { 
                x: square.getAttribute('valuex'), 
                y: square.getAttribute('valuey'),
                squareTypeInt : currentSquareType
            },
            success: function (data) {
                if (data === true) {
                    switch (currentSquareType) {
                        case TYPES['EMPTY']:
                            square.setAttribute('class', "square squareEMPTY");
                            break;
                        case TYPES['WALL']:
                            square.setAttribute('class', "square squareWALL");
                            break;
                        case TYPES['START']:
                            if (currentStartSquare != null) {
                                currentStartSquare.setAttribute('class', "square squareEMPTY");
                            }
                            currentStartSquare = square;
                            square.setAttribute('class', "square squareSTART");
                            break;
                        case TYPES['END']:
                            if (currentEndSquare != null) {
                                currentEndSquare.setAttribute('class', "square squareEMPTY");
                            }
                            currentEndSquare = square;
                            square.setAttribute('class', "square squareEND");
                            break;
                        case TYPES['PATH']:
                            square.setAttribute('class', "square squarePATH");
                            break;
                    }
                }
            }
        });
    }
}

/**
 * Creates the maze on the page.
 */
function ResizeMaze() {
    $.ajax({
        type: "POST",
        url: "/Home/ResizeMaze",
        data: {
            width: Math.floor((window.innerWidth * 0.8) / 32), 
            height: Math.floor((window.innerHeight * 0.7) / 32) 
        },
        success: function (data) {
            var mazeHolder = document.getElementById('MazeContainer')
            mazeHolder.innerHTML = data;
            var $squares = [].slice.call( document.querySelectorAll( '.square' ) );
            $squares.map( function ( square ) {
                square.addEventListener( 'mousedown', function ( e ) {
                    SelectSquare(e, square)
                } );
                square.addEventListener( 'mouseover', function ( e ) {
                    SelectSquare(e, square)
                } );
            } );
        }
    });
    SelectSquareType(currentSquareType);
}

// Initialize the maze
ResizeMaze();