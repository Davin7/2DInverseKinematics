using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Davin Jimeno
// 2020

//defines the base location for the child SegmentIKs
// segmentIK connect base point root to outer point branch
public class RootIK : MonoBehaviour
{ 

    // Top segment of armature heirarchy
    public SegmentIK seg;

    // game object for armature to follow
    public GameObject control;

    // number of segments in the armature
    public int numSegments;

    // fixed location of armature
    private Vector2 armatureRoot;

    private SegmentIK[] armature;

    // Specify number of passes of Inversekinematics on the armature
    private const int NumPasses = 20;

    void Start()
    {
        armature = new SegmentIK[numSegments];
        //segLen = seg.getLength();
        //calculateBranch();
        //calculateRoot();
        buildArmature(seg);
        armatureRoot = Vector2.zero;
    }

    // Runs every frame
    void Update()
    {

        // get branch location on target plus segment half width offset
       // calculateBranch();
       // calculateRoot();

       // Debug.Log(name + "  branch :" + branch);
        //Debug.Log(name + "  root :" + root);
        // place segment between root and branch
        // followTarget(seg, branch, root);
        // pointSegment(seg, branch, root);

        // Loop for number of passes required by prefab
        for (int p = 1; p <= NumPasses; p++)
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
                    Debug.Log(armature[s].name + "  axisIK: " + axisIK);
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
    /*
    private void calculateRoot()
    {
        Vector2 dir = branch - root;
        dir.Normalize();
        dir = dir * segLen;
        dir = dir * -1;

        root = dir + branch;
    }*/
    /*
    private void followTarget(SegmentIK seg, Vector2 branch, Vector2 root)
    {
        Vector2 loc = (branch + root) / 2;
        seg.transform.position = loc;
    }
    private void pointSegment(SegmentIK seg, Vector2 branch, Vector2 root)
    {
        Debug.Log(name + "  root location : " + root);
        Debug.Log(name + "  control : " + branch);
        Vector2 dir = branch - root;
        seg.gameObject.transform.rotation = Quaternion.FromToRotation(Vector2.right, dir);

    }*/
    /*
    private void calculateBranch()
    {
        branch.Set(control.transform.position.x, control.transform.position.y);
    }*/

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
