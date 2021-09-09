using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager.Streaming.Type
{
    public sealed partial class Action
    {
        public sealed class Hack : ActionInterface.Assingment
        {
            public override bool Act(IActiveObject _object, StackDataDel action = null)
            {
                Array values = Enum.GetValues(typeof(OperationEnum));
                System.Random random = new System.Random();
                OperationEnum operation = (OperationEnum)values.GetValue(random.Next(values.Length));
                Dictionary<string, int> temp = GenerateAssignment();
                InvokeController(_object, temp, GenerateSolution(temp.ToDictionary(x => x.Key, x => x.Value), operation), operation); //ToDictinary because it is used like a refrence
                return true;
            }

            public override int Check(ComputerConditionSetEventArgs answers)
            {
                return Calculate(answers.Assingment[answers.Assingment.ElementAt(answers.LeftDigit).Key], answers.Assingment[answers.Assingment.ElementAt(answers.RightDigit).Key], answers.Operation);
            }

            protected override void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent)
            {
                switch (computerConditionSetEvent.Child_Id)
                {
                    case 1:
                        screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(0).Key.ToString();
                        screenUI.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(0).Value.ToString();
                        screenUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(1).Key.ToString();
                        screenUI.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(1).Value.ToString();
                        screenUI.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(2).Key.ToString();
                        screenUI.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(2).Value.ToString();
                        break;
                    case 2:
                        screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Answers.First().ToString();
                        screenUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(computerConditionSetEvent.LeftDigit).Key.ToString();
                        screenUI.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(computerConditionSetEvent.RightDigit).Key.ToString();
                        screenUI.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(0).Key.ToString();
                        screenUI.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(1).Key.ToString();
                        screenUI.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Assingment.ElementAt(2).Key.ToString();
                        screenUI.transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = OperationToString(computerConditionSetEvent.Operation);
                        break;
                    default:
                        break;
                };
            }

            string GetLetter()
            {
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                System.Random rand = new System.Random();
                int num = rand.Next(0, chars.Length - 1);
                return chars[num].ToString();
            }

            protected override Dictionary<string, int> GenerateAssignment()
            {
                Dictionary<string, int> LettersDictionary = new Dictionary<string, int>();
                string letter;
                int value;
                for (int i = 0; i < 3; i++)
                {
                    letter = GetLetter();
                    value = new System.Random().Next(0, 10);
                    if (LettersDictionary.ContainsKey(letter))
                        i--;
                    else
                        LettersDictionary.Add(letter, value);
                }
                return LettersDictionary;
            }

            

            public override List<int> GenerateSolution(Dictionary<string, int> dictionary, OperationEnum operation)
            {
                Queue<int> assingment = new Queue<int>();
                for (int i = 0; i <= 2; i++)
                {
                    if (i % 2 == 0)
                    {
                        assingment.Enqueue(dictionary.First().Value);
                        dictionary.Remove(dictionary.First().Key);
                    }
                    else
                    {
                        assingment.Enqueue((int)operation);
                    }
                }
                return new List<int>() { TakeApart(assingment) };
            }
            int TakeApart(Queue<int> assingment)
            {
                int rez = 0;
                for (int i = 0; i <= assingment.Count / 3; i++)
                {
                    int x = assingment.Dequeue();
                    int operation = assingment.Dequeue();
                    int y = assingment.Dequeue();
                    rez = Calculate(x, y, (OperationEnum)operation);
                }
                return rez;
            }

            protected override void ChooseUIButton(Transform screenUI, ComputerConditionSetEventArgs conditionSet)
            {
                switch (conditionSet.Button_Id)
                {
                    case SubObjectTypeEnum.LeftButton:
                        screenUI.GetChild(conditionSet.Child_Id).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(conditionSet.LeftDigit).Key.ToString();
                        break;
                    case SubObjectTypeEnum.MiddleButton:
                        break;
                    case SubObjectTypeEnum.RightButton:
                        screenUI.GetChild(conditionSet.Child_Id).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = conditionSet.Assingment.ElementAt(conditionSet.RightDigit).Key.ToString();
                        break;
                }
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

