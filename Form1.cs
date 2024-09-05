
using TronGame.Game_Logic;

namespace TronGame
{
    public partial class TronGame : Form
    {
        public enum InGameObj
        {
            Void = 0,

            Bomb = 1,

            Energy = 2,
            HyperVelocity = 3,
            NewJet = 4,
            Shield = 5,

            InvincibleJetWall = 6,
            PlayerJetWall = 7,
            BotJetWall = 8,
            Invincible = 9,
            Player = 10,
            Bot = 11,
        }

        public int imageSize = 32;
        private Random randomValue;
        public TheGrid gameGrid;
        Player player;
        Graphics gpx;
        DateTime inGameTimer;
        
        public TronGame()
        {
            InitializeComponent();
            randomValue = new Random();
            gameGrid = new TheGrid(32, 19);

            player = new Player(10, 11, 5, 3, Player.Direction.Up);
        }

        public void TronGameLoad(object sender, EventArgs e)
        {
            GameField.Image = new Bitmap(1024, 608);
            gpx = Graphics.FromImage(GameField.Image);
            gpx.Clear(Color.Black);
            inGameTimer = DateTime.Now;

            DrawPlayerInGrid(player, gameGrid);

            for (int i = 0; i < 2; i++) AddGameItem(gameGrid);

            RenderGrid();
        }

        private void TronGameKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (player.playerAlive) player.ChangeDirection(Player.Direction.Up);
                    break;
                case Keys.Down:
                    if (player.playerAlive) player.ChangeDirection(Player.Direction.Down);
                    break;
                case Keys.Right:
                    if (player.playerAlive) player.ChangeDirection(Player.Direction.Right);
                    break;
                case Keys.Left:
                    if (player.playerAlive) player.ChangeDirection(Player.Direction.Left);
                    break;
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            //------
            Player.PlayerCoords lastTrailCoords = player.playerPosition;

            switch (player.playerDirection)
            {
                case Player.Direction.Up:
                    lastTrailCoords = player.InsertNewHead(new Player.PlayerCoords(player.playerPosition.posX, player.playerPosition.posY - 1), true);
                    break;
                case Player.Direction.Down:
                    lastTrailCoords = player.InsertNewHead(new Player.PlayerCoords(player.playerPosition.posX, player.playerPosition.posY + 1), true);
                    break;
                case Player.Direction.Right:
                    lastTrailCoords = player.InsertNewHead(new Player.PlayerCoords(player.playerPosition.posX + 1, player.playerPosition.posY), true);
                    break;
                case Player.Direction.Left:
                    lastTrailCoords = player.InsertNewHead(new Player.PlayerCoords(player.playerPosition.posX - 1, player.playerPosition.posY), true);
                    break;
            }

            if (gameGrid.CheckItem(player.playerPosition.posX, player.playerPosition.posY) != InGameObj.Void)
            {
                InGameObj item = gameGrid.CheckItem(player.playerPosition.posX, player.playerPosition.posY);

                switch (item)
                {
                    case InGameObj.Bomb:
                    case InGameObj.Invincible:
                    case InGameObj.InvincibleJetWall:
                    case InGameObj.Player:
                    //case InGameObj.PlayerJetWall:
                    case InGameObj.Bot:
                    case InGameObj.BotJetWall:
                        if (!player.playerInvincible) player.playerAlive = false;
                        break;

                    case InGameObj.PlayerJetWall:
                        AddGameItem(gameGrid);
                        break;

                    case InGameObj.Energy:
                    case InGameObj.HyperVelocity:
                    case InGameObj.NewJet:
                    case InGameObj.Shield:
                        player.CollectItem(item);
                        break;
                }

                RenderGrid();
            }

            // Maintain the correct trail length
            while (player.trail.Length() >= player.playerSize)
            {
                player.trail.RemoveLast();
            }

            // Update the grid with the new player and trail positions
            gameGrid.ModifyGrid(lastTrailCoords.posX, lastTrailCoords.posY, InGameObj.Void); // Clear the last trail position

            if (player.playerAlive) DrawPlayerInGrid(player, gameGrid); // Draw the updated player and trail positions
            else DestroyPlayer(player);
            //------

            if (DateTime.Now - inGameTimer >= TimeSpan.FromSeconds(1.5)) 
            {
                AddGameItem(gameGrid);
                inGameTimer = DateTime.Now;
            }

            RenderGrid();
        }

        private void DrawPlayerInGrid(Player player, TheGrid gameGrid)
        {
            var currentNode = player.trail.GetFirst();

            while (currentNode != null)
            {
                gameGrid.ModifyGrid(currentNode.Data.posX, currentNode.Data.posY, player.playerInvincible ? InGameObj.InvincibleJetWall : InGameObj.PlayerJetWall);
                currentNode = currentNode.Next;
            }

            gameGrid.ModifyGrid(player.trail.GetFirst().Data.posX, player.trail.GetFirst().Data.posY, player.playerInvincible ? InGameObj.Invincible : InGameObj.Player);
        }

        private void AddGameItem(TheGrid gameBoard, InGameObj? specificItem = null)
        {
            int coordX, coordY;
            InGameObj itemToPlace = specificItem ?? (InGameObj)randomValue.Next(1, 6);

            do
            {
                coordX = randomValue.Next(0, gameBoard.rows);
                coordY = randomValue.Next(0, gameBoard.columns);

            } while (gameBoard.CheckItem(coordX, coordY) != InGameObj.Void);

            gameBoard.ModifyGrid(coordX, coordY, itemToPlace);
        }

        private void DestroyPlayer(Player player)
        {
            var currentNode = player.trail.GetFirst();
            while (currentNode != null)
            {
                gameGrid.ModifyGrid(currentNode.Data.posX, currentNode.Data.posY, InGameObj.Void);
                currentNode = currentNode.Next;
            }
        }

        private void RenderGrid()
        {
            gpx.Clear(Color.Black);

            for (int i = 0; i < gameGrid.rows; i++)
            {
                for (int j = 0; j < gameGrid.columns; j++)
                {
                    InGameObj item = gameGrid.CheckItem(i, j);
                    
                    if (item != InGameObj.Void)
                    {
                       gpx.DrawImage(GameGPX.Images[(int)item], i * imageSize, j * imageSize);
                    }
                }
            }

            GameField.Refresh();
        }
    }
}
