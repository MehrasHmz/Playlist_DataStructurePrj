using System;
using System.Collections;
using System.Collections.Generic;

namespace Playlist2
{
    public class LinkedList<T> : IEnumerable<T>
    {
        private Node<T>? head;

        public LinkedList() => head = null;

        public Node<T>? GetHead() => head;

        public void AddLast(T data)
        {
            var n = new Node<T>(data);
            if (head == null)
            {
                head = n;
                return;
            }

            var cur = head;
            while (cur.Next != null) cur = cur.Next;
            cur.Next = n;
        }

        public void AddFirst(T data)
        {
            var n = new Node<T>(data) { Next = head };
            head = n;
        }

        public bool Remove(Predicate<T> predicate)
        {
            if (head == null) return false;
            if (predicate(head.Data))
            {
                head = head.Next;
                return true;
            }

            var prev = head;
            var cur = head.Next;
            while (cur != null)
            {
                if (predicate(cur.Data))
                {
                    prev.Next = cur.Next;
                    return true;
                }

                prev = cur;
                cur = cur.Next;
            }

            return false;
        }

        public int Count()
        {
            int c = 0;
            var cur = head;
            while (cur != null)
            {
                c++;
                cur = cur.Next;
            }

            return c;
        }

        public List<T> ToList()
        {
            var list = new List<T>();
            var cur = head;
            while (cur != null)
            {
                list.Add(cur.Data);
                cur = cur.Next;
            }

            return list;
        }

        public void ReplaceAll(IEnumerable<T> items)
        {
            head = null;
            foreach (var it in items) AddLast(it);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var cur = head;
            while (cur != null)
            {
                yield return cur.Data;
                cur = cur.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}