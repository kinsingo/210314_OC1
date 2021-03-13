using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PNC_Csharp.Practices
{
    class PriorityQueue<T>
    {
        class Node
        {
            public T data;
            public int priority;
            public static bool operator <(Node n1, Node n2)
            {
                return (n1.priority < n2.priority);
            }
            public static bool operator >(Node n1, Node n2)
            {
                return (n1.priority > n2.priority);
            }
            public static bool operator <=(Node n1, Node n2)
            {
                return (n1.priority <= n2.priority);
            }
            public static bool operator >=(Node n1, Node n2)
            {
                return (n1.priority >= n2.priority);
            }
            public static bool operator ==(Node n1, Node n2)
            {
                return (n1.priority == n2.priority);
            }
            public static bool operator !=(Node n1, Node n2)
            {
                return (n1.priority != n2.priority);
            }

            public Node(T data, int priority)
            {
                this.data = data;
                this.priority = priority;
            }
        }

        Node[] nodeArr;
        int count;
        int arrSize;
        public PriorityQueue(int arrSize)
        {
            this.arrSize = arrSize;
            nodeArr = new Node[this.arrSize];
            count = 0;
        }

        public void Enqueue(T data, int priority)
        {
            Node newNode = new Node(data, priority);
            if (count == nodeArr.Length)
            {
                throw new Exception("The PriorityQueue is at full capacity");
            }
            else
            {
                nodeArr[count] = newNode;
                siftUp(count);
                count++;
            }
        }

        private void siftUp(int index)
        {
            if (index != 0)
            {
                int ParentIndex = getParentIndex(index);
                if (nodeArr[ParentIndex] > nodeArr[index])
                {
                    Swap(ref nodeArr[ParentIndex], ref nodeArr[index]);
                    siftUp(ParentIndex);
                }
            }
        }

        public void decrease_priority(T target, int newPriority)
        {
            Node newNode = new Node(target, newPriority);
        }



        private int getParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private int GetLeftChildIndex(int index)
        {
            return (2 * index) + 1;
        }

        private int GetRightChildIndex(int index)
        {
            return (2 * index) + 2;
        }

        private void Swap(ref Node A, ref Node B)
        {
            Node temp = A;
            A = B;
            B = temp;
        }
        
    }
}
