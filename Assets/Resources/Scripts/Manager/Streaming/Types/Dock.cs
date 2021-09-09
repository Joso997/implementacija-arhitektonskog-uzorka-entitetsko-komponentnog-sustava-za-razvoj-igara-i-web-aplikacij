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
        public sealed class Dock : ActionInterface.Assingment
        {
            public override bool Act(IActiveObject _object, StackDataDel action = null)
            {
                System.Array values = System.Enum.GetValues(typeof(OperationEnum));
                System.Random random = new System.Random();
                OperationEnum operation = OperationEnum.Plus;
                Dictionary<string, int> temp = GenerateAssignment();
                InvokeController(_object, temp, GenerateSolution(temp.ToDictionary(x => x.Key, x => x.Value), operation), operation, null, true, 4);
                return true;
            }
            public override int Check(ComputerConditionSetEventArgs answers)
            {
                if(answers.Assingment.Count < 12)
                    return -1000;
                int x = Calculate(answers.Assingment.ElementAt(2).Value, answers.Assingment.ElementAt(4).Value, (OperationEnum)answers.Assingment.ElementAt(3).Value);
                x = Calculate(x, answers.Assingment.ElementAt(6).Value, (OperationEnum)answers.Assingment.ElementAt(5).Value);
                int y = Calculate(answers.Assingment.ElementAt(7).Value, answers.Assingment.ElementAt(9).Value, (OperationEnum)answers.Assingment.ElementAt(8).Value);
                y = Calculate(y, answers.Assingment.ElementAt(11).Value, (OperationEnum)answers.Assingment.ElementAt(10).Value);
                UnityEngine.Debug.Log("Prvi number 1: " + answers.Assingment.ElementAt(2).Value + " number 2: " + answers.Assingment.ElementAt(4).Value + " number 3: " + answers.Assingment.ElementAt(6).Value);
                UnityEngine.Debug.Log("Drugi number 1: " + answers.Assingment.ElementAt(7).Value + " number 2: " + answers.Assingment.ElementAt(9).Value + " number 3: " + answers.Assingment.ElementAt(11).Value);
                UnityEngine.Debug.Log("nubmer x: " + x + " number y: " +y);
                if (x == y)
                    return -500;
                else
                    return -1000;
            }
            protected override void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs conditionSet)
            {
                switch (conditionSet.Child_Id)
                {
                    case 1:
                        screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(0).Value.ToString();
                        screenUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(1).Value.ToString();
                        break;
                    case 2:
                        screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(0).Value.ToString();
                        screenUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(4)) + conditionSet.Answers.ElementAt(conditionSet.LeftDigit).ToString();
                        screenUI.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(4)) + conditionSet.Answers.ElementAt(conditionSet.RightDigit).ToString();
                        screenUI.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(4)) + conditionSet.Answers.ElementAt(0).ToString();
                        screenUI.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(5)) + conditionSet.Answers.ElementAt(1).ToString();
                        screenUI.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(6)) + conditionSet.Answers.ElementAt(2).ToString();
                        screenUI.transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(7)) + conditionSet.Answers.ElementAt(3).ToString();
                        break;
                    case 3:
                        screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(1).Value.ToString();
                        screenUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(4)) + conditionSet.Answers.ElementAt(conditionSet.LeftDigit).ToString();
                        screenUI.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(4)) + conditionSet.Answers.ElementAt(conditionSet.RightDigit).ToString();
                        screenUI.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(4)) + conditionSet.Answers.ElementAt(0).ToString();
                        screenUI.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(5)) + conditionSet.Answers.ElementAt(1).ToString();
                        screenUI.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(6)) + conditionSet.Answers.ElementAt(2).ToString();
                        screenUI.transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString((OperationEnum)conditionSet.Answers.ElementAt(7)) + conditionSet.Answers.ElementAt(3).ToString();
                        break;
                    default:
                        break;
                };
            }

            protected override void ChooseUIButton(Transform screenUI, ComputerConditionSetEventArgs conditionSet)
            {
                switch (conditionSet.Button_Id)
                {
                    case SubObjectTypeEnum.LeftButton:
                        screenUI.GetChild(conditionSet.Child_Id).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "+"+conditionSet.Answers.ElementAt(conditionSet.LeftDigit).ToString();
                        break;
                    case SubObjectTypeEnum.MiddleButton:
                        //temp variables just to be safe
                        var child_Id = conditionSet.Child_Id;
                        var tempUI = screenUI;
                        conditionSet.Assingment.Add("element" + 0 + conditionSet.Child_Id.ToString(), conditionSet.Assingment.ElementAt(child_Id - 2).Value);
                        conditionSet.Assingment.Add("operation" + 1 + conditionSet.Child_Id.ToString(), conditionSet.Answers.ElementAt(4 + conditionSet.LeftDigit));
                        conditionSet.Assingment.Add("element" + 1 + conditionSet.Child_Id.ToString(), conditionSet.Answers.ElementAt(conditionSet.LeftDigit));
                        conditionSet.Assingment.Add("operation" + 2 + conditionSet.Child_Id.ToString(), conditionSet.Answers.ElementAt(4 + conditionSet.RightDigit));
                        conditionSet.Assingment.Add("element" + 2 + conditionSet.Child_Id.ToString(), conditionSet.Answers.ElementAt(conditionSet.RightDigit));
                        if (screenUI.childCount > child_Id + 1)
                        {
                            conditionSet.UIContoller.Restart(tempUI, child_Id, 5);
                        }    
                        break;
                    case SubObjectTypeEnum.RightButton:
                        screenUI.GetChild(conditionSet.Child_Id).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "+" + conditionSet.Answers.ElementAt(conditionSet.RightDigit).ToString();
                        break;
                }
            }

            protected override Dictionary<string, int> GenerateAssignment()
            {
                Dictionary<string, int> LettersDictionary = new Dictionary<string, int>();
                int value;
                for (int i = 0; i < 2; i++)
                {
                    value = new System.Random().Next(0, 10);
                    if (LettersDictionary.ContainsValue(value))
                        i--;
                    else
                        LettersDictionary.Add(i.ToString(), value);
                }
                return LettersDictionary; // x, y
                
            }

            public override List<int> GenerateSolution(Dictionary<string, int> dictionary, OperationEnum operation)
            {
                
                List<int> temp = new List<int>();
                int value;
                for (int j = 0; j < 2; j++)
                {
                    value = new System.Random().Next(1, 10);
                    if (temp.Contains(value))
                        j--;
                    else
                    {                    
                        temp.Add(value); // j, l                      
                    }
                         
                }
                int k = dictionary.First().Value + temp[0] + temp[1] - dictionary.Last().Value; // k
                temp.Add(k);
                temp.Add(0);              
                temp.Add((int)OperationEnum.Plus);
                temp.Add((int)OperationEnum.Plus);
                temp.Add((int)OperationEnum.Plus);
                temp.Add((int)OperationEnum.Plus);
                //temp.Add((dictionary.First().Value + temp[0] + temp[1]) + (k + dictionary.Last().Value)); // |y| + |x|             
                temp.Add(-500); // answer             
                return temp; // j, l, k, 0, operation, operation, operation, operation, answer
            }

            protected override SinglyList<EventPhase> EndEventPhaseData()
            {
                var temp = new SinglyList<EventPhase>();
                temp.EnList(new EventPhase() { Code = -1, Consequences = null, EndingCondition = "Vent" });
                return temp;
            }
            
        }
    }

}

