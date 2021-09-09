using System.Collections;
using System.Collections.Generic;
using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using Manager.Mechanics;
using Manager.Tracking;
using Manager.Tracking.Type;
using UnityEngine;

namespace Manager.UI
{
    public class TimeUI : UIContoller<Dictionary<ConditionEnum, Transform>>
    {
        readonly int starttime = 50;
        readonly int endtime = 0;
        public TimeUI(Dictionary<ConditionEnum, Transform> conditionScreens) : base(conditionScreens){}
        public override void SetUI(Transform uIScreens, int id, int wait = 1)
        {
            id = RefreshChildCount.Invoke(id);
            if (endtime < id)
            {
                uIScreens.GetChild(0).gameObject.SetActive(true);
                GetConditionDefinition.Invoke(uIScreens.GetChild(0));
                UIThread = StartTheThread(uIScreens.GetChild(0).gameObject, id, wait);
            }
            else
            {
                StopUI(uIScreens, 0);
                Consequence[] consequencesArray = new Consequence[1];
                for (int i = 0; i < 1; i++)
                {
                    consequencesArray[i].Code = -2;
                    consequencesArray[i].CodeEventPhase = -2;
                    consequencesArray[i].DamageType = DamageTypeEnum.Physical;
                    consequencesArray[i].ObjectType = InteractableObjectTypeEnum.Stasis;
                    consequencesArray[i].Probability = ProbabilityEnum.Unavoidable;
                    consequencesArray[i].Severity = SeverityEnum.Major;
                }
                Decider.ProcessConsequences(ref consequencesArray);
                CheckAnswers.Invoke(true, new EndActionData());
                Debug.Log("Time has run out");
            }
        }

        public override void StartUI(Transform uIScreens, ExtraData extraData, int id = 0, int wait = 1)
        {    
            StopUI(uIScreens, 0);
            CheckAnswers = extraData.CheckAnswers;
            GetConditionDefinition = extraData.GetConditionDefinition;
            RefreshChildCount = extraData.RefreshChildCount;
            GetConditionDefinition.Invoke(uIScreens.GetChild(0));
            //fix to not showing 50 at start of countdown.
            uIScreens.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = (starttime).ToString();
            UIThread = StartTheThread(uIScreens.GetChild(0).gameObject, starttime, wait);
            uIScreens.GetChild(0).gameObject.SetActive(true);
        }
    }
}