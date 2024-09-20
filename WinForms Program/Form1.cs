using TronGame.Data_Structures;
using TronGame.Game_Logic;

namespace TronGame
{
    public partial class TronGame : Form
    {
        public int imageSize = 32;  // Size of each image in the grid.
        private Random randomValue;  // Random number generator.
        public TheGrid gameGrid;  // Game grid.
        Player player, bot1, bot2, bot3;  // Player and bots.
        Graphics gpx;  // Graphics object for drawing.
        DateTime inGameTimer;  // Timer for adding new items.
        Player[] playersList;  // Array of players.

        public TronGame()
        {
            InitializeComponent();
            randomValue = new Random();
            gameGrid = new TheGrid(32, 19); // Initialize grid with dimensions 32x19. 

            player = new Player(false, 29, 9, 4, Player.Direction.Left); // Initialize main . 

            // Initialize Bots.  
            bot1 = new Player(true, 8, 4, 4, Player.Direction.Down);
            bot2 = new Player(true, 4, 9, 4, Player.Direction.Right);
            bot3 = new Player(true, 8, 14, 4, Player.Direction.Up);

            // Initialize Players Array. 
            playersList = new Player[4];
            playersList[0] = player;
            playersList[1] = bot1;
            playersList[2] = bot2;
            playersList[3] = bot3;

            energyBar.Value = player.playerEnergy; // Set initial energy bar value -Max Value-. 
            speedInfo.Text = $"Speed : MARC {player.playerSpeed}"; // Display initial speed. 
        }

        public void TronGameLoad(object sender, EventArgs e) // Load Game SetUp. 
        {
            GameField.Image = new Bitmap(1024, 608);  // Create a new bitmap for the game field.
            gpx = Graphics.FromImage(GameField.Image);  // Create graphics object from the bitmap.
            gpx.Clear(Color.Black);  // Clear the bitmap with a black background.

            inGameTimer = DateTime.Now;  // Start in-game timer.

            /*
            // Collect initial items (optinal initial setting). 
            player.CollectItem(InGameObj.HyperVelocity);
            player.CollectItem(InGameObj.NewJet);
            player.CollectItem(InGameObj.Energy);
            player.CollectItem(InGameObj.Shield);

            // Collect initial items for bots (optinal initial setting). 
            bot1.CollectItem(InGameObj.HyperVelocity);
            bot1.CollectItem(InGameObj.NewJet);
            bot1.CollectItem(InGameObj.Energy);
            bot1.CollectItem(InGameObj.Shield);
            */

            foreach (Player player in playersList) DrawPlayerInGrid(player, gameGrid); // Draw the player and bots (if any) on the grid. 

            for (int i = 0; i < 3; i++) AddGameItem(gameGrid); // Add initial game items. 

            RenderGrid(); // Render Game. 
        }

        // Key down event handler for player controls. 
        private void TronGameKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Change player direction based on key press. 
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

                // Handle item usage by number keys.
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

        private void GameLoop(object sender, EventArgs e) // Game Loop. 
        {
            if (!player.playerAlive) return; // End Game if Main Character is dead. 

            foreach (Player player in playersList) // Move the character base on their NPC description. 
            {
                if (player.isNPC) MoveNPC(player, playersList[0], gameGrid);
                else MovePlayer(player, gameGrid); 
            }

            // Add new game item periodically (4.5 seconds). 
            if (DateTime.Now - inGameTimer >= TimeSpan.FromSeconds(4.5))
            {
                AddGameItem(gameGrid);
                inGameTimer = DateTime.Now;
            }

            RenderGrid(); // Update Game Display. 
        }

        // Move the player based on current direction and handle collisions.
        private void MovePlayer(Player player, TheGrid gameGrid)
        {
            if (!player.playerAlive)
            { 
                // Update Display Stats and Screen Items, if player is dead. 
                DestroyPlayer(player, gameGrid); 
                energyBar.Value = player.playerEnergy;
                speedInfo.Text = $"Speed | MARC 0{player.playerSpeed}";
                return;
            }

            for (int s = 0; s < player.playerSpeed; s++) // Loop for Speed. 
            {
                // Handle item effects. 

                // Use New item in collection (3.5 seconds).
                if (!player.itemsCollected.IsEmpty() && DateTime.Now - player.useItemTimer >= TimeSpan.FromSeconds(3.5))
                {
                    player.HandleItem(player.itemsCollected.Pop());
                    player.useItemTimer = DateTime.Now;
                }
                // Reset to original speed (3.5 seconds).
                if (DateTime.Now - player.hyperSpeedTimer >= TimeSpan.FromSeconds(3.5))
                {
                    player.playerSpeed = 1;
                }
                // Reset invincibility (5 seconds). 
                if (player.playerInvincible && DateTime.Now - player.invincibilityTimer >= TimeSpan.FromSeconds(5))
                {
                    player.playerInvincible = false;
                }

                // Calculate new position. 
                Player.PlayerCoords lastTrailCoords = player.playerPosition;
                Player.PlayerCoords newPosition = GetNewPlayerPosition(player);

                // Check for boundary collisions or Energy depletion. 
                if (newPosition.posX < 0 || newPosition.posY < 0 || newPosition.posX >= gameGrid.rows || newPosition.posY >= gameGrid.columns || player.playerEnergy <= 0)
                {
                    player.playerAlive = false;
                    DestroyPlayer(player, gameGrid);
                    energyBar.Value = player.playerEnergy;
                    speedInfo.Text = $"Speed | MARC 0{player.playerSpeed}";
                    return;
                }

                // Update trail and player position. 
                lastTrailCoords = player.InsertNewHead(newPosition, true);
                if (!player.playerInvincible) player.playerEnergy--;
                energyBar.Value = player.playerEnergy;
                speedInfo.Text = $"Speed | MARC 0{player.playerSpeed}";

                // Handle collisions with items. 
                InGameObj itemInNewPosition = gameGrid.CheckItem(player.playerPosition.posX, player.playerPosition.posY);
                HandleCollision(player, gameGrid, itemInNewPosition);

                while (player.trail.Length() >= player.playerSize) // Maintain trail length. 
                {
                    player.trail.RemoveLast();
                }

                // Update grid with new trai. 
                gameGrid.ModifyGrid(lastTrailCoords.posX, lastTrailCoords.posY, InGameObj.Void);

                // Draw player or handle player destruction. 
                if (player.playerAlive) DrawPlayerInGrid(player, gameGrid);
                else DestroyPlayer(player, gameGrid);
            }
        }

        // Move the NPC towards the target player. 
        private void MoveNPC(Player npc, Player target, TheGrid gameGrid)
        {
            if (!npc.playerAlive)
            {
                DestroyPlayer(npc, gameGrid);
                return;
            }

            for (int s = 0; s < npc.playerSpeed; s++) // Speed Loop. 
            {
                // Handle item effects. 

                // Use New item in collection (3.5 seconds).
                if (!npc.itemsCollected.IsEmpty() && DateTime.Now - npc.useItemTimer >= TimeSpan.FromSeconds(3.5))
                {
                    npc.HandleItem(npc.itemsCollected.Pop());
                    npc.useItemTimer = DateTime.Now;
                }
                // Reset to original speed (3.5 seconds).
                if (DateTime.Now - npc.hyperSpeedTimer >= TimeSpan.FromSeconds(3.5))
                {
                    npc.playerSpeed = 1;
                }
                // Reset invincibility (5 seconds). 
                if (npc.playerInvincible && DateTime.Now - npc.invincibilityTimer >= TimeSpan.FromSeconds(5))
                {
                    npc.playerInvincible = false;
                }

                // Determine NPC's movement direction based on target position.
                int deltaX = target.playerPosition.posX - npc.playerPosition.posX;
                int deltaY = target.playerPosition.posY - npc.playerPosition.posY;

                Player.PlayerCoords newPosition = npc.playerPosition;

                if (Math.Abs(deltaX) > Math.Abs(deltaY))
                {
                    if (deltaX > 0)
                    {
                        npc.ChangeDirection(Player.Direction.Right);
                        newPosition.posX += 1;
                    }
                    else
                    {
                        npc.ChangeDirection(Player.Direction.Left);
                        newPosition.posX -= 1;
                    }
                }
                else
                {
                    if (deltaY > 0)
                    {
                        npc.ChangeDirection(Player.Direction.Down);
                        newPosition.posY += 1;
                    }
                    else
                    {
                        npc.ChangeDirection(Player.Direction.Up);
                        newPosition.posY -= 1;
                    }
                }

                // Check for boundary collisions or Energy depletion. 
                if (newPosition.posX < 0 || newPosition.posY < 0 || newPosition.posX >= gameGrid.rows || newPosition.posY >= gameGrid.columns || npc.playerEnergy <= 0)
                {
                    npc.playerAlive = false;
                    DestroyPlayer(npc, gameGrid);
                    return;
                }

                // Update trail and NPC position.
                Player.PlayerCoords lastTrailCoords = npc.InsertNewHead(newPosition, true);

                if (!npc.playerInvincible) npc.playerEnergy--;

                // Handle collisions with items. 
                InGameObj itemInNewPosition = gameGrid.CheckItem(npc.playerPosition.posX, npc.playerPosition.posY);
                HandleCollision(npc, gameGrid, itemInNewPosition);

                // Maintain trail length. 
                while (npc.trail.Length() >= npc.playerSize)
                {
                    npc.trail.RemoveLast();
                }

                // Update grid with new trail. 
                gameGrid.ModifyGrid(lastTrailCoords.posX, lastTrailCoords.posY, InGameObj.Void);

                // Draw NPC or handle NPC destruction. 
                if (npc.playerAlive) DrawPlayerInGrid(npc, gameGrid);
                else DestroyPlayer(npc, gameGrid);
            }
        }

