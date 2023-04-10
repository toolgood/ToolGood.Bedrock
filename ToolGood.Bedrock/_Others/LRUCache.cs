using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock._Others
{
    public class LRUCache<TKey, TValue>
    {
        private readonly DoubleLinkedListNode<TKey, TValue> _head;
        private readonly DoubleLinkedListNode<TKey, TValue> _tail;
        private readonly ConcurrentDictionary<TKey, DoubleLinkedListNode<TKey, TValue>> _dictionary;
        private readonly int _capacity;
        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _head = new DoubleLinkedListNode<TKey, TValue>();

            _tail = new DoubleLinkedListNode<TKey, TValue>();
            _head.Next = _tail;
            _tail.Previous = _head;
            _dictionary = new ConcurrentDictionary<TKey, DoubleLinkedListNode<TKey, TValue>>();
        }
        public TValue this[TKey key] { get { return Get(key); } set { Set(key, value); } }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public TValue Get(TKey key)
        {

            if (_dictionary.TryGetValue(key, out var node)) {
                RemoveNode(node);
                AddLastNode(node);
                return node.Value;
            }
            return default;
        }
        public void Set(TKey key, TValue value)
        {
            if (_dictionary.TryGetValue(key, out var node)) {
                RemoveNode(node);
                AddLastNode(node);
                node.Value = value;
            } else {
                if (_dictionary.Count >= _capacity) {
                    var firstNode = RemoveFirstNode();
                    _dictionary.Remove(firstNode.Key, out _);
                }
                var newNode = new DoubleLinkedListNode<TKey, TValue>(key, value);
                AddLastNode(newNode);
                _dictionary.TryAdd(key, newNode);
            }
        }
        public void Remove(TKey key)
        {
            if (_dictionary.Remove(key, out var node)) {
                RemoveNode(node);
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
