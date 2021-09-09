namespace Manager.Scene
{
    public class SceneHolder : SceneTransitionManager
    {
        public System.Action<int> TempDelegate { get; private set; }
        void Start()
        {
            TempDelegate = Continue();
            TempDelegate.Invoke(1);
        }

    }
}

