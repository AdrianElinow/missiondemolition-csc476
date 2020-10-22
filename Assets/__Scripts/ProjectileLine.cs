using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour {  

    static public ProjectileLine S; // Singleton

    [Header("Set in Inspector")]
    public float                minDist = 0.1f;

    private LineRenderer       line;
    private GameObject         _poi;
    private List<Vector3>      points;

    private List<Vector3>[] lines;
    private int lineIndex;

    void Awake() {

        S = this;// Set the singleton
        // Get a reference to the LineRenderer
        line = GetComponent<LineRenderer>();
        // Disable the LineRenderer until it's needed
        line.enabled = false;
        // Initialize the points List

        lines = new List<Vector3>[3];
        lineIndex = 0;
    }

    // This is a property (that is, a method masquerading as a field)
    public GameObject poi {

        get {
            return ( _poi );
        } set {

            _poi = value;

            if ( _poi != null ) {

                // When _poi is set to something new, it resets everything
                line.enabled = false;
                lines[lineIndex] = new List<Vector3>();
                AddPoint();

            }

        }

    }

    // This can be used to clear the line directly

    public void Clear() {

        _poi = null;
        line.enabled = false;
        
        if( lineIndex + 1 > 2 )
            lineIndex = 0;

        lines[lineIndex] = new List<Vector3>();

    }


    public void AddPoint() {

        // This is called to add a point to the line

        Vector3 pt = _poi.transform.position;

        if ( lines[lineIndex].Count > 0 && (pt - lastPoint).magnitude < minDist ) {

            // If the point isn't far enough from the last point, it returns
            return;
        }

        if ( lines[lineIndex].Count == 0 ) { // If this is the launch point...

            Vector3 launchPosDiff = pt -Slingshot.LAUNCH_POS; // To be defined
            // ...it adds an extra bit of line to aid aiming later
            lines[lineIndex].Add( pt + launchPosDiff );
            lines[lineIndex].Add(pt);
            line.positionCount = 2;
            // Sets the first two points
            line.SetPosition(0, lines[lineIndex][0] );
            line.SetPosition(1, lines[lineIndex][1] );
            // Enables the LineRenderer
            line.enabled = true;

        } else {

            // Normal behavior of adding a point
            lines[lineIndex].Add( pt );
            line.positionCount = lines[lineIndex].Count;
            line.SetPosition( lines[lineIndex].Count-1, lastPoint );
            line.enabled = true;

        }

    }



    // Returns the location of the most recently added point

    public Vector3 lastPoint {
        get {
            if (lines[lineIndex] == null ) {
                // If there are no points, returns Vector3.zero
                return ( Vector3.zero );
            }
            return ( lines[lineIndex][lines[lineIndex].Count-1] );
        }

    }

    void FixedUpdate () {

        if ( poi == null ) {
            // If there is no poi, search for one
            if (FollowCam.POI != null ) {

                if (FollowCam.POI.tag == "Projectile") 
                    poi = FollowCam.POI;
                else return; // Return if we didn't find a poi
                
            } else return; // Return if we didn't find a poi
            

        }

        // If there is a poi, it's loc is added every FixedUpdate
        AddPoint();
        if (FollowCam.POI == null ) {
            // Once FollowCam.POI is null, make the local poi nulll too
            poi = null;
        }

    }

}