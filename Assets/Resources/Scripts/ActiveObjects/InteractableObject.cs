using Manager.Events.Type;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class InteractableObject : ObjectTemplate, IActiveObject
{
    //Object Type Variables
    public List<InteractableObjectStatEnum> StatsEnums;
    public List<int> StatsValues;
    public GameObject Node;
    public InteractableObject ParentObject;
    [SerializeField] public List<Animator> Indicator;
    public Dictionary<InteractableObjectStatEnum, InteractableObjectStatAbstract> Stats { get; private set; }
    //Event Holder Variables (Use for Refrencing administered functions)
    public Stack<StackDataDel> InvokeEventHolder { get; set; } 
    void Awake()
    {
        Region = (RegionEnum)Region_index;
        ObjectType = (InteractableObjectTypeEnum)InteractableObjectType_index;
        SubObjectType = (SubObjectTypeEnum)SubObjectType_index;
        ActionType = (ConditionEnum)Action_index;
        InvokeEventHolder = new Stack<StackDataDel>();
        if (!this.GetComponent<BoxCollider>())
            this.gameObject.AddComponent<BoxCollider>();
        this.tag = "Node";
        SetStatsIntoDictionary();
        Condition.Subscribe(this.SetEndingCondition, Region, ObjectType, SubObjectType, this.ChangeStat);
    }

    void SetStatsIntoDictionary()
    {
        int amountOfElements = 3; //It should equal to 3
        Stats = new Dictionary<InteractableObjectStatEnum, InteractableObjectStatAbstract>();
        foreach (var list in StatsEnums.Select((x, i) => (Value: x, Index: i)))
        {
            Stats.Add(list.Value, ObjectTypes.InteractableObjectStats[list.Value].Invoke());
            int i_temp = amountOfElements * list.Index;

            Stats[list.Value].minPoints = StatsValues[i_temp];
            Stats[list.Value].Points = StatsValues[i_temp + 1];
            Stats[list.Value].maxPoints = StatsValues[i_temp + 2];
        }
    }

    void SetEndingCondition(TaskLoadedEventArgs e)
    {
        /*MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.red);
        this.GetComponentInChildren<Renderer>().SetPropertyBlock(block);
        Transform RefuelUI = GameObject.FindGameObjectWithTag("UI").transform.Find("___Monitor Canvas___/Main-Monitor-Canvas/Condition-Screens/$Refuel");
        GameObject RefuelUI_Start = RefuelUI.GetChild(0).gameObject;
        RefuelUI_Start.SetActive(true);*/
        if ( ((ICheckAnswer<bool, InteractableObject>) Condition.Conditions[e.ConditionEnum]).Check(this))
        {
            e.EndPhase.Invoke(new EndActionData());
        }
        else
        { 
            EndingCondition = e.ConditionEnum;   
            EndPhase = e.EndPhase;
            UnityEngine.Debug.Log(e.ConditionEnum);
            Condition.Conditions[e.ConditionEnum].InvokeController(this);
        }
    }

    void ChangeStat(StatChangeEventArgs e)
    {
        Stats[e.StatType].Points += e.Amount;
        Stats[e.StatType].CheckRequirements(this);
    }

    public void GiveParent()
    {
        var temp = FindObjectsOfType<InteractableObject>();
        ParentObject = System.Array.Find(temp, test => test.InteractableObjectType_index == InteractableObjectType_index && (SubObjectTypeEnum)test.SubObjectType_index == SubObjectTypeEnum.ParentObject);
    }
}


