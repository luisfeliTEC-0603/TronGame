
using TronGame.Data_Structures;
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
        Player[] playersList;

        public TronGame()
        {
            InitializeComponent();
            randomValue = new Random();
            gameGrid = new TheGrid(32, 19);

            player = new Player(10, 11, 5, Player.Direction.Up);

            playersList = new Player[1];
            playersList[0] = player;

            energyBar.Value = player.playerEnergy;
            speedInfo.Text = $"Speed : MARC {player.playerSpeed}";
        }

        public void TronGameLoad(object sender, EventArgs e)
        {
            GameField.Image = new Bitmap(1024, 608);
            gpx = Graphics.FromImage(GameField.Image);
            gpx.Clear(Color.Black);

            inGameTimer = DateTime.Now;

            foreach (Player player in playersList) DrawPlayerInGrid(player, gameGrid);

            for (int i = 0; i < 9; i++) AddGameItem(gameGrid);

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

                case Keys.D1:
                case Keys.NumPad1:
                    player.itemsCollected.SetElementAsHead(1);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    player.itemsCollected.SetElementAsHead(2);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    player.itemsCollected.SetElementAsHead(3);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    player.itemsCollected.SetElementAsHead(4);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    player.itemsCollected.SetElementAsHead(5);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    player.itemsCollected.SetElementAsHead(6);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    player.itemsCollected.SetElementAsHead(7);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    player.itemsCollected.SetElementAsHead(8);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    player.itemsCollected.SetElementAsHead(9);
                    break;
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {

            foreach (Player player in playersList) MovePlayer(player, gameGrid);

            if (DateTime.Now - inGameTimer >= TimeSpan.FromSeconds(3))
            {
                AddGameItem(gameGrid);
                inGameTimer = DateTime.Now;
            }

            RenderGrid();
        }

        public void MovePlayer(Player player, TheGrid gameGrid)
        {
            if (!player.playerAlive)
            {
                energyBar.Value = player.playerEnergy;
                speedInfo.Text = $"Speed | MARC 0{player.playerSpeed}";
                return;
            }

            for (int s = 0; s < player.playerSpeed; s++)
            {

                if (!player.itemsCollected.IsEmpty() && DateTime.Now - player.useItemTimer >= TimeSpan.FromSeconds(3.5))
                {
                    player.HandleItem(player.itemsCollected.Pop());
                }
                if (DateTime.Now - player.hyperSpeedTimer >= TimeSpan.FromSeconds(3.5))
                {
                    player.playerSpeed = 1;
                }

                Player.PlayerCoords lastTrailCoords = player.playerPosition;
                Player.PlayerCoords newPosition = GetNewPlayerPosition(player);

                if (newPosition.posX < 0 || newPosition.posY < 0 || newPosition.posX >= gameGrid.rows || newPosition.posY >= gameGrid.columns)
                {
                    DestroyPlayer(player, gameGrid);
                    return;
                }

                lastTrailCoords = player.InsertNewHead(newPosition, true);
                if (!player.playerInvincible) player.playerEnergy--;
                energyBar.Value = player.playerEnergy;
                speedInfo.Text = $"Speed | MARC 0{player.playerSpeed}";
                if (player.playerEnergy <= 0) player.playerAlive = false;

                InGameObj itemInNewPosition = gameGrid.CheckItem(player.playerPosition.posX, player.playerPosition.posY);
                HandleCollision(player, gameGrid, itemInNewPosition);

                while (player.trail.Length() >= player.playerSize)
                {
                    player.trail.RemoveLast();
                }

                gameGrid.ModifyGrid(lastTrailCoords.posX, lastTrailCoords.posY, InGameObj.Void);

                if (player.playerAlive) DrawPlayerInGrid(player, gameGrid);
                else DestroyPlayer(player, gameGrid);
            }
        }

        private Player.PlayerCoords GetNewPlayerPosition(Player player)
        {
            switch (player.playerDirection)
            {
                case Player.Direction.Up:
                    return new Player.PlayerCoords(player.playerPosition.posX, player.playerPosition.posY - 1);
                case Player.Direction.Down:
                    return new Player.PlayerCoords(player.playerPosition.posX, player.playerPosition.posY + 1);
                case Player.Direction.Right:
                    return new Player.PlayerCoords(player.playerPosition.posX + 1, player.playerPosition.posY);
                case Player.Direction.Left:
                    return new Player.PlayerCoords(player.playerPosition.posX - 1, player.playerPosition.posY);
                default:
                    return player.playerPosition;
            }
        }

        private void HandleCollision(Player player, TheGrid gameGrid, InGameObj item)
        {
            switch (item)
            {
                case InGameObj.Bomb:
                case InGameObj.Invincible:
                case InGameObj.InvincibleJetWall:
                case InGameObj.Player:
                case InGameObj.PlayerJetWall:
                case InGameObj.Bot:
                case InGameObj.BotJetWall:
                    if (!player.playerInvincible) DestroyPlayer(player, gameGrid);
                    break;

                case InGameObj.Energy:
                case InGameObj.HyperVelocity:
                case InGameObj.NewJet:
                case InGameObj.Shield:
                    player.CollectItem(item);
                    break;
            }
        }

        private void DrawPlayerInGrid(Player player, TheGrid gameGrid)
        {
            var currentNode = player.trail.GetFirst();

            if (player.playerInvincible && DateTime.Now - player.invincibilityTimer >= TimeSpan.FromSeconds(5)) player.playerInvincible = false;

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

        private void DestroyPlayer(Player player, TheGrid grid)
        {
            var currentNode = player.trail.GetFirst();
            while (currentNode != null)
            {
                gameGrid.ModifyGrid(currentNode.Data.posX, currentNode.Data.posY, InGameObj.Void);
                currentNode = currentNode.Next;
            }

            while (!player.itemsCollected.IsEmpty())
            {
                AddGameItem(grid, player.itemsCollected.Pop());
            }

            player.Death();
        }

        private void RenderGrid()
        {
            gpx.Clear(Color.Black);

            for (int i = 0; i < gameGrid.rows; i++)
            {
                for (int j = 0; j < gameGrid.columns; j++)
                {
                    InGameObj item = gameGrid.CheckItem(i, j);

                    if (item != InGameObj.Void && item != InGameObj.Player && item != InGameObj.Bot && item != InGameObj.Invincible)
                    {
                        gpx.DrawImage(GameGPX.Images[(int)item], i * imageSize, j * imageSize);
                    }
                }
            }

            foreach (Player player in playersList)
            {
                if (player.playerAlive)
                {

                    Image playerHeadImage = GameGPX.Images[player.playerInvincible ? (int)InGameObj.Invincible : (int)InGameObj.Player];
                    int rotationAngle = 0;

                    switch (player.playerDirection)
                    {
                        case Player.Direction.Up:
                            rotationAngle = 0;
                            break;
                        case Player.Direction.Right:
                            rotationAngle = 90;
                            break;
                        case Player.Direction.Down:
                            rotationAngle = 180;
                            break;
                        case Player.Direction.Left:
                            rotationAngle = 270;
                            break;
                    }

                    Image rotatedHeadImage = RotateImage(playerHeadImage, rotationAngle);

                    gpx.DrawImage(rotatedHeadImage, player.playerPosition.posX * imageSize, player.playerPosition.posY * imageSize);

                    DrawPlayerInGrid(player, gameGrid);
                }
            }

            DrawCollectedItems();

            GameField.Refresh();
        }

        private void DrawCollectedItems()
        {
            int itemSize = 32;
            int maxItems = 9;
            Bitmap powerUpsBitmap = new Bitmap(itemSize * maxItems, itemSize);

            using (Graphics gfx = Graphics.FromImage(powerUpsBitmap))
            {
                gfx.Clear(Color.Black);

                var tempStack = new PriorityStack<InGameObj>();  
                var currentStack = player.itemsCollected; 

                int index = 0;

                while (!currentStack.IsEmpty() && index < maxItems)
                {
                    var currentItem = currentStack.Peek();
                    int itemId = (int)currentItem; 

                    gfx.DrawImage(GameGPX.Images[itemId], index * itemSize, 0, itemSize, itemSize);

                    tempStack.Push(currentStack.Pop(), index);

                    index++;
                }

                while (!tempStack.IsEmpty())
                {
                    var item = tempStack.Pop();
                    currentStack.Push(item, index);
                }
            }

            PowerUps.Image = powerUpsBitmap;
            PowerUps.Refresh();
        }

        private Image RotateImage(Image img, float rotationAngle)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.TranslateTransform((float)img.Width / 2, (float)img.Height / 2);
                gfx.RotateTransform(rotationAngle);
                gfx.TranslateTransform(-(float)img.Width / 2, -(float)img.Height / 2);
                gfx.DrawImage(img, new Point(0, 0));
            }
            return bmp;
        }
    }
}
