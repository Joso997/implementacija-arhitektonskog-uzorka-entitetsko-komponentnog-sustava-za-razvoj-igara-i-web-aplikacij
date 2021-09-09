using Manager.Events.Type;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Manager.Mechanics;

namespace Manager.UI
{
    public abstract class UIContoller<T> : System.IDisposable, IPause
    {
        ManualResetEvent mre = new ManualResetEvent(true);
        protected Thread UIThread { get; set; }
        public T Screens { get; set; }
        protected System.Action<Transform> GetConditionDefinition { get; set; }
        protected System.Func<int, int> RefreshChildCount { get; set; }
        protected System.Func<bool, EndActionData, bool> CheckAnswers { get; set; }
        public UIContoller(T _screens)
        {
            Screens = _screens;
        }
        public abstract void SetUI(Transform uIScreens, int id, int wait);
        public abstract void StartUI(Transform uIScreens, ExtraData extraData, int id, int wait);
        void Fade(GameObject temp, int id, int wait)
        {
            int time = wait;
            while (time != 0)
            {
                mre.WaitOne(Timeout.Infinite);
                Thread.Sleep(1000);
                time--;
            }
            UnityMainThreadDispatcher.Instance().Enqueue(DoFade(temp, id, wait));
        }
        IEnumerator DoFade(GameObject temp, int id, int wait)
        {
            temp.SetActive(false);
            SetUI(temp.transform.parent, id, wait);
            yield return null;
        }
        protected Thread StartTheThread(GameObject temp, int id, int wait)
        {
            var t = new Thread(() => Fade(temp, id, wait));
            t.IsBackground = true;
            t.Start();
            if (UIThread == null)
                Resume();
            return t;
        }
        public void StopUI(Transform uIScreens)
        {
            if (UIThread != null)
            {
                UIThread.Abort();
                foreach (Transform child in uIScreens.GetComponentInChildren<Transform>())
                    child.gameObject.SetActive(false);
            }
        }
        public void StopUI(Transform uIScreens, int id)
        {
            if (UIThread != null)
            {
                UIThread.Abort();
                uIScreens.GetChild(id).gameObject.SetActive(false);
            }               
        }
        public void Restart(Transform uIScreens, int id, int wait)
        {
            StopUI(uIScreens, id);
            SetUI(uIScreens, id, wait);
        }
        public void RestartAll(Transform uIScreens, int id, int wait)
        {
            StopUI(uIScreens);
            SetUI(uIScreens, id, wait);
        }
        public void Pause()
        {
            mre.Reset();
        }

        public void Resume()
        {
            mre.Set();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(UIThread != null)
                        UIThread.Abort();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}