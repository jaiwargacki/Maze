const EMPTY = 0;
const WALL = 1;
const START = 2;
const END = 3;
const PATH = 4;

var currentSquareType = EMPTY;
var currentStartSquare = null;
var currentEndSquare = null;

function SelectSquareType(squareType) {
    document.getElementById("SQUARETYPE:" + currentSquareType).removeAttribute("class");
    document.getElementById("SQUARETYPE:" + squareType).setAttribute("class", "clicked");
    currentSquareType = squareType;
    var maze = document.getElementById('Maze');
    switch (squareType) {
        case EMPTY:
            maze.setAttribute("class", "hoverEMPTY");
            break;
        case WALL:
            maze.setAttribute("class", "hoverWALL");
            break;
        case START:
            maze.setAttribute("class", "hoverSTART");
            break;
        case END:
            maze.setAttribute("class", "hoverEND");
            break;
    }
}

function ClearPath() {
    $.ajax({
        type: "POST",
        url: "/Home/ClearPath",
        success: function (data) {
            if (data.success === true) {
                for (var i = 0; i < data.clear.length; i++) {
                    var square = document.getElementById(data.clear[i].x + ":" + data.clear[i].y);
                    square.setAttribute("class", "square squareEMPTY");
                }
            }
        }
    });
}

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
                for (var i = 0; i < data.path.length; i++) {
                    var square = document.getElementById(data.path[i].x + ":" + data.path[i].y);
                    square.setAttribute("class", "square squarePATH");
                }
            } else {
                alert(data.message);
            }
        }
    });
}

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
                        case EMPTY:
                            square.setAttribute('class', "square squareEMPTY");
                            break;
                        case WALL:
                            square.setAttribute('class', "square squareWALL");
                            break;
                        case START:
                            if (currentStartSquare != null) {
                                currentStartSquare.setAttribute('class', "square squareEMPTY");
                            }
                            currentStartSquare = square;
                            square.setAttribute('class', "square squareSTART");
                            break;
                        case END:
                            if (currentEndSquare != null) {
                                currentEndSquare.setAttribute('class', "square squareEMPTY");
                            }
                            currentEndSquare = square;
                            square.setAttribute('class', "square squareEND");
                            break;
                        case PATH:
                            square.setAttribute('class', "square squarePATH");
                            break;
                    }
                }
            }
        });
    }
}

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

ResizeMaze();