
using System;
using System.Collections.Generic;

namespace Manager.Events
{
    public class EventController
    {
        private Event Event;
        private DebugLog.WriteInLogDelegate WriteToFile;

        public EventController(DebugLog.WriteInLogDelegate _writeToFile)
        {
            WriteToFile = _writeToFile;
        }

        public void ChooseMode(int code)
        {
            switch (code)
            {
                case 0:
                    //Episode_1
                    Event = new DataLoad().GetStory(code);
                    Event.AddDebugLog(WriteToFile);
                    Event.AfterInitialization(NextEvent);
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }

        public void NextEvent(int code)
        {
            /*if (code == -1)
                //throw new System.Exception();
            else
            {
                Event = new DataLoad().GetStory(code);
                Event.AfterInitialization(NextEvent);
            }*/
            if(code != -1)
            {
                Event = new DataLoad().GetEvent(code);
                Event.AfterInitialization(NextEvent);
            }
            
        }

    }

    public class Story
    {
        //Database Variables
        public int Code { get; set; }
        public int StartEventCode { get; set; }
    }
}

