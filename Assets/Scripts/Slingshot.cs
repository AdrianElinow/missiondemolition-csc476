using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject launchpoint;

    void Awake(){
        Transform launchpointTrans = transform.Find("LaunchPoint");
        launchpoint = launchpointTrans.gameObject;
        launchpoint.SetActive(false);
    }

    void onMouseEnter(){

        print("Slingshot:OnMouseEnter()");
        launchpoint.SetActive(true);

    }

    void onMouseExit(){

        print("Slingshot:OnMouseExit()");
        launchpoint.SetActive(false);
    }


}
