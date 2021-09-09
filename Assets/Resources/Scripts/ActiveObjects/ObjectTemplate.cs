using Manager.Events;
using Manager.Events.Type;
using UnityEngine;

public class ObjectTemplate : MonoBehaviour, IActiveObject
{
    //Object Type Variables
    public RegionEnum Region { get; set; }
    public InteractableObjectTypeEnum ObjectType { get; set; }
    public SubObjectTypeEnum SubObjectType { get; set; }
    public ConditionEnum ActionType { get; set; }
    public string Dialogue { get; set; }
    //For Storing Value In Editor
    public int Region_index;
    public int InteractableObjectType_index;
    public int SubObjectType_index;
    public int Action_index;
    //For Temp Storage Of Value In Editor
    //public int interactableObjectType_index;
    public int subObjectType_index;
    public int action_index;

    //Object Task Variables
    public ConditionEnum EndingCondition { get; protected set; }
    protected EndPhaseDel EndPhase;
    public EndPhaseDel RecordProgress { get; set; }

    public void ConfirmAction(ConditionEnum _action, EndActionData endActionData)
    {
        if (EndingCondition == _action)
        {
            var tempEndPhase = EndPhase;
            EndingCondition = ConditionEnum.None;
            EndPhase = null;
            RecordProgress?.Invoke(endActionData);
            RecordProgress = null;
            tempEndPhase.Invoke(endActionData);
            //For Testing Only
            /*if (this.GetType() == typeof(InteractableObject))
            {
                Transform RefuelUI = GameObject.FindGameObjectWithTag("UI").transform.Find("___Monitor Canvas___/Main-Monitor-Canvas/Condition-Screens/$Refuel");
                GameObject RefuelUI_Start = RefuelUI.GetChild(0).gameObject;
                GameObject RefuelUI_End = RefuelUI.GetChild(1).gameObject;
                RefuelUI_Start.SetActive(false);
                RefuelUI_End.SetActive(true);
                MaterialPropertyBlock block;
                block = new MaterialPropertyBlock();
                block.SetColor("_BaseColor", Color.green);
                this.GetComponent<Renderer>().SetPropertyBlock(block);
            }*/

        }
    }

}

