using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Scene
{
    public abstract class SceneTransitionManager : MonoBehaviour
    {
        protected System.Action<int> Continue(int wait = 1)
        {
            GameObject[] gameObjects = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();
            var temp = gameObjects[0].GetComponent<SceneHolder>();
            temp.TempDelegate?.Invoke(wait);
            return NextScene;
        }
        void NextScene(int wait = 1)
        {
            StartCoroutine(WaitBeforeNextScene(wait));
        }

        IEnumerator WaitBeforeNextScene(int wait)
        {
            yield return new WaitForSeconds(wait);
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene > 1)
                StartCoroutine(UnloadScene(currentScene));
            else
                StartCoroutine(LoadScene(currentScene));
        }

        IEnumerator LoadScene(int currentScene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentScene + 1, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentScene + 1));
        }
        IEnumerator UnloadScene(int currentScene)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            while (!asyncUnload.isDone)
                yield return null;
            StartCoroutine(LoadScene(currentScene));
        }

    }
}

