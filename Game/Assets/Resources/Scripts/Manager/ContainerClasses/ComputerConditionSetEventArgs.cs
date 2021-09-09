using CyberTale.Collections;
using Manager.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Manager.Events.Type
{
    public class ComputerConditionSetEventArgs : System.EventArgs
    {
        public Alarms Alarm { private get; set; }
        public delegate void setUIDelegate(UnityEngine.Transform transform, ComputerConditionSetEventArgs computerConditionSet, int id);
        public UIContoller<Dictionary<ConditionEnum, UnityEngine.Transform>> UIContoller { get; internal set; }
        public readonly IActiveObject DefaultObject;
        public Dictionary<string, int> Assingment { get; internal set; }
        public List<int> Answers { get; }
        public OperationEnum Operation { get; }
        private readonly bool Equal;
        private setUIDelegate SetUIComponents { get; }
        public int LeftDigit { get; internal set; }
        public int RightDigit { get; internal set; }
        public int Child_Id { get; internal set; }
        public int NumberOfDigits { get; }
        public SubObjectTypeEnum Button_Id { get; private set; }
        private readonly List<string> SplitedDialogue;
        readonly SinglyList<EventPhase> endEventPhaseData;
        bool answeredOnce = false;
        public ComputerConditionSetEventArgs(IActiveObject _object, setUIDelegate _setDialogueUI)
        {
            SetUIComponents = _setDialogueUI;
            DefaultObject = _object;
            Child_Id = 0;
            if (!string.IsNullOrEmpty(DefaultObject.Dialogue))
                SplitedDialogue = Regex.Matches(DefaultObject.Dialogue, @"(\b.{1,125})(?:\s+|$)").Cast<Match>().Select(p => p.Groups[1].Value).ToList();
        }

        public ComputerConditionSetEventArgs(setUIDelegate _setAssingmentUI, IActiveObject _object, Dictionary<string, int> _assingment, List<int> _answers, OperationEnum _operation, bool _equal, int _numberOfDigits, SinglyList<EventPhase> _endEventPhaseData)
        {
            SetUIComponents = _setAssingmentUI;
            Assingment = _assingment;
            DefaultObject = _object;
            Answers = _answers;
            Operation = _operation;
            Equal = _equal;
            NumberOfDigits = _numberOfDigits - 1;
            Child_Id = 0;
            endEventPhaseData = _endEventPhaseData;
        }

        public bool CheckAnswers(bool skip = false, EndActionData endActionData = new EndActionData())//TODO Remove endActionData if not needed
        {
            if (!answeredOnce)
            {
                answeredOnce = true;
                MonitorHeaderScroll.textToScroll = "Everything is fine.";
                if (Alarm != null)
                    Alarm.ActivateAlarm(1, false);
                if (skip)
                {
                    DefaultObject.ConfirmAction(DefaultObject.ActionType, endActionData);
                    return true;
                }
                else
                    foreach (var answer in Answers)
                    {
                        UnityEngine.Debug.Log(answer);
                    }
                if (Equal == CheckIfAnswersMatch())
                {
                    UnityEngine.Debug.Log(Equal == CheckIfAnswersMatch());
                    DefaultObject.ConfirmAction(DefaultObject.ActionType, new EndActionData() { EndEventPhaseData = endEventPhaseData });
                    return true;
                }
                else
                {
                    DefaultObject.ConfirmAction(DefaultObject.ActionType, new EndActionData() { ConditionFailed = true });
                    return false;
                }
            }
            return true;
        }

        bool CheckIfAnswersMatch()
        {
            int temp = ((ICheckAnswer<int, ComputerConditionSetEventArgs>)Condition.Conditions[DefaultObject.ActionType]).Check(this);
            foreach (int answer in Answers)
            {
                if (answer == temp)
                    return true;
            }
            return false;
        }

        public void GetConditionDefinition(UnityEngine.Transform transform)
        {
            LeftDigit = 0;
            RightDigit = 0;
            SetUIComponents.Invoke(transform, this, 0);
        }
        public void GetButtonDefinition(UnityEngine.Transform transform, SubObjectTypeEnum button_Id)
        {
            Button_Id = button_Id;
            SetUIComponents.Invoke(transform, this, 1);
        }
        public UnityEngine.Transform GetUIScreen()
        {
            return UIContoller.Screens[DefaultObject.ActionType];
        }
        /*public bool CheckConditionEnum(ConditionEnum conditionEnum)
        {
            return false;
            if (conditionEnum == DefaultObject.ActionType)
                return true;
            else
                return false;
        }*/

        internal string GetDialogue(int id)
        {
            try
            {
                return SplitedDialogue[id];
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return null;
            }
        }
        public void SetWatcherOnEndPhase(EndPhaseDel recordProgress)
        {
            DefaultObject.RecordProgress = recordProgress;
        }
    }

}