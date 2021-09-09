using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Mechanics
{
    public abstract class MechanicController : System.IDisposable
    {
        protected System.Action dispose;
        protected System.Action resume;
        protected System.Action pause;
        protected MechanicController()
        {
            SubscribeConditions();
        }
        protected abstract void SubscribeConditions();
        protected abstract void Button(SubObjectTypeEnum button_id);
        protected Dictionary<string, int> Shuffle(Dictionary<string, int> _dictionary)
        {
            System.Random r = new System.Random();
            return _dictionary.OrderBy(x => r.Next()).ToDictionary(item => item.Key, item => item.Value);
        }

        public void Pause()
        {
            pause.Invoke();
        }

        public void Resume()
        {
            resume.Invoke();
        }

        public virtual void Stop()
        {
            Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dispose?.Invoke();
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