        // Get the new position of the player based on the current direction. 
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

        // Handle collisions with items. 
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
                    if (!player.playerInvincible)
                    {
                        foreach (Player otherPlayer in playersList)
                        {
                            if (otherPlayer != player && otherPlayer.playerPosition.Equals(player.playerPosition))
                            {
                                otherPlayer.playerAlive = false;
                                DestroyPlayer(otherPlayer, gameGrid);

                                if (player == playersList[0] || otherPlayer == playersList[0])
                                {
                                    player.playerAlive = false;
                                    DestroyPlayer(player, gameGrid);
                                    energyBar.Value = player.playerEnergy;
                                    speedInfo.Text = $"Speed | MARC 0{player.playerSpeed}";
                                }
                            }
                        }

                        player.playerAlive = false;
                        DestroyPlayer(player, gameGrid);
                        if (player == playersList[0])
                        {
                            energyBar.Value = player.playerEnergy;
                            speedInfo.Text = $"Speed | MARC 0{player.playerSpeed}";
                        }
                        return;
                    }

                    break;

                case InGameObj.Energy:
                case InGameObj.HyperVelocity:
                case InGameObj.NewJet:
                case InGameObj.Shield:
                    player.CollectItem(item);
                    break;
            }
        }

        // Draw the player on the grid. 
        private void DrawPlayerInGrid(Player player, TheGrid gameGrid)
        {
            Node<Player.PlayerCoords> currentNode = player.trail.GetFirst();

            while (currentNode != null)
            {
                // Update grid with player's trail. 
                gameGrid.ModifyGrid(currentNode.Data.posX, currentNode.Data.posY, player.playerInvincible ? InGameObj.InvincibleJetWall : player.isNPC ? InGameObj.BotJetWall : InGameObj.PlayerJetWall);
                currentNode = currentNode.Next;
            }

            // Draw player head. 
            gameGrid.ModifyGrid(player.trail.GetFirst().Data.posX, player.trail.GetFirst().Data.posY, player.playerInvincible ? InGameObj.Invincible : player.isNPC ? InGameObj.Bot : InGameObj.Player);
        }

        // Add a new game item to the grid. 
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

        // Destroy a player and handle item collection.
        private void DestroyPlayer(Player player, TheGrid grid)
        {
            Node<Player.PlayerCoords> currentNode = player.trail.GetFirst();

            // Modify grid. 
            while (currentNode != null)
            {
                gameGrid.ModifyGrid(currentNode.Data.posX, currentNode.Data.posY, InGameObj.Void);
                currentNode = currentNode.Next;
            }

            // Re-add collected items to the grid. 
            while (!player.itemsCollected.IsEmpty())
            {
                AddGameItem(grid, player.itemsCollected.Pop());
            }

            player.Death(); // Player's Death. 
        }

        private void RenderGrid() 
        {
            gpx.Clear(Color.Black); // Clear the graphics object. 

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

            // Draw each player -head
            foreach (Player player in playersList)
            {
                if (player.playerAlive)
                {

                    Image playerHeadImage = GameGPX.Images[player.playerInvincible ? (int)InGameObj.Invincible : player.isNPC ? (int)InGameObj.Bot : (int)InGameObj.Player];
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

            DrawCollectedItems(); // Draw collected items. 

            GameField.Refresh(); // Refresh the game field. 
        }

        // Draw the collected items for the player. 
        private void DrawCollectedItems()
        {
            int maxItems = 10;
            Bitmap powerUpsBitmap = new Bitmap(imageSize * maxItems, imageSize);

            using (Graphics gfx = Graphics.FromImage(powerUpsBitmap))
            {
                gfx.Clear(Color.Black);

                itemStack currentStack = player.itemsCollected;

                Node<InGameObj> currentNode = currentStack.items.GetFirst();

                int index = 0;

                while (currentNode != null && index < maxItems)
                {
                    InGameObj currentItem = currentNode.Data;
                    int itemId = (int)currentItem;

                    gfx.DrawImage(GameGPX.Images[itemId], index * imageSize, 0, imageSize, imageSize);

                    currentNode = currentNode.Next;
                    index++;
                }
            }

            PowerUps.Image = powerUpsBitmap;
            PowerUps.Refresh();
        }

        // Rotate an image by a given angle. 
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