using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager.Streaming.Type
{
    public sealed partial class Action
    {
        public sealed class Avoid : ActionInterface.Assingment
        {
            public override bool Act(IActiveObject _object, StackDataDel action = null)
            {           
                System.Array values = System.Enum.GetValues(typeof(OperationEnum));
                System.Random random = new System.Random();
                OperationEnum operation = (OperationEnum)values.GetValue(random.Next(values.Length));
                Dictionary<string, int> temp = GenerateAssignment();
                InvokeController(_object, temp, GenerateSolution(temp.ToDictionary(x => x.Key, x => x.Value), operation), operation, EndEventPhaseData(), false);
                ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Clock].ChooseSubType(new DependentObject() {ActionType = ConditionEnum.Time } as IActiveObject);
                return true;
            }
            public override int Check(ComputerConditionSetEventArgs answers)
            {
                Debug.Log("Answers First: " + answers.Assingment.ElementAt(answers.LeftDigit).Value + " Second: " + answers.Assingment.ElementAt(answers.RightDigit).Value);
                Debug.Log("Result: " + Calculate(answers.Assingment.ElementAt(answers.LeftDigit).Value, answers.Assingment.ElementAt(answers.RightDigit).Value, answers.Operation));
                return Calculate(answers.Assingment.ElementAt(answers.LeftDigit).Value, answers.Assingment.ElementAt(answers.RightDigit).Value, answers.Operation);
            }
            protected override void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent)
            {
                MonitorHeaderScroll.textToScroll = "!Alert! Collision Incoming.";
                Debug.Log("Answers First: "+computerConditionSetEvent.Answers[0]+" Second: "+ computerConditionSetEvent.Answers[1]+" Third: "+ computerConditionSetEvent.Answers[2]);
                switch (computerConditionSetEvent.Child_Id)
                {
                    case 1:
                        screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Answers.ElementAt(0).ToString();
                        screenUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Answers.ElementAt(1).ToString();
                        screenUI.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Answers.ElementAt(2).ToString();
                        break;
                    case 2:
                        screenUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(computerConditionSetEvent.LeftDigit).Value.ToString();
                        screenUI.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(computerConditionSetEvent.RightDigit).Value.ToString();
                        screenUI.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(0).Value.ToString();
                        screenUI.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(1).Value.ToString();
                        screenUI.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(2).Value.ToString();
                        screenUI.transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString(computerConditionSetEvent.Operation);
                        break;
                    default:
                        break;
                };
            }

            protected override Dictionary<string, int> GenerateAssignment()
            {
                Dictionary<string, int> LettersDictionary = new Dictionary<string, int>();
                int value;
                for (int i = 0; i < 3; i++)
                {
                    value = new System.Random().Next(0, 10);
                    if (LettersDictionary.ContainsValue(value))
                        i--;
                    else
                        LettersDictionary.Add(i.ToString(), value);
                    
                }
                return LettersDictionary;
            }

            public override List<int> GenerateSolution(Dictionary<string, int> dictionary, OperationEnum operation)
            {
                List<int> temp = new List<int>();
                int i = 1;
                int randomNum = new System.Random().Next(1, 3);
                foreach (KeyValuePair<string, int> keyValuePair in dictionary)
                {
                    foreach (KeyValuePair<string, int> valuePair in dictionary)
                    {
                        if (i % randomNum == 0)
                        {
                            int num = Calculate(keyValuePair.Value, valuePair.Value, operation);
                            if (temp.Contains(num))
                                continue;
                            temp.Add(num);
                            break;
                        }
                        i++;
                    }
                    i = 1;
                    randomNum = new System.Random().Next(1, 3);
                }
                /*foreach (int entry in temp)
                    UnityEngine.Debug.Log(entry);*/
                return temp;
            }

            protected override void ChooseUIButton(Transform screenUI, ComputerConditionSetEventArgs conditionSet)
            {
                switch (conditionSet.Button_Id)
                {
                    case SubObjectTypeEnum.LeftButton:
                        screenUI.GetChild(conditionSet.Child_Id).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(conditionSet.LeftDigit).Value.ToString();
                        break;
                    case SubObjectTypeEnum.MiddleButton:
                        break;
                    case SubObjectTypeEnum.RightButton:
                        screenUI.GetChild(conditionSet.Child_Id).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(conditionSet.RightDigit).Value.ToString();
                        break;
                }
            }
            protected override SinglyList<EventPhase> EndEventPhaseData()
            {
                var temp = new SinglyList<EventPhase>();
                //var test = new List<Consequence>() { new Consequence() { Code = 0,  } };
                temp.EnList(new EventPhase() { Code = -1, Consequences = null, EndingCondition = System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Vent), RepeatNumber = 1 });
                return temp;
            }
        }
    }

}

