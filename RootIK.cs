using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Davin Jimeno
// 2/15/2020

//defines the base location for the child SegmentIKs
// segmentIK connect base point root to outer point branch
public class RootIK : MonoBehaviour
{ 

    // Top segment of armature heirarchy
    public SegmentIK seg;

    // game object to point armature
    public GameObject control;

    public int numSegments;

    public int numPasses;
    
    public Vector2 rootLocation;
    
    // fixed location of armature
    private Vector2 armatureRoot;

    private SegmentIK[] armature;


    void Start()
    {
        armature = new SegmentIK[numSegments];
        //segLen = seg.getLength();
        //calculateBranch();
        //calculateRoot();
        buildArmature(seg);
        armatureRoot = rootLocation;
    }


    // Runs every frame
    void Update()
    {

        // Loop for number of passes required by prefab
        for (int p = 1; p <= numPasses; p++)
        {
            // Rotate segments following controlIK point
            armature[0].inverseKinematicMovement(control.transform.position);

            for (int k = 1; k < armature.Length; k++)
            {
                armature[k].inverseKinematicMovement(armature[k - 1].root);
            }

            // Use verticle vector pole to check if in-between segment angles are consistent
            // if they are not, mirror the pairs of segments over the IKAxis
            for (int s = 0; s <= armature.Length - 2; s++)
            {
                float ang = Vector2.SignedAngle(armature[s].direction, armature[s + 1].direction);
                if ((control.transform.position.x > 0 && ang < -0.1) || (control.transform.position.x < 0 && ang > 0.1))
                {
                    Vector2 axisIK = armature[s].branch - armature[s + 1].root;
                    //Debug.Log(armature[s].name + "  axisIK: " + axisIK);
                    armature[s].applyVerticalPole(axisIK);
                    armature[s + 1].inverseKinematicMovement(armature[s].branch);
                }
            }

            // Position the armature using the root and propagate the position up the
            // armature
            armature[armature.Length - 1].fixedKinematicStep(armatureRoot);
            for (int f = armature.Length - 2; f >= 0; f--)
            {
                armature[f].fixedKinematicStep(armature[f + 1].branch);
            }
        }
    }
    
    // loop through children segments of armature and build armature segment array
    private void buildArmature(SegmentIK topSeg)
    {
        SegmentIK curSeg = topSeg;
        int i = 0;
        while(true)
        {
            Debug.Log(name + "  curseg " + curSeg.name);
            armature[i] = curSeg;
            try
            {
                curSeg = armature[i].transform.GetChild(0).GetComponent<SegmentIK>();

            }
            catch
            {
                break;
            }
            i++;
        }
    }
}
