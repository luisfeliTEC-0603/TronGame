
using TronGame.Game_Logic;

namespace TronGame.Data_Structures
{
    public class itemStack
    {
        public SimpleLinkedList<InGameObj> items; // Linked list to hold stack elements. 

        public itemStack() // Constructor.
        {
            items = new SimpleLinkedList<InGameObj>();
        }

        // Pushes a new item onto the stack
        public void Push(InGameObj item)
        {
            items.InsertFirst(item);
            EnergyFirst();
        }

        // Pops the top item off the stack. 
        public InGameObj Pop()
        {
            if (items.IsEmpty())
                throw new InvalidOperationException("The stack is empty.");

            return items.RemoveFirst().Data;
        }

        // Peeks at the top item without removing it. 
        public InGameObj Peek()
        {
            if (items.IsEmpty())
                throw new InvalidOperationException("The stack is empty.");

            return items.GetFirst().Data;
        }

        // Checks if the stack is empty
        public bool IsEmpty() => items.IsEmpty();

        // Clears all items from the stack. 
        public void Clear()
        {
            while (!items.IsEmpty())
            {
                items.RemoveFirst(); // Remove items one by one. 
            }
        }

        // Sets the node at the given index as the new head of the stack. 
        public void SetElementAsHead(int index)
        {
            if (index <= 0 || index >= items.Length()) return; // Validate index. 

            Node<InGameObj> previousNode = null;
            Node<InGameObj> currentNode = items.GetFirst();
            Node<InGameObj> newHeadNode = null;

            int currentIndex = 0;

            // Iterates through the list to find the node at the specified index. 
            while (currentNode != null)
            {
                if (currentIndex == index)
                {
                    newHeadNode = currentNode; // Node to be moved to the head. 
                    break;
                }
                previousNode = currentNode;
                currentNode = currentNode.Next;
                currentIndex++;
            }

            // Adjusts the linked list. 
            if (newHeadNode != null)
            {
                if (previousNode != null)
                {
                    previousNode.Next = newHeadNode.Next;
                }
                else
                {
                    items.RemoveFirst();  // Handle moving the head node.
                }

                newHeadNode.Next = items.GetFirst();
                items.InsertFirst(newHeadNode.Data);
            }
        }

        // Prioritize Energy consumtion. 
        public void EnergyFirst() 
        {
            Node<InGameObj> previousNode = null;
            Node<InGameObj> currentNode = items.GetFirst();

            while (currentNode != null) // Search for energy in the stack.
            {
                if (currentNode.Data == InGameObj.Energy) 
                {
                    if (previousNode != null)
                    {   
                        // If the energy is not the head, makes current node the head. 
                        previousNode.Next = currentNode.Next;
                        currentNode.Next = items.GetFirst();
                        items.InsertFirst(currentNode.Data);
                    }
                    break;
                }

                // Moves in the list. 
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }
        }
    }
}