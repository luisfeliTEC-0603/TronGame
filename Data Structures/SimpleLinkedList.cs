namespace TronGame.Data_Structures
{
    public class SimpleLinkedList<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public SimpleLinkedList() // Constructor. 
        {
            head = null;
            tail = null;
            count = 0;
        }

        // Insert at the beginning.
        public void InsertFirst(T data)
        {
            Node<T> newNode = new Node<T>(data); // Create a new node with the provided data.

            if (IsEmpty()) // Check if the list is empty.
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                // Add the element as new head. 
                newNode.Next = head;
                head = newNode;
            }
            count++;
        }

        // Insert at the end.
        public void InsertLast(T data) 
        {
            Node<T> newNode = new Node<T>(data); // Create a new node with the provided data.

            if (IsEmpty()) // Check if the list is empty. 
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                // Add the element as new tail.
                tail.Next = newNode;
                tail = newNode;
            }
            count++;
        }

        // Remove and return the first node.
        public Node<T> RemoveFirst()
        {
            if (IsEmpty()) return null; // Check if the list is empty. 

            Node<T> removedNode = head;
            head = head.Next; // Update the head. 

            if (head == null) tail = null; // Both head and tail are null, if the list is now empty.
            count--;

            return removedNode;
        }

        // Remove and return the last node.
        public Node<T> RemoveLast()
        {
            if (IsEmpty()) return null; // Check if the list is empty. 

            if (head == tail) // Only one node in the list. 
            {
                // List becomes empty and return removed node. 
                Node<T> nodeToRemove = tail; 
                head = null;
                tail = null;
                count--;
                return nodeToRemove;
            }

            Node<T> current = head;
            while (current.Next != tail) // Traverse to the node before the tail. 
            {
                current = current.Next;
            }

            // Updates tail and remove previous tail. 
            Node<T> nodeToRemoveLast = tail;
            tail = current;
            tail.Next = null;

            count--;
            return nodeToRemoveLast;
        }

        // Get the first node
        public Node<T> GetFirst() => head;

        // Get the last node
        public Node<T> GetLast() => tail;

        // Get the number of nodes
        public int Length() => count;

        // Check if the list is empty
        public bool IsEmpty() => count == 0 || head == null;

        // Clear the list
        public void Destroy()
        {
            head = null;
            tail = null;
            count = 0;
        }
    }

    public class Node<T>
    {
        public T Data { get; set; } // Data stored. 
        public Node<T> Next { get; set; } // Reference to the next node. 

        public Node(T data) // Constructor. 
        {
            Data = data;
            Next = null;
        }
    }
}