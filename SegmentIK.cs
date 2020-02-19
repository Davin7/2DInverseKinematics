using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Davin Jimeno
// 2/15/2020

public class SegmentIK : MonoBehaviour
{
    // point to focus the armature
    public Vector2 branch;

    // fixed location of armature
    public Vector2 root;

    public Vector2 direction;

    private float myThickness;
    private float myLength;
    private Sprite mySprite;
    private float jointOffset;

    private void Awake()
    {
        mySprite = GetComponent<SpriteRenderer>().sprite;
        myLength = mySprite.rect.width / mySprite.pixelsPerUnit;
        myThickness = mySprite.rect.height / mySprite.pixelsPerUnit;
        jointOffset = myThickness;
    }

    private void calculateRoot()
    {
        direction = branch - root;
        //Debug.Log(name + "  angle between parent and current: " + ang + "   parent dir: " + parentDirection + "     child dir : " + direction);
        Vector2 dir = direction;
        dir.Normalize();
        dir = dir * myLength;
        dir = dir * -1;
        root = dir + branch;

    }

    //private void followTarget(Vector2 branch, Vector2 root)
    //{
    //    Vector2 loc = (branch + root) / 2;
    //    transform.position = loc;
    //}

    private void pointSegment(Vector2 branch, Vector2 root)
    {
        // Debug.Log(name + "  root location : " + root);
        // Debug.Log(name + "  control : " + branch);
        direction = branch - root;
        // Debug.Log(name + "  direction: " + dir);
        Quaternion curRot = Quaternion.FromToRotation(Vector2.right, direction);
        //Debug.Log(name + "  angle between parent and current: " + ang + "   parent dir: " + parentDirection + "     child dir : " + direction);
        transform.rotation = curRot;
    }

    private void calculateBranch(Vector2 parentRoot)
    {
        direction = branch - root;
        Vector2 dir = direction;
        dir.Normalize();

        branch = parentRoot + (dir * jointOffset);
    }
    public void inverseKinematicMovement(Vector2 parentRoot)
    {
       // Debug.Log(name + "  parent root: " + parentRoot);
        calculateBranch(parentRoot);
        calculateRoot();

        //followTarget(branch, root);
        pointSegment(branch, root);
    }

    // cartesian to polar step
    public void fixedKinematicStep(Vector2 fixedBase)
    {
        root = fixedBase; 
        direction = branch - root;
        Vector2 dir = direction;
        dir.Normalize();
        //Debug.Log(name + "  root fixed step: " + root);
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        float dx = myLength * Mathf.Cos(Mathf.Deg2Rad * angle);
        float dy = myLength * Mathf.Sin(Mathf.Deg2Rad * angle);
        //Debug.Log(name + "  angle fixed step: " + angle);
        //Debug.Log(name + "  dx: " + dx + "   dy: " + dy);
        branch = new Vector2(root.x + dx, root.y + dy);

        branch = branch - dir * jointOffset;
        //Debug.Log(name + "  branch fixed step: " + branch);
        transform.position = (branch + root) / 2;

    }


    public void applyVerticalPole(Vector2 axisIK)
    {
        //if ((branch.x > 0 && ang > 0.1) || (branch.x < 0 && ang < -0.1))

            // all ang need to be positive on left and negative on right
            // mirror parent's root along IK axis 
       // Debug.Log(name + "  applyVerticalPole  ");
        Vector2 dir = branch - root;
        //Debug.Log(name + "  dir: " + dir);
        float angle = Vector2.SignedAngle(axisIK, dir);
        float dy = myLength * Mathf.Sin(Mathf.Deg2Rad * angle);
        //Debug.Log(name + "  dy: " + dy);
        Vector2 dirNormal = Vector2.Perpendicular(axisIK).normalized * 2 * dy;
        //Debug.Log(name + "  dir normal: " + dirNormal);
        root = dirNormal * root;
    }
}
