
using TronGame.Data_Structures;
using static TronGame.TronGame;

namespace TronGame.Game_Logic
{
    public class Player
    {
        public bool playerAlive {  get; set; }

        public struct PlayerCoords
        {
            public int posX {  get; set; }  
            public int posY {  get; set; }

            public PlayerCoords(int x, int y)
            {
                posX = x;
                posY = y;
            }
        }

        public PlayerCoords playerPosition { get; set; }
        public int playerEnergy { get; set; }
        public int playerSize {  get; set; }
        public int playerSpeed {  get; set; }
        public bool playerInvincible { get; set; }
        public SimpleLinkedList<PlayerCoords> trail {  get; set; }
        public PriorityStack<InGameObj> itemsCollected { get; set; }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
        public Direction playerDirection { get; private set; }

        Random randomValue;
        private DateTime invincibilityTimer;

        public Player(int initialCoordX, int initialCoordY, int initialSize, int initialSpeed, Direction initialDirection) 
        {
            playerAlive = true;
            playerInvincible = false;

            playerEnergy = 100;
            playerSize = initialSize;
            playerSpeed = initialSpeed;
            playerDirection = initialDirection;

            playerPosition = new PlayerCoords(initialCoordX, initialCoordY);
            trail = new SimpleLinkedList<PlayerCoords>();
            itemsCollected = new PriorityStack<InGameObj>();

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
                
                trail.InsertLast(newPosition);
            }
        }

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

        public PlayerCoords InsertNewHead(PlayerCoords newHeadCoords, bool removeLast)
        {
            trail.InsertFirst(newHeadCoords);
            playerPosition = newHeadCoords;

            if (removeLast)
            {
                return trail.RemoveLast().Data;
            }
            else
            {
                playerSize++;
                return newHeadCoords;
            }
        }

        public void CollectItem(InGameObj item)
        {
            int priority = 1;
            if (item == InGameObj.Energy) priority = 0;

            itemsCollected.Push(item, priority);
        }

        public InGameObj GetLastCollectedItem()
        {
            return !itemsCollected.IsEmpty() ? itemsCollected.Pop() : default;
        }

        public void HandleItem(InGameObj item)
        {
            switch (item)
            {
                case InGameObj.Energy:
                    if (playerEnergy < 100) playerEnergy += randomValue.Next(1, 100 - playerEnergy);
                    break;

                case InGameObj.HyperVelocity:
                    if (playerSize < 5) playerSpeed += randomValue.Next(1, 5 - playerSpeed);
                    break;

                case InGameObj.NewJet:
                    PlayerCoords setCoords = new PlayerCoords(playerPosition.posX, playerPosition.posY);
                    InsertNewHead(setCoords, false);
                    break;

                case InGameObj.Shield:
                    playerInvincible = true;
                    break;
            }
        }
    }
}
