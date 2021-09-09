using UnityEngine;
using Manager.Streaming;
using Manager.Events;
using Manager.Tracking;
using Manager.Mechanics.MemoryGame;
using Manager.Mechanics.PowerGame;
using Manager.Mechanics.DialogueGame;
using System.Collections.Generic;
using Manager.Events.Type;
using Manager.UI;
using Manager.Mechanics.TimeGame;
using Manager.Mechanics.ColonizationGame;

namespace Manager
{
    public class Initialization : MonoBehaviour
    {
        protected DebugLog DebugLog { get; private set; }
        protected StreamHandler StreamHandler { get; private set; }
        protected GlobalStats GlobalStats { get; private set; }
        protected EventController EventController { get; private set; } 
        protected MemoryGameMechanicController MemoryGameMechanicContorller { get; private set; }
        protected TimeGameMechanicController TimeGameMechanicController { get; private set; }
        /*protected PowerGameMechanicController PowerGameMechanicController { get; private set; }
        protected ThrustersMechanicController ThrustersMechanicController { get; private set; }*/
        protected ColonizationGameMechanicController ColonizationGameMechanicController { get; private set; }
        protected DialogueGameMechanicController DialogueGameMechanicController { get; private set; }
        protected PowerGameMechanicController PowerGameMechanicController { get; private set; }
        protected EndingGameController EndingGameController { get; private set; }

        void Awake()
        {
            LoadMechanics();
            if (UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1).isLoaded)
            {
                GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1).GetRootGameObjects();
                MainMenu.OnMenu += OnMenu;
            }
            //UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void OnMenu(bool _enabled)
        {
            this.enabled = _enabled;
        }

        void LoadMechanics()
        {
            Dictionary<ConditionEnum, Transform> conditionScreens = GameObject.FindGameObjectWithTag("UI").GetComponent<UIData>().ConditionScreens;
            Dictionary<ConditionEnum, Transform> stasisMonitorScreens = GameObject.FindGameObjectWithTag("UI").GetComponent<UIData>().StasisMonitorScreens;
            Dictionary<int, StasisData.ChamberData> stasisChambers = GameObject.FindGameObjectWithTag("StasisChambers").GetComponent<StasisData>().StasisChambers;
            Alarms alarms = this.GetComponent<Alarms>();
            DebugLog = new DebugLog();
            TimeGameMechanicController = new TimeGameMechanicController(new TimeUI(conditionScreens));
            MemoryGameMechanicContorller = new MemoryGameMechanicController(new AssingmentUI(conditionScreens), alarms);
            PowerGameMechanicController = new PowerGameMechanicController(new PowerUI(conditionScreens));
            EndingGameController = new EndingGameController(new EndingUI(conditionScreens), stasisChambers);
            //ThrustersMechanicController = new ThrustersMechanicController();
            ColonizationGameMechanicController = new ColonizationGameMechanicController(new StasisUI(stasisMonitorScreens), stasisChambers);
            DialogueGameMechanicController = new DialogueGameMechanicController(new DialogueUI(conditionScreens));
            StreamHandler = new StreamHandler(DebugLog.SetDataPath(Application.dataPath + "/DebugLogFile/textfile.txt"));
            GlobalStats = new GlobalStats(Tracking.Type.DifficultyEnum.Medium, DebugLog.WriteToFile);
            EventController = new EventController(DebugLog.WriteToFile);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Scanner>().Streamer = StreamHandler.Streamer;
            StreamHandler.ActedEvent += GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().Initialize;
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.Pressure,0);
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.EnergyGeneration,0);
        }

        /*private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            //GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();
        }*/

        public void Test(ObjectTemplate temp)
        {
            MaterialPropertyBlock block;
            block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", Color.green);
            temp.GetComponent<Renderer>().SetPropertyBlock(block);
        }

        void OnEnable()
        {
            ColonizationGameMechanicController.Resume();
            DialogueGameMechanicController.Resume();
            TimeGameMechanicController.Resume();
            MemoryGameMechanicContorller.Resume();
        }

        void OnDisable()
        {
            ColonizationGameMechanicController.Pause();
            DialogueGameMechanicController.Pause();
            TimeGameMechanicController.Pause();
            MemoryGameMechanicContorller.Pause();
        }

#if UNITY_ANDROID
        void OnApplicationFocus(bool focus)
        {
            if (focus) OnEnable();
            else OnDisable();
        }
#endif

#if UNITY_EDITOR || UNITY_IOS
        void OnApplicationPause(bool pause)
        {
            if (pause) OnDisable();
            else OnEnable();
        }
#endif

        void OnDestroy()
        {
            ColonizationGameMechanicController.Stop();
            DialogueGameMechanicController.Stop();
            TimeGameMechanicController.Stop();
            MemoryGameMechanicContorller.Stop();
            GlobalStats.Stop();
            Condition.MakeNull();
            ObjectTypes.MakeNull();
            //(Condition.Conditions[ConditionEnum.Dialogue] as ISubController<ActedDefaultDel>).UnSubscribeController();
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }
    }
}


