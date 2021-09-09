using Manager.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : SceneTransitionManager
{
    public enum ButtonEnum { Back, Start, Help, Exit, Menu};
    int previousScene = 0;
    public delegate void MenuActived(bool enabled);
    public static event MenuActived OnMenu;
    public GameObject ScreenCredits;
    public GameObject ScreenMainMenu;
    private void Awake()
    {
        int i = 0;
        foreach (var button in GetComponentsInChildren<Button>(true))
        {
            button.name = i.ToString();
            button.onClick.AddListener(() => OnButton(button.name));
            i++;
        }
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene old, Scene @new)
    {
        if (old.buildIndex == 1)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            OnMenu?.Invoke(true);
        }
        else
        if (@new.buildIndex == 1)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(false);
            OnMenu?.Invoke(false);
        }
        else
        if (old.buildIndex > 1 && @new.buildIndex == 0)
            OnMenu = null;
        previousScene = old.buildIndex;
    }
    public void OnButton(string buttonId)
    {
        switch (System.Enum.Parse(typeof(ButtonEnum), buttonId))
        {
            case ButtonEnum.Back:
                ScreenCredits.SetActive(false);
                ScreenMainMenu.SetActive(true);
                break;
            case ButtonEnum.Start:
                if(previousScene == 0)
                    Continue();
                else
                    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(previousScene));
                break;
            case ButtonEnum.Help:
                ScreenCredits.SetActive(true);
                ScreenMainMenu.SetActive(false);
                break;
            case ButtonEnum.Exit:
                if (previousScene == 0)
                    Application.Quit();
                else
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(previousScene));
                previousScene = 0;
                OnMenu = null;
                break;
            case ButtonEnum.Menu:
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
                break;
            default:
                Debug.Log("Something Went Wrong (MainMenu Buttons)");
                break;
        }
    }
}
