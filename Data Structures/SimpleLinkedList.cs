public class SimpleLinkedList<T>
{
    private Node<T> _head;
    private Node<T> _tail;
    private int _count;

    public SimpleLinkedList()
    {
        _head = null;
        _tail = null;
        _count = 0;
    }

    public void InsertFirst(T data)
    {
        Node<T> newNode = new Node<T>(data);
        if (_head == null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            newNode.Next = _head;
            _head = newNode;
        }
        _count++;
    }

    public void InsertLast(T data)
    {
        Node<T> newNode = new Node<T>(data);
        if (_tail == null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            _tail.Next = newNode;
            _tail = newNode;
        }
        _count++;
    }

    public Node<T> RemoveFirst()
    {
        if (_head == null) return null;

        Node<T> removedNode = _head;
        _head = _head.Next;

        if (_head == null)
        {
            _tail = null;
        }

        _count--;
        return removedNode;
    }

    public Node<T> RemoveLast()
    {
        if (_tail == null) return null;

        if (_head == _tail)
        {
            Node<T> removedNode = _tail;
            _head = null;
            _tail = null;
            _count--;
            return removedNode;
        }

        Node<T> current = _head;
        while (current.Next != _tail)
        {
            current = current.Next;
        }

        Node<T> removedNodeLast = _tail;
        _tail = current;
        _tail.Next = null;

        _count--;
        return removedNodeLast;
    }

    public Node<T> GetFirst() => _head;

    public Node<T> GetLast() => _tail;

    public int Length() => _count;

    public bool IsEmpty() => _count == 0;
}

public class Node<T>
{
    public T Data { get; set; }
    public Node<T> Next { get; set; }

    public Node(T data)
    {
        Data = data;
        Next = null;
    }
}
