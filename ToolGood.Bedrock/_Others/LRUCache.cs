using System.Collections.Generic;
using System.Threading;

namespace ToolGood.Bedrock._Others
{
    public class LRUCache<TKey, TValue>
    {
        private readonly DoubleLinkedListNode<TKey, TValue> _head;
        private readonly DoubleLinkedListNode<TKey, TValue> _tail;
        private readonly Dictionary<TKey, DoubleLinkedListNode<TKey, TValue>> _dictionary;
        private readonly ReaderWriterLockSlim _slimLock = new ReaderWriterLockSlim();

        private readonly int _capacity;
        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _head = new DoubleLinkedListNode<TKey, TValue>();
            _tail = new DoubleLinkedListNode<TKey, TValue>();
            _head.Next = _tail;
            _tail.Previous = _head;
            _dictionary = new Dictionary<TKey, DoubleLinkedListNode<TKey, TValue>>();
        }

        public TValue Get(TKey key)
        {
            _slimLock.EnterUpgradeableReadLock();
            try {
                if (_dictionary.TryGetValue(key, out var node)) {
                    if (_tail != node.Next) {
                        _slimLock.EnterWriteLock();
                        RemoveNode(node);
                        AddLastNode(node);
                        _slimLock.ExitWriteLock();
                    }
                    return node.Value;
                }
            } finally {
                _slimLock.ExitUpgradeableReadLock();
            }
            return default;
        }
        public void Put(TKey key, TValue value)
        {
            _slimLock.EnterWriteLock();
            try {
                if (_dictionary.TryGetValue(key, out var node)) {
                    if (_tail != node.Next) {
                        RemoveNode(node);
                        AddLastNode(node);
                    }
                    node.Value = value;
                } else {
                    if (_dictionary.Count >= _capacity) {  
                        var firstNode = RemoveFirstNode();
                        _dictionary.Remove(firstNode.Key);
                    }
                    var newNode = new DoubleLinkedListNode<TKey, TValue>(key, value);
                    AddLastNode(newNode);
                    _dictionary.TryAdd(key, newNode);
                }
            } finally {
                _slimLock.ExitWriteLock();
            }
        }
        public void Remove(TKey key)
        {
            _slimLock.EnterWriteLock();
            try {
                if (_dictionary.Remove(key, out var node)) {
                    RemoveNode(node);
                }
            } finally {
                _slimLock.ExitWriteLock();
            }
        }
        private void AddLastNode(DoubleLinkedListNode<TKey, TValue> node)
        {
            node.Previous = _tail.Previous;
            node.Next = _tail;
            _tail.Previous.Next = node;
            _tail.Previous = node;
        }
        private DoubleLinkedListNode<TKey, TValue> RemoveFirstNode()
        {
            var firstNode = _head.Next;
            _head.Next = firstNode.Next;
            firstNode.Next.Previous = _head;
            firstNode.Next = null;
            firstNode.Previous = null;
            return firstNode;
        }
        private void RemoveNode(DoubleLinkedListNode<TKey, TValue> node)
        {
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
            node.Next = null;
            node.Previous = null;
        }
        internal class DoubleLinkedListNode<TKey, TValue>
        {
            public DoubleLinkedListNode() { }
            public DoubleLinkedListNode(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
            public TKey Key { get; private set; }
            public TValue Value { get; set; }
            public DoubleLinkedListNode<TKey, TValue> Previous { get; set; }
            public DoubleLinkedListNode<TKey, TValue> Next { get; set; }
        }
    }

}
