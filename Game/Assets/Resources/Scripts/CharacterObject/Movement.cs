using Manager.Events.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector2Int PlayerLocation;
    public GameObject PlayerNode;
    public Transform Player;
    private Vector2Int EndLocation;
    public GameObject EndNode;
    bool move = false;
    bool moveByAnimation = false;
    public string currentLadderAnimation;
    InteractableObject InteractableObject;

    PlayerStats PlayerStats { get; set; }

    public void Start()
    {
        PlayerLocation = PlayerNode.GetComponent<Node>().Location;
        if(EndNode != null)
            EndLocation = EndNode.GetComponent<Node>().Location;
        PlayerStats = new PlayerStats();
    }
    //For Testing Only
    public void Initialize(GameObject node)
    {
        EndNode = node;
        PlayerLocation = PlayerNode.GetComponent<Node>().Location;
        EndLocation = EndNode.GetComponent<Node>().Location;
        this.enabled = true;
    }
    //Runs between Act and Enact
    public void Initialize(ActedUponEventArgs e)
    {
        InteractableObject = e.InteractableObject;
        EndNode = InteractableObject.Node;
        PlayerLocation = PlayerNode.GetComponent<Node>().Location;
        EndLocation = EndNode.GetComponent<Node>().Location;
        this.enabled = true;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            if ((Vector3.Distance(Player.transform.position, PlayerNode.GetComponent<Transform>().transform.position) < 0.01f && !moveByAnimation) || Player.GetComponent<AnimationTrigger>().ladderEndBooleanTEST)
            {
                CheckIfEndMovement();
                if (moveByAnimation)
                {
                    Player.GetComponent<AnimationTrigger>().LadderAnimation(currentLadderAnimation, false);
                    Player.GetComponent<AnimationTrigger>().ladderEndBooleanTEST = false;
                    moveByAnimation = false;
                    Player.GetComponent<AnimationTrigger>().WalkingAnim(true);
                }
                move = false;
            }
            else
            {
                if (!moveByAnimation)
                {
                    Player.GetComponent<AnimationTrigger>().WalkingAnim(true);
                    //Debug.Log("Walking");
                    MovePlayer(PlayerNode.GetComponent<Transform>().position);
                }


            }
        }
        else
        {
            CheckIfEndMovement();
            if (PlayerLocation.x == EndLocation.x)
            {
                CompareLeftRight(PlayerLocation.y, EndLocation.y);
            }
            else
            {
                if (!GoRight())
                {
                    if (RotationCorrect())
                        CompareUpDown(PlayerLocation.x, EndLocation.x);
                    //CompareLeftRight(PlayerLocation.y, EndLocation.y);
                }
            }
            //Debug.Log(PlayerLocation);
        }
    }


    void CompareLeftRight(int player, int end)
    {
        if (PlayerLocation.y != EndLocation.y)
        {
            if (player < end)
            {
                PlayerNode = PlayerNode.GetComponent<Node>().RightNode;
            }
            else
            {
                PlayerNode = PlayerNode.GetComponent<Node>().LeftNode;
            }
            PlayerLocation = PlayerNode.GetComponent<Node>().Location;
            move = true;
        }

    }

    void CompareUpDown(int player, int end)
    {
        Player.GetComponent<AnimationTrigger>().WalkingAnim(false);
        Debug.Log(PlayerLocation.x + "->" + EndLocation.x);
        currentLadderAnimation = PlayerLocation.x + "-" + EndLocation.x;
        Player.GetComponent<AnimationTrigger>().LadderAnimation(currentLadderAnimation, true);
        moveByAnimation = true;
        move = true;
        while (PlayerLocation.x != EndLocation.x)
        {
            if (player < end)
            {
                PlayerNode = PlayerNode.GetComponent<Node>().DownNode;
            }
            else
            {
                PlayerNode = PlayerNode.GetComponent<Node>().UpNode;
            }
            PlayerLocation = PlayerNode.GetComponent<Node>().Location;

        }
        //Player.transform.position = ConvertToPlayerPosition(PlayerNode.GetComponent<Transform>().position);


    }

    bool GoRight()
    {
        if (PlayerNode.GetComponent<Node>().UpNode == null && PlayerNode.GetComponent<Node>().DownNode == null)
        {
            PlayerNode = PlayerNode.GetComponent<Node>().RightNode;
            PlayerLocation = PlayerNode.GetComponent<Node>().Location;
            move = true;
            return true;
        }
        else
        {
            return false;
        }

    }

    void MovePlayer(Vector3 destionation)
    {
        Player.transform.position = Vector3.MoveTowards(Player.transform.position, new Vector3(destionation.x, destionation.y, destionation.z), 0.53f * Time.deltaTime);
        Vector3 relativePos = destionation - Player.transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, rotation, 2.0f * Time.deltaTime); ;
    }

    bool RotationCorrect()
    {
        if (Player.transform.rotation.y > Mathf.Deg2Rad * -56.4254f)
            return true;
        else
        {
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, Quaternion.Euler(0, 300, 0), 16.0f * Time.deltaTime); ;
            return false;
        }
    }

    void CheckIfEndMovement()
    {
        if (PlayerNode == EndNode)
        {
            Player.GetComponent<AnimationTrigger>().WalkingAnim(false);
            PlayerNode = EndNode;
            this.enabled = false;
            if(InteractableObject != null)
            {
                if (InteractableObject.InvokeEventHolder.Count > 0)
                    InteractableObject.InvokeEventHolder.Pop().Invoke(new StackData(InteractableObject, PlayerStats));
            }
        }
    }
}
