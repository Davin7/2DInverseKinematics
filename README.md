# 2DInverseKinematics
2D Inverse Kinematics system supporting polarization without raycasting

This Unity 2D inverse kinematics system uses the following prefab heirarchy. Each [item] represents a Unity gameobject with
indented gameobjects as transform.children of predecessor gameobjects.

[Armature]

-----[Root]
              
----------[Segment0]

---------------[Segment1]

--------------------[...]

-------------------------[SegmentN]

-----[Control]
    
The Armature is an empty gameobject that acts as a "folder" for its children, Root and Control. 

The scripts are attached to the gameobjects as follows:

* The RootIK script is attached to the Root gameobject.
* The ControlIK script is attached to the Control gameobject.
* The SegmentIK script is attached to the Segment gameobjects.

In the Unity Editor, each script will have the following public parameters to set for the 2D inverse kinematics prefab that you are building:

RootIK:
* seg: the top segment of the armature heirarchy, in the above example, Segment0
* control: the control object the inverse kinematics armature will follow, in the above example, Control
* numSegments: the number of segment gameobjects the RootIK script will "dig" through and add to the Root's List<Segment> armature
    
SegmentIK:
* The public variables in SegmentIK do not need to be edited in the Unity Editor

ControlIK:
* This script has no public variables and is used to identify 

