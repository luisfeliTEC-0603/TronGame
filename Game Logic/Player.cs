using TronGame.Data_Structures;

namespace TronGame.Game_Logic
{
    public class Player
    {
        public bool playerAlive { get; set; } // Indicates if the player is alive.

        // Struct to hold the player's coordinates.
        public struct PlayerCoords
        {
            public int posX { get; set; }  // X-coordinate.
            public int posY { get; set; }  // Y-coordinate.

            public PlayerCoords(int x, int y) // Constructor to initialize coordinates.
            {
                posX = x;
                posY = y;
            }
        }

        public PlayerCoords playerPosition { get; set; } // Player's current position -head position in grid-.
        public int playerEnergy { get; set; } // Player's energy level.
        public int playerSize { get; set; } // Player's size (length of body).
        public int playerSpeed { get; set; } // Player's current speed. 
        public bool playerInvincible { get; set; } // Indicates if the player is invincible. 
        public SimpleLinkedList<PlayerCoords> trail { get; set; } // List of coordinates representing the player's trail. 
        public itemStack itemsCollected { get; set; } // Stack of items collected by the player. 

        // Enum for player direction
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public Direction playerDirection { get; private set; } // Player's current direction

        private Random randomValue = new Random(); // Random number generator. 

        // Timers for various power-ups.
        public DateTime invincibilityTimer { get; set; }
        public DateTime useItemTimer { get; set; }
        public DateTime hyperSpeedTimer { get; set; }

        public bool isNPC { get; set; } // Indicates if the player is an NPC/Bot. 

        public Player(bool npc, int initialCoordX, int initialCoordY, int initialSize, Direction initialDirection) // Constructor. 
        {
            // Boolean attributes. 
            playerAlive = true;
            isNPC = npc;
            playerInvincible = false;

            // Stats-related attributes.
            playerEnergy = 500; 
            playerSize = initialSize;
            playerSpeed = 1; 
            playerDirection = initialDirection;

            // Position-related attributes. 
            playerPosition = new PlayerCoords(initialCoordX, initialCoordY);
            trail = new SimpleLinkedList<PlayerCoords>();
            
            itemsCollected = new itemStack(); // Items Collected attribute. 

            // Initialize the player's trail based on the initial direction
            for (int i = 0; i < initialSize - 1; i++)
            {
                PlayerCoords newPosition = new PlayerCoords();

                switch (initialDirection)
                {
                    case Direction.Up:
                        newPosition = new PlayerCoords(initialCoordX, initialCoordY + i);
                        break;

                    case Direction.Down:
                        newPosition = new PlayerCoords(initialCoordX, initialCoordY - i);
                        break;

                    case Direction.Left:
                        newPosition = new PlayerCoords(initialCoordX + i, initialCoordY);
                        break;

                    case Direction.Right:
                        newPosition = new PlayerCoords(initialCoordX - i, initialCoordY);
                        break;
                }

                trail.InsertLast(newPosition);  // Add the new position to the trail
            }
        }

        // Change the player's direction if Valid. 
        public void ChangeDirection(Direction newDirection)
        {
            if (newDirection != playerDirection && IsValidDirectionChange(newDirection))
            {
                playerDirection = newDirection;
            }
        }
        private bool IsValidDirectionChange(Direction newDirection)
        {
            return (playerDirection == Direction.Up && newDirection != Direction.Down) ||
                   (playerDirection == Direction.Down && newDirection != Direction.Up) ||
                   (playerDirection == Direction.Left && newDirection != Direction.Right) ||
                   (playerDirection == Direction.Right && newDirection != Direction.Left);
        }

        // Insert a new head to the player's trail. 
        public PlayerCoords InsertNewHead(PlayerCoords newHeadCoords, bool removeLast)
        {
            trail.InsertFirst(newHeadCoords);  // Insert new head at the start of the trail.
            playerPosition = newHeadCoords;  // Update player position.

            if (removeLast)
            {
                // Remove the last element of the trail and return its data.
                return trail.RemoveLast().Data;
            }
            else
            {
                playerSize++;  // Increase player size.
                return newHeadCoords;
            }
        }

        // Collect an item and handle any related actions. 
        public void CollectItem(InGameObj item)
        {
            int priority = item == InGameObj.Energy ? 0 : 1;

            if (itemsCollected.IsEmpty()) useItemTimer = DateTime.Now;

            itemsCollected.Push(item);  // Add the item to the stack. 
        }

        // Handle the effects of collected items. 
        public void HandleItem(InGameObj item)
        {
            switch (item)
            {
                case InGameObj.Energy:
                    // Increase energy, but not above 500 -Max Energy-. 
                    if (playerEnergy < 500) playerEnergy += randomValue.Next(1, 500 - playerEnergy);
                    break;

                case InGameObj.HyperVelocity:
                    // Increase speed, but not above 5 -Max Speed-. 
                    if (playerSpeed < 5) playerSpeed += randomValue.Next(1, 5 - playerSpeed);
                    hyperSpeedTimer = DateTime.Now;  // Start hyper speed timer. 
                    break;

                case InGameObj.NewJet:
                    // Add a new segment to the player's trail. 
                    PlayerCoords setCoords = new PlayerCoords(playerPosition.posX, playerPosition.posY);
                    InsertNewHead(setCoords, false); // New head without removing last trail. 
                    break;

                case InGameObj.Shield:
                    // Activate the shield and set invincibility timer. 
                    playerInvincible = true;
                    invincibilityTimer = DateTime.Now;
                    break;
            }
        }

        // Handle player death. 
        public void Death()
        {
            playerAlive = false;  // Set player status to dead
            playerEnergy = 0;
            playerSpeed = 0; 
            trail.Destroy();  
        }
    }
}
