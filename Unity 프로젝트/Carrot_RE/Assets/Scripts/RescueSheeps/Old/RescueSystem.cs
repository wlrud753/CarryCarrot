using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueSystem : MonoBehaviour
{
    Camera MainCam;

    Vector3 MousePos;
    RaycastHit2D rayHit;

    bool Rescuing;
    Sheep sheep; string sheepName;

    public float RescueTime;
    float rescueTimer;

    List<Wolf> wolves;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;

        RescueTime = 2f;
        rescueTimer = 0;

        wolves = new List<Wolf>();
        setWolves();

        resetVar();
    }

    void setWolves()
    {
        Transform WolvesTR = GameObject.Find("Wolves").transform;

        for(int i = 0; i < WolvesTR.childCount; i++)
        {
            wolves.Add(WolvesTR.GetChild(i).GetComponent<Wolf>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MousePos = MainCam.ScreenToWorldPoint(Input.mousePosition);

            rayHit = Physics2D.Raycast(MousePos, transform.forward);

            if (rayHit)
            {
                if (rayHit.collider.tag == "Sheep")
                {
                    if(sheepName == null)
                    {
                        Rescuing = true;

                        sheep = rayHit.collider.GetComponent<Sheep>();
                        sheepName = sheep.name;
                    }

                    if (rayHit.collider.name == sheepName)
                    {
                        Rescue();
                    }
                    else
                    {
                        StopRescue();
                    }

                    
                    //CheckWolvesTarget();
                }
                else if (Rescuing)
                {
                    StopRescue();
                }
            }
        }
    }

    bool halfTimer;
    void Rescue()
    {
        rescueTimer += Time.deltaTime;

        if(!halfTimer && rescueTimer >= RescueTime / 2)
        {
            halfTimer = true;
            sheep.Rescuing();
            ChangeWolvesTarget();
        }

        if(rescueTimer >= RescueTime)
        {
            sheep.Kill();
            ChangeWolvesTarget();
            // 보상
            Debug.Log("Success Rescue");

            resetVar();
        }
    }
    void StopRescue()
    {
        sheep.StopRescue();

        resetVar();
    }

    void ChangeWolvesTarget()
    {
        for(int i = 0; i < wolves.Count; i++)
        {
            if(wolves[i].getHasTarget())
                wolves[i].ChangeTarget(sheep.name);
        }
    }


    void resetVar()
    {
        Rescuing = false;
        sheepName = null;
        rescueTimer = 0;
        halfTimer = false;
    }
}