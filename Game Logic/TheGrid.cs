namespace TronGame.Game_Logic
{
    public class TheGrid
    {
        // 2D array of GridNodes. 
        public GridNode[,] grid { get; private set; }

        public int rows { get; }
        public int columns { get; }

        public TheGrid(int numRows, int numColumns) // Constructor. 
        {
            rows = numRows;
            columns = numColumns;
            grid = new GridNode[rows, columns];
            InitializeGrid(rows, columns);
        }

        // Initialize the grid with GridNode objects and set up neighbors.
        private void InitializeGrid(int rows, int columns)
        {
            // Create GridNode objects for each cell in the grid.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i, j] = new GridNode(i, j, InGameObj.Void);
                }
            }

            // Set up the north, east, south, and west neighbors for each GridNode.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i, j]._north = i > 0 ? grid[i - 1, j] : grid[rows - 1, j];
                    grid[i, j]._east = j < columns - 1 ? grid[i, j + 1] : grid[i, 0];
                    grid[i, j]._south = i < rows - 1 ? grid[i + 1, j] : grid[0, j];
                    grid[i, j]._west = j > 0 ? grid[i, j - 1] : grid[i, columns - 1];
                }
            }
        }

        // Modify the GridNode at the specified coordinates. 
        public InGameObj ModifyGrid(int coordX, int coordY, InGameObj obj)
        {
            // Validate the coordinates.
            if (coordX < 0 || coordX >= grid.GetLength(0) || coordY < 0 || coordY >= grid.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("Coordinates Exceeded.");
            }

            // Save the existing object and update the GridNode with the new object.
            GridNode itemInNode = grid[coordX, coordY];
            grid[coordX, coordY] = new GridNode(coordX, coordY, obj);

            return itemInNode._occupyingObj;
        }

        // Check the object occupying the GridNode at the specified coordinates.
        public InGameObj CheckItem(int coordX, int coordY)
        {
            return grid[coordX, coordY]._occupyingObj;
        }

        // Nested Node Class.
        public class GridNode
        {
            // Neighbors of the GridNode.
            public GridNode _north { get; set; }
            public GridNode _east { get; set; }
            public GridNode _west { get; set; }
            public GridNode _south { get; set; }

            // Object occupying the GridNode.
            public InGameObj _occupyingObj { get; set; }

            // Coordinates.
            public int _posX { get; private set; }
            public int _posY { get; private set; }

            public GridNode(int x, int y, InGameObj gameObj) // Constructor. 
            {
                _posX = x;
                _posY = y;
                _occupyingObj = gameObj;
            }
        }
    }
}
