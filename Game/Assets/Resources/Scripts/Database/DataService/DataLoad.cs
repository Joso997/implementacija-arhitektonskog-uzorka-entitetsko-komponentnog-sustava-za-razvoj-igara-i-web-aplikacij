using System.Collections;
using System.Collections.Generic;
using Manager.Events;
using CyberTale.Collections;

public class DataLoad
{
    private readonly DataService DataService;

    // Use this for initialization
    public DataLoad()
    {
        DataService = new DataService("GameTwo.db");
    }

    public Event GetStory(int code)
    {
        return GetEvent(DataService._connection.Table<Story>().Where(x => x.Code == 0).First().StartEventCode);
    }

    public Event GetEvent(int code)
    {
        return DataService._connection.Table<Event>().Where(x => x.Code == code).First();
    }

    public SinglyList<EventPhase> GetEventPhases(int code, Manager.DebugLog.WriteInLogDelegate writeToFile)
    {
        SinglyList<EventPhase> eventPhases = new SinglyList<EventPhase>();
        EventPhase _temp;
        while (code >= 0)
        {
            _temp = GetEventPhase(code);
            _temp.AfterInitialization(writeToFile);
            eventPhases.EnList(_temp);
            code = _temp.NextPhaseCode;
        }
        return eventPhases;
    }

    /*public Queue<Dialogue> GetDialogues(int code)
    {
        Queue<Dialogue> dialogue = new Queue<Dialogue>();
        Dialogue _temp;
        while (code >= 0)
        {
            _temp = GetDialogue(code);
            dialogue.Enqueue(_temp);
            code = _temp.NextDialogueCode;
        }
        return dialogue;
    }*/

    public Dialogue GetDialogue(int code)
    {
        Dialogue temp;
        try
        {
            temp = DataService._connection.Table<Dialogue>().Where(x => x.CodeEventPhase == code).First();
        }
        catch (System.InvalidOperationException)
        {
            temp = new Dialogue();
        }
        return temp;
    }

    public EventPhase GetEventPhase(int code)
    {
        return DataService._connection.Table<EventPhase>().Where(x => x.Code == code).First();
    }

    public IEnumerable<Consequence> GetConsequences(int code)
    {
        return DataService._connection.Table<Consequence>().Where(x => x.CodeEventPhase == code);
    }
}
