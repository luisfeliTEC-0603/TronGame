
using static TronGame.TronGame;

namespace TronGame.Game_Logic
{
    public class TheGrid
    {
        public GridNode[,] _grid { get; private set; }
        public int rows { get; }
        public int columns { get; }

        public TheGrid(int numRows, int numColumns)
        {
            rows = numRows;
            columns = numColumns;
            _grid = new GridNode[rows, columns];
            InitializeGrid(rows, columns);
        }

        private void InitializeGrid(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _grid[i, j] = new GridNode(i, j, InGameObj.Void);
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _grid[i, j]._north = i > 0 ? _grid[i - 1, j] : _grid[rows - 1, j];
                    _grid[i, j]._east = j < columns - 1 ? _grid[i, j + 1] : _grid[i, 0];
                    _grid[i, j]._south = i < rows - 1 ? _grid[i + 1, j] : _grid[0, j];
                    _grid[i, j]._west = j > 0 ? _grid[i, j - 1] : _grid[i, columns - 1];
                }
            }
        }

        public InGameObj ModifyGrid(int coordX, int coordY, InGameObj obj)
        {
            if (coordX < 0 || coordX >= _grid.GetLength(0) || coordY < 0 || coordY >= _grid.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("Coordinates Exceeded.");
            }

            GridNode itemInNode = _grid[coordX, coordY];
            _grid[coordX, coordY] = new GridNode(coordX, coordY, obj);

            return itemInNode._occupyingObj;
        }

        public InGameObj CheckItem(int coordX, int coordY)
        {
            return _grid[coordX, coordY]._occupyingObj;
        }

        public class GridNode
        {
            public GridNode _north { get; set; }
            public GridNode _east { get; set; }
            public GridNode _west { get; set; }
            public GridNode _south { get; set; }
            public InGameObj _occupyingObj { get; set; }
            public int _posX { get; private set; }
            public int _posY { get; private set; }

            public GridNode(int x, int y, InGameObj gameObj)
            {
                _posX = x;
                _posY = y;
                _occupyingObj = gameObj;
            }
        }
    }
}
