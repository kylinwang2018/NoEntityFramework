using System.Collections;
using System.Collections.Generic;

namespace NoEntityFramework.Collections
{
    public class OrderedSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        public OrderedSet() : this(EqualityComparer<T>.Default)
        {
        }
        public OrderedSet(IEqualityComparer<T> comparer)
        {
            this.m_Dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            this.m_LinkedList = new LinkedList<T>();
        }
        public OrderedSet(T[] items) : this(EqualityComparer<T>.Default)
        {
            for (int i = 0; i < items.Length; i++)
            {
                this.Add(items[i]);
            }
        }
        public OrderedSet(IEnumerable<T> items) : this(EqualityComparer<T>.Default)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }
        public OrderedSet(T[] items, IEqualityComparer<T> comparer)
        {
            this.m_Dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            this.m_LinkedList = new LinkedList<T>();
            for (int i = 0; i < items.Length; i++)
            {
                this.Add(items[i]);
            }
        }
        public OrderedSet(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            this.m_Dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            this.m_LinkedList = new LinkedList<T>();
            foreach (T item in items)
            {
                this.Add(item);
            }
        }
        public int Count
        {
            get
            {
                return this.m_Dictionary.Count;
            }
        }
        public virtual bool IsReadOnly
        {
            get
            {
                return this.m_Dictionary.IsReadOnly;
            }
        }
        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public int AddRange(IEnumerable<T> items)
        {
            int num = 0;
            foreach (T item in items)
            {
                bool flag = this.Add(item);
                if (flag)
                {
                    num++;
                }
            }
            return num;
        }
        public void Clear()
        {
            this.m_LinkedList.Clear();
            this.m_Dictionary.Clear();
        }
        public bool Remove(T item)
        {
            if (!m_Dictionary.TryGetValue(item, out LinkedListNode<T> node))
            {
                return false;
            }
            else
            {
                this.m_Dictionary.Remove(item);
                this.m_LinkedList.Remove(node);
                return true;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return this.m_LinkedList.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public bool Contains(T item)
        {
            return this.m_Dictionary.ContainsKey(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.m_LinkedList.CopyTo(array, arrayIndex);
        }
        public bool Add(T item)
        {
            if (m_Dictionary.ContainsKey(item))
            {
                return false;
            }
            else
            {
                var value = this.m_LinkedList.AddLast(item);
                this.m_Dictionary.Add(item, value);
                return true;
            }
        }

        private readonly IDictionary<T, LinkedListNode<T>> m_Dictionary;
        private readonly LinkedList<T> m_LinkedList;
    }
}
