using System.Collections;
using System.Collections.Generic;

namespace Sop.Core.Miscellaneous
{
    /// <summary>
    /// 按插入顺序排序的HashSet，类似于Java里的LinkedHashSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedHashSet<T> : ICollection<T>
    {
        private readonly IDictionary<T, LinkedListNode<T>> _mDictionary;
        private readonly LinkedList<T> _mLinkedList;

        public LinkedHashSet()
            : this(EqualityComparer<T>.Default)
        {
        }

        public LinkedHashSet(IEqualityComparer<T> comparer)
        {
            _mDictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            _mLinkedList = new LinkedList<T>();
        }

        public int Count
        {
            get
            {
                 return _mDictionary.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get { return _mDictionary.IsReadOnly; }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            _mLinkedList.Clear();
            _mDictionary.Clear();
        }

        public bool Remove(T item)
        {
            LinkedListNode<T> node;
            bool found = _mDictionary.TryGetValue(item, out node);
            if (!found) return false;
            _mDictionary.Remove(item);
            _mLinkedList.Remove(node);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _mLinkedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            return _mDictionary.ContainsKey(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _mLinkedList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(T item)
        {
            if (_mDictionary.ContainsKey(item)) return false;
            var node = _mLinkedList.AddLast(item);
            _mDictionary.Add(item, node);
            return true;
        }
    }
}