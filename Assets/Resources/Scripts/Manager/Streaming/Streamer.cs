using System;
using System.Collections.Generic;
using UnityEngine;
using CyberTale.Collections;

namespace Manager.Streaming
{
    public class Streamer<T> 
    {
        public delegate void HandleMethod(T interactableObject);
        public delegate void EndMethod();
        readonly HandleMethod HandleMethodDelegate;
        readonly EndMethod EndMethodDelegate;

        private SinglyList<T> Stream = new SinglyList<T>();

        private bool StreamEnded { get; set; } = true;

        public Streamer(HandleMethod _handleMethod)
        {
            HandleMethodDelegate = _handleMethod;
        }

        public Streamer(HandleMethod _handleMethod, EndMethod _endMethod)
        {
            HandleMethodDelegate = _handleMethod;
            EndMethodDelegate = _endMethod;
        }

        public void StreamEnqueue(T sender, bool overrideQueue = false)
        {
            //Debug.Log("Stream Enqueued" + DateTime.Now.Millisecond);
            if (overrideQueue)
                Stream.DeleteAll();
            Stream.EnList(sender);
            if (StreamEnded || overrideQueue)
            {
                StreamEnded = false;
                StreamNext();
            }
        }

        public void StreamNext(SinglyList<T> queue = null)
        {
            if (queue != null)
                Stream.PrependList(queue);
            if (Stream.Count > 0)
                HandleMethodDelegate(Stream.DeList());
            else
            {
                StreamEnded = true;
                if (EndMethodDelegate != null)
                    EndMethodDelegate.Invoke();
            }          
        }

        public void SetQueue(SinglyList<T> queue)
        {
            Stream = queue;
        }
    }
}
