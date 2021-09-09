using Manager.Events.Type;
using Manager.Streaming;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    MaterialPropertyBlock block;
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;
    public Streamer<InteractableObject> Streamer { private get; set; }

    void Awake()
    {
        this.tag = "MainCamera";
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1).GetRootGameObjects();
            MainMenu.OnMenu += OnMenu;
        }
    }
    private void OnMenu(bool _enabled)
    {
        this.enabled = _enabled;
    }
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (DoubleClick())
        {
            RaycastHit raycastHit;
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.tag == "Node")
                {
                    Scan(raycastHit.collider.GetComponent<InteractableObject>(), true);
                }
            }
        }else
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.tag == "Node")
                {
                    Scan(raycastHit.collider.GetComponent<InteractableObject>(), false);
                }
            }
        }
#else
        if (DoubleTap())
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.tag == "Node")
                {
                    Scan(raycastHit.collider.GetComponent<InteractableObject>(), true);
                }
            }
        }else
        if (((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.tag == "Node")
                {
                    Scan(raycastHit.collider.GetComponent<InteractableObject>());
                }
            }
        } 
#endif
    }
    public void Scan(InteractableObject sender, bool prepend = false)
    {
        /*block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.red);
        sender.GetComponent<Renderer>().SetPropertyBlock(block);*/
        Streamer.StreamEnqueue(sender, prepend);
    }

    bool DoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked++;
            if (clicked == 1)
                clicktime = Time.time;
        }
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clicktime > 1)
            clicked = 0;
        return false;
    }

    bool DoubleTap()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            clicked++;
            if (clicked == 1)
                clicktime = Time.time;
        }
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clicktime > 1)
            clicked = 0;
        return false;
    }

}
