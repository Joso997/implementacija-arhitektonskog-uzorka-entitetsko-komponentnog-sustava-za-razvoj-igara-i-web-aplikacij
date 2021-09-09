using Manager.Events.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIData : MonoBehaviour
{
    public Dictionary<ConditionEnum, Transform> ConditionScreens { get; private set; }
    public Dictionary<ConditionEnum, Transform> StasisMonitorScreens { get; private set; }
    public Transform ChamberScreens { get; private set; }

    void Awake()
    {
        this.tag = "UI";
        ConditionScreens = new Dictionary<ConditionEnum, Transform>();
        StasisMonitorScreens = new Dictionary<ConditionEnum, Transform>();
        var mainMonitor = this.transform.Find("___Main Monitor Canvas___/Main-Monitor-Canvas/").GetComponent<Transform>();
        foreach (ScreenId screen in mainMonitor.Find("Condition-Screens/").GetComponentsInChildren<ScreenId>(true))
        {
            ConditionScreens.Add(screen.conditionEnum, screen.transform);
        }
        var stasisMonitor = this.transform.Find("___Statis Monitor Canvas___/Statis-Monitor-Canvas/").GetComponent<Transform>();
        foreach (ScreenId screen in stasisMonitor.Find("Stasis-Screens/").GetComponentsInChildren<ScreenId>(true))
        {
            StasisMonitorScreens.Add(screen.conditionEnum, screen.transform);
        }
        ChamberScreens = this.transform.Find("___Statis Main Monitor Canvas___/Statis-Main-Monitor-Canvas/Chamber-Screens/").GetComponent<Transform>();
    }
}
