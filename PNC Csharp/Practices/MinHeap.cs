using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PNC_Csharp.Practices
{
    class MinHeap<T>
    {
        Form1 f1 = (Form1)Application.OpenForms["Form1"];

        T[] arr;
        int arrSize;//size for the array container
        int Count;//keeps track of the number of elements;
       
        
        public MinHeap(int size)
        {
            SetHeapSize(size);
            Count = 0;
        }

        public void SetHeapSize(int size)
        {
            arrSize = size;
            arr = new T[arrSize];
        }

        public void Insert(T value)
        {
            if (Count < arrSize)
            {
                int index = Count;
                
                arr[index] = value;
                siftUp(index);

                Count++;
            }
            else
            {
                throw new Exception("Heap is at full capacity");
            }
        }

        public void Remove(T value)
        {
            for (int index = 0; index < (Count - 1); index++)
            {
                if (((dynamic)arr[index] == (dynamic)value))
                {
                    arr[index] = arr[(Count - 1)];
                    Count--;
                    siftDown(index);
                    break;
                }
            }
        }

        public void RemoveMin()
        {
            if (Count == 0)
            {
                throw new Exception("Heap is empty !");
            }
            else
            {
                arr[0] = arr[Count - 1];
                Count--;
                if (Count > 0)
                {
                    siftDown(0);
                }
            }
        }

        public void displayHeap()
        {
            f1.GB_Status_AppendText_Nextline("--Elements of the MinHeap--", Color.DarkBlue);
            StringBuilder temp = new StringBuilder();

            for(int index = 0;index < Count;index++)
              temp.Append(" " + arr[index]);

            f1.GB_Status_AppendText_Nextline(temp.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("---------------------------", Color.DarkBlue);
        }

        public T getMin()
        {
            return arr[0];
        }

        public void BuildMinHeap(T[] input)
        {
            ClearAndResizeHeap(input);
            for (int index = (Count - 1)/2 ; index >= 0; index--)
            {
                MinHeapify(index);
            }
        }

        private void MinHeapify(int index)
        {
            int leftChildIndex = GetLeftChildIndex(index); //GetLeftChildIndex(index) - 1;//??
            int rightChildIndex = GetRightChildIndex(index); //GetRightChildIndex(index) - 1;//??
            int smallestValueIndex = index;

            if(IsNoChild(leftChildIndex, rightChildIndex))
            {
                return;
            }
            else if (IsOnlyLeftChild(leftChildIndex, rightChildIndex))
            {
                if ((dynamic)arr[leftChildIndex] < (dynamic)arr[index])
                   smallestValueIndex = leftChildIndex;
            }
            else  //if it has both left and right child
            {
                if ((dynamic)arr[leftChildIndex] < (dynamic)arr[smallestValueIndex])
                    smallestValueIndex = leftChildIndex;
                if ((dynamic)arr[rightChildIndex] < (dynamic)arr[smallestValueIndex])
                    smallestValueIndex = rightChildIndex;

                if (smallestValueIndex != index)
                {
                    swap(arr, index, smallestValueIndex);
                    MinHeapify(smallestValueIndex);
                }
            }
        }
        

        private void swap(T[] inputArr, int a, int b)
        {
            T temp = inputArr[a];
            inputArr[a] = inputArr[b];
            inputArr[b] = temp;
        }

        private void ClearAndResizeHeap(T[] inputArr)
        {
            Array.Resize(ref arr, inputArr.Length);
            Count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = inputArr[i];
                Count++;
            }
        }

        private void siftDown(int index)
        {
            int leftChildIndex = GetLeftChildIndex(index);
            int rightChildIndex = GetRightChildIndex(index);

            if (IsNoChild(leftChildIndex, rightChildIndex))
            {
                return;
            }
            else
            {
                int minValueChildIndex = GetMinValueChildIndex(leftChildIndex, rightChildIndex);
                if ((dynamic)arr[index] > arr[minValueChildIndex])
                {
                    Swap(ref arr[index], ref arr[minValueChildIndex]);
                    siftDown(minValueChildIndex);
                }
            }
        }

        private int GetMinValueChildIndex(int leftChildIndex, int rightChildIndex)
        {
            int minValueChildIndex;

            if (IsOnlyLeftChild(leftChildIndex, rightChildIndex))
            {
                minValueChildIndex = leftChildIndex;
            }
            else  //if it has both left and right child
            {
                if ((dynamic)arr[leftChildIndex] <= (dynamic)arr[rightChildIndex])
                    minValueChildIndex = leftChildIndex;
                else
                    minValueChildIndex = rightChildIndex;
            }

            return minValueChildIndex;
        }

        private bool IsNoChild(int leftChildIndex, int rightChildIndex)
        {
            return ((rightChildIndex >= Count) && (leftChildIndex >= Count)) ;
        }

        private bool IsOnlyLeftChild(int leftChildIndex, int rightChildIndex)
        {
            return ((rightChildIndex >= Count) && (leftChildIndex < Count));
        }

        private int GetLeftChildIndex(int index)
        {
            return (2 * index) + 1;
        }

        private int GetRightChildIndex(int index)
        {
            return (2 * index) + 2;
        }

        private void siftUp(int index)
        {
            if (index != 0)
            {
                int parentIndex = getParentIndex(index);
                if ((dynamic)arr[parentIndex] > (dynamic)arr[index])
                {
                    Swap(ref arr[parentIndex], ref arr[index]);
                    siftUp(parentIndex);
                }
            }
        }

        private void Swap(ref T A,ref T B)
        {
            T temp = A;
            A = B;
            B = temp;
        }

        private int getParentIndex(int arrIndex)
        {
            return (arrIndex - 1) / 2;
        }

        public void deleteHeap()
        {
            Array.Resize(ref arr, 0);
            Count = 0;
        }
    }
     
}
