
using Manager.Events.Type;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Manager.Mechanics.Interface
{
    public class MechanicInterface
    {
        public abstract class HasUIController<T> : MechanicController
        {
            protected HasUIController(T uIContoller) : base()
            {
                UIContoller = uIContoller;
                dispose = ((System.IDisposable)uIContoller).Dispose;
                pause = ((IPause)uIContoller).Pause;
                resume = ((IPause)uIContoller).Resume;
            }

            protected T UIContoller { get; }
            protected ComputerConditionSetEventArgs ConditionSet { get; set; }
            protected UnityEngine.Transform UIScreens { get; set; }//TODO compress this and UIController screens into one
            protected bool NotFirstTask { get; set; }
            protected abstract void ActivateUI(ComputerConditionSetEventArgs e);
            protected virtual int RefreshChildCount(int id)
            {
                id++;
                ConditionSet.Child_Id = id;
                return id;
            }
        }

        public abstract class DefaultControler : MechanicController
        {
            protected DefaultControler() : base(){}
        }

    }
}