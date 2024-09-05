
namespace TronGame.Data_Structures
{
    public class PriorityStack<T>
    {
        private SimpleLinkedList<PriorityElement<T>> items;

        // Constructor
        public PriorityStack()
        {
            items = new SimpleLinkedList<PriorityElement<T>>();
        }

        // Push an item onto the stack with a specified priority
        public void Push(T item, int priority)
        {
            var newElement = new PriorityElement<T>(item, priority);
            if (items.IsEmpty())
            {
                items.InsertFirst(newElement);
            }
            else
            {
                Node<PriorityElement<T>> current = items.GetFirst();
                Node<PriorityElement<T>> previous = null;

                // Traverse the list to find the appropriate place to insert the new element
                while (current != null && current.Data.Priority >= priority)
                {
                    previous = current;
                    current = current.Next;
                }

                if (previous == null)
                {
                    // Insert at the beginning
                    items.InsertFirst(newElement);
                }
                else if (current == null)
                {
                    // Insert at the end
                    items.InsertLast(newElement);
                }
                else
                {
                    // Insert in the middle
                    Node<PriorityElement<T>> newNode = new Node<PriorityElement<T>>(newElement);
                    newNode.Next = current;
                    previous.Next = newNode;
                }
            }
        }

        // Pop the top item from the stack
        public T Pop()
        {
            if (items.IsEmpty())
                throw new InvalidOperationException("The stack is empty.");

            return items.RemoveFirst().Data.Data;
        }

        // Peek at the top item without removing it
        public T Peek()
        {
            if (items.IsEmpty())
                throw new InvalidOperationException("The stack is empty.");

            return items.GetFirst().Data.Data;
        }

        // Check if the stack is empty
        public bool IsEmpty()
        {
            return items.IsEmpty();
        }

        // Get the number of items in the stack
        public int Count()
        {
            return items.Length();
        }

        // Clear the stack
        public void Clear()
        {
            while (!items.IsEmpty())
            {
                items.RemoveFirst();
            }
        }

        // Set the element with a specific priority as the new head
        public void SetElementAsHead(int index)
        {
            if (index < 0 || index >= items.Length())
                throw new IndexOutOfRangeException("Index out of range.");

            if (index == 0)
                return; // Already the head, nothing to do

            Node<PriorityElement<T>> previousNode = null;
            Node<PriorityElement<T>> currentNode = items.GetFirst();
            Node<PriorityElement<T>> newHeadNode = null;

            int currentIndex = 0;
            while (currentNode != null)
            {
                if (currentIndex == index)
                {
                    newHeadNode = currentNode;
                    break;
                }
                previousNode = currentNode;
                currentNode = currentNode.Next;
                currentIndex++;
            }

            if (newHeadNode != null && previousNode != null)
            {
                // Detach the node from its current position
                previousNode.Next = newHeadNode.Next;

                // Insert the node as the new head
                newHeadNode.Next = items.GetFirst();
                items.InsertFirst(newHeadNode.Data);
            }
        }
    }

    public class PriorityElement<T>
    {
        public T Data { get; set; }
        public int Priority { get; set; }

        public PriorityElement(T data, int priority)
        {
            Data = data;
            Priority = priority;
        }
    }
}
