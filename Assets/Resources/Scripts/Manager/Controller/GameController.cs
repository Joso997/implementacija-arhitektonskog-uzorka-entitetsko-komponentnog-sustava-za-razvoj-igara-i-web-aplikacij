using System.Collections;
using UnityEngine;
using CielaSpike;

namespace Manager
{
    public class GameController : Initialization
    {
        bool showEventData = false;
        IEnumerator LoadEventData()
        {
            yield return this.StartCoroutineAsync(Blocking());
            MemoryGameMechanicContorller.StartAssingment();
            DialogueGameMechanicController.MakeUIStartable();
        }
        IEnumerator Blocking()
        {         
            yield return new WaitUntil(() => showEventData == true);
        }

        private void Start()
        {
            EventController.ChooseMode(0);
            StartCoroutine(LoadEventData());
        }

        public void ShowEventData()
        {
            showEventData = true;
        }
        
    }
}
