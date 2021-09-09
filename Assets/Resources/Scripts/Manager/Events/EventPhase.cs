using CyberTale.Collections;
using Manager.Events.Type;
using Manager.Tracking;
using System.Collections.Generic;

namespace Manager.Events
{
    public class EventPhase
    {
        public IEnumerable<Consequence> Consequences; //Need to add a constructor
        private Dialogue Dialogue;
        public delegate void NextEventPhaseMethod(SinglyList<EventPhase> singlyList);
        private NextEventPhaseMethod NextEventPhaseMethodDelegate;
        private DebugLog.WriteInLogDelegate WriteToFile;

        //Database Variables
        public int Code { get; set; }
        public string StartingCondition { get; set; }
        public string EndingCondition { get; set; }
        public int RepeatNumber { get; set; }
        public int NextPhaseCode { get; set; }
        public int Region { get; set; }

        public void AfterInitialization(DebugLog.WriteInLogDelegate _writeToFile)
        {
            WriteToFile = _writeToFile;
            Consequences = new DataLoad().GetConsequences(Code);
            Dialogue = new DataLoad().GetDialogue(Code);
        }

        bool CheckStartingCondition()
        {
            return true;
        }

        public void StartPhase(NextEventPhaseMethod nextPhase)
        {
            NextEventPhaseMethodDelegate = nextPhase;
            if (CheckStartingCondition() && System.Enum.TryParse(EndingCondition, out ConditionEnum endingCondition))
            {
                EnactConsequences();
                if (endingCondition == ConditionEnum.None)
                {
                    EndPhase(new EndActionData());
                }
                else
                {
                    PrepareDialogue(endingCondition);
                    Condition.Conditions[endingCondition].InvokeCondition(endingCondition, EndPhase, Dialogue.Content);
                }                                
            }
            else
                EndPhase(new EndActionData());
        }

        public void EndPhase(EndActionData endActionData)
        {
            if (!endActionData.ConditionFailed)
                RepeatNumber--;
            if (RepeatNumber > 0)
                StartPhase(NextEventPhaseMethodDelegate);
            else
                NextEventPhaseMethodDelegate.Invoke(endActionData.EndEventPhaseData);
        }

        void EnactConsequences()
        {
            if (Consequences != null)
                Decider.ProcessConsequences(ref Consequences);
        }

        void PrepareDialogue(ConditionEnum endingCondition)
        {
#if UNITY_EDITOR
            WriteToFile?.Invoke(new List<string>()
                    {
                        "System Condition",
                        System.Enum.GetName(typeof(ConditionEnum), endingCondition),
                        Dialogue.Content != null?Dialogue.Content:"No Dialogue"
                    }
            );
#endif
            /*if(Dialogue.Content != "")
            {
                UnityEngine.Debug.Log(Dialogue.Content);
            }*/
        }
        //For Testing Only
        SinglyList<EventPhase> EventPhaseTest()
        {
            var temp = new SinglyList<EventPhase>();
            temp.EnList(new EventPhase() { Code = -1, Consequences = null, EndingCondition = "Hack" });
            return temp;
        }
    }
}

