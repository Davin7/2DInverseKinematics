# 2DInverseKinematics
2D Inverse Kinematics system supporting polarization without raycasting

This Unity 2D inverse kinematics system uses the following prefab heirarchy. Each [item] represents a Unity gameobject with
indented gameobjects as transform.children of predecessor gameobjects.

[Armature]
    [Root]
        [Segment0]
            [Segment1]
                ...
                    [SegmentN]
    [Control]
    
The Armature is an empty gameobject that acts as a "folder" for its children, Root and Control. 

The scripts are attached to the gameobjects as follows:

-The RootIK script is attached to the Root gameobject.
-The ControlIK script is attached to the Control gameobject.
-The SegmentIK script is attached to the Segment gameobjects.

