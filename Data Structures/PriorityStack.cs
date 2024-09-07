
namespace TronGame.Data_Structures
{
    public class PriorityStack<T>
    {
        private SimpleLinkedList<PriorityElement<T>> items;

        public PriorityStack()
        {
            items = new SimpleLinkedList<PriorityElement<T>>();
        }

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

                while (current != null && current.Data.Priority >= priority)
                {
                    previous = current;
                    current = current.Next;
                }

                if (previous == null)
                {
                    items.InsertFirst(newElement);
                }
                else if (current == null)
                {
                    items.InsertLast(newElement);
                }
                else
                {
                    Node<PriorityElement<T>> newNode = new Node<PriorityElement<T>>(newElement);
                    newNode.Next = current;
                    previous.Next = newNode;
                }
            }
        }

        public T Pop()
        {
            if (items.IsEmpty())
                throw new InvalidOperationException("The stack is empty.");

            return items.RemoveFirst().Data.Data;
        }

        public T Peek()
        {
            if (items.IsEmpty())
                throw new InvalidOperationException("The stack is empty.");

            return items.GetFirst().Data.Data;
        }

        public bool IsEmpty()
        {
            return items.IsEmpty();
        }

        public int Count()
        {
            return items.Length();
        }

        public void Clear()
        {
            while (!items.IsEmpty())
            {
                items.RemoveFirst();
            }
        }

        public void SetElementAsHead(int index)
        {
            if (index <= 0 || index >= items.Length()) return;

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
                previousNode.Next = newHeadNode.Next;

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
