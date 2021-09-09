using System.Collections.Generic;
using UnityEngine;

public class StasisData : MonoBehaviour
{
    public Shutters Shutters { get; set; }
    public Dictionary<int, ChamberData> StasisChambers { get; private set; }
    public enum JobTypeEnum { Architect, Builder, Farmer, Diplomat, Economist, Administrator, Culture, Engineer, Researcher, Terraformer };
    public enum ChamberStatusEnum { dead, alive };
    public enum GenderEnum { male, female};

    void Awake()
    {
        this.tag = "StasisChambers";
        StasisChambers = new Dictionary<int, ChamberData>();
        int i = 0;
        foreach (InteractableObject chamber in this.GetComponentsInChildren<InteractableObject>(true))
        {
            StasisChambers.Add(chamber.name.GetHashCode(), new ChamberData(chamber, (JobTypeEnum)i, ChamberStatusEnum.alive, (GenderEnum)(i%2)));
            i++;
        }
    }

    public class ChamberData
    {
        public InteractableObject InteractableObject { get; }
        public JobTypeEnum JobReq { get; } 
        public ChamberStatusEnum ChamberStatus { get; set; } 
        public GenderEnum Gender { get; } 

        public ChamberData(InteractableObject _interactableObject, JobTypeEnum _jobReq, ChamberStatusEnum _chamberStatus, GenderEnum _gender)
        {
            InteractableObject = _interactableObject;
            JobReq = _jobReq;
            ChamberStatus = _chamberStatus;
            Gender = _gender;
        }
    }
}
