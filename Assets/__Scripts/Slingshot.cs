﻿using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{

    static private Slingshot S;

    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;
    private LineRenderer band;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS {                                        // b
        get {
            if (S == null ) return Vector3.zero;
            return S.launchPos;
        }
    }
    void Awake(){
         S = this; 

        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);

        band = GetComponent<LineRenderer>();
        band.enabled = true;

        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter(){

        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);

    }

    void OnMouseExit(){

        //print("Slingshot:OnMouseExit()");

        launchPoint.SetActive(false);
    }

    void OnMouseDown(){
        aimingMode = true;

        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;

        projectileRigidbody = projectile.GetComponent<Rigidbody>(); 
        projectileRigidbody.isKinematic = true;

        print("Drawing Band");
        band.SetPosition(1, projectile.transform.position);

    }

    void Update(){

        if( projectile != null)
            band.SetPosition(1, projectile.transform.position);
        else band.SetPosition(1, launchPoint.transform.position);

        if(!aimingMode)return;

        Vector3 mousePos2D = Input.mousePosition;                                  // c
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D );

        // Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D-launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider          // d
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        
        if ( Input.GetMouseButtonUp(0) ) {                                         // e
            // The mouse has been released

            print("Band Released");


            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            
            FollowCam.POI = projectile;

            projectile = null;
            
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile; 

        }


    }
    


}
