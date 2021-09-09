using System;
using System.Collections;
using System.Collections.Generic;

namespace CyberTale.Collections
{
    public class SinglyList<T>
    {
        public int Count
        {
            get
            {
                int elements = 0;
                Node count = head;
                while (count != null)
                {
                    count = count.next;
                    elements++;
                }
                return elements;
            }
        }
        public sealed class Node
        {
            // node variables  
            public T data;
            public Node next;

            public Node(T data)
            {
                this.data = data;
                this.next = null;
            }
        }

        // create reference variable of Node  
        public Node head;
        public Node tail;
        // function to insert a node  
        // at the beginning of the list  
        public void InsertAtStart(T data)
        {
            // create a node  
            Node new_node = new Node(data);
            if (head == null)
            {
                head = new_node;
                tail = new_node;
                return;
            }
            new_node.next = head;
            head = new_node;
        }

        // function to insert node  
        // at the end of the list  
        public void EnList(T data)
        {
            Node new_node = new Node(data);
            if (head == null)
            {
                head = new_node;
                tail = new_node;
                return;
            }

            new_node.next = null;
            Node last = tail;
            last.next = new_node;
            tail = new_node;
        }

        // function to delete a node  
        // at the beginning of the list  
        public void DeleteFirst()
        {
            if (head == null)
            {
                Console.WriteLine("List is empty");
                return;
            }
            head = head.next;
        }

        // function to delete a node  
        // from the end of the list  
        public void DeleteLast()
        {
            Node delete = head;
            while (delete.next != null
                && delete.next.next != null)
            {
                delete = delete.next;
            }
            delete.next = null;
            tail = delete;
        }

        public void DeleteAll()
        {
            head = null;
            tail = null;
        }

        // function to display all the nodes of the list  
        public void Display()
        {
            Node disp = head;
            while (disp != null)
            {
                UnityEngine.Debug.Log(disp.data + "->");
                disp = disp.next;
            }
        }
        public T DeList()
        {
            var temp = head.data;
            DeleteFirst();
            return temp;
        }
        public void PrependList(SinglyList<T> queue)
        {
            queue.tail.next = this.head;
            this.head = queue.head;
        }
    }
}

