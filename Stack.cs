using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    class Stack<T>
    {
        private Node<T> head;
        public Stack()
        {
            this.head = null;
        }
        public void Push(T x)
        {
            Node<T> temp = new Node<T>(x);
            temp.SetNext(head);
            head = temp;
        }
        public T Pop()
        {
            T x = head.GetValue();
            head = head.GetNext();
            return x;
        }
        public T Top()
        {
            return head.GetValue();
        }
        public bool IsEmpty()
        {
            return head == null;
        }
        public string ToString()
        {
            if (this.IsEmpty()) return "[]";
            Stack<T> temp = new Stack<T>();
            while (!this.IsEmpty())
                temp.Push(this.Pop());
            string s = "[";
            while (!temp.IsEmpty())
            {
                s = s + temp.Top() + ',';
                this.Push(temp.Pop());
            }
            s = s.Substring(0, s.Length - 1) + "]";
            return s;
        }
    }
}
