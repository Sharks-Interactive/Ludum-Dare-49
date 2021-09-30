using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Transform/Mirror")]
public class MirrorTransform : MonoBehaviour
{
    [Header("References")]
    public Transform ObjectToMirror;
    [Tooltip("Leave null to use the attached transform.")]
    public Transform ObjectToMirrorTo;

    [Header("Mirror Settings")]
    [Tooltip("Sets whether or not the mirror Transform should be applied.")]
    public bool Enabled = true;

    [Header("Position")]
    [Tooltip("Set an axis to 1 to enable, 0 to disable.")]
    public Vector3 ActivePosAxis;
    [Tooltip("Whether the transformations are applied in local or world space.")]
    public bool LocalPosSpace;

    [Header("Rotation")]
    [Tooltip("Set an axis to 1 to enable, 0 to disable.")]
    public Quaternion ActiveRotAxis;
    [Tooltip("Whether the transformations are applied in local or world space.")]
    public bool LocalRotSpace;

    [Header("Scale")]
    [Tooltip("Set an axis to 1 to enable, 0 to disable.")]
    public Vector3 ActiveScaleAxis;

    [Header("Offsets")]
    [Tooltip("Sets whether or not the object should be offset from the center of the targert OBJ.")]
    public bool AllowPosOffset;

    [Tooltip("The positional offset of the object.")]
    public Vector3 PosOffset;

    [Tooltip("Sets whether or not the object's rotation should be offset from the object.")]
    public bool AllowRotOffset;

    [Tooltip("The rotational offset of the object.")]
    public Quaternion RotOffset;

    //Cache
    private Vector3 Pos;
    private Vector3 Scale;
    private Quaternion Rot;

    /// <summary>
    /// Initiliazation
    /// </summary>
    void Start()
    {
        if (ObjectToMirrorTo == null)
            ObjectToMirrorTo = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Enabled)
            return;

        //Create transformations
        Pos.Set(
            ObjectToMirror.position.x * ActivePosAxis.x +
            (ObjectToMirrorTo.position.x * (ActivePosAxis.x - 1) * -1) +
            PosOffset.x,
            ObjectToMirror.position.y * ActivePosAxis.y +
            (ObjectToMirrorTo.position.y * (ActivePosAxis.y - 1) * -1) +
            PosOffset.y,
            ObjectToMirror.position.z * ActivePosAxis.z +
            (ObjectToMirrorTo.position.z * (ActivePosAxis.z - 1) * -1) +
            PosOffset.z);
        Scale.Set(
            ObjectToMirror.localScale.x * ActiveScaleAxis.x + 
            (ObjectToMirrorTo.localScale.x * (ActiveScaleAxis.x - 1) * -1), 
            ObjectToMirror.localScale.y * ActiveScaleAxis.y + 
            (ObjectToMirrorTo.localScale.y * (ActiveScaleAxis.y - 1) * -1), 
            ObjectToMirror.localScale.z * ActiveScaleAxis.z + 
            (ObjectToMirrorTo.localScale.z * (ActiveScaleAxis.z - 1) * -1));
        Rot.Set(
            ObjectToMirror.rotation.x * ActiveRotAxis.x + 
            (ObjectToMirrorTo.rotation.x * (ActiveRotAxis.x - 1) * -1), 
            ObjectToMirror.rotation.y * ActiveRotAxis.y + 
            (ObjectToMirrorTo.rotation.y * (ActiveRotAxis.y - 1) * -1), 
            ObjectToMirror.rotation.z * ActiveRotAxis.z + 
            (ObjectToMirrorTo.rotation.z * (ActiveRotAxis.x - 1) * -1), 
            ObjectToMirror.rotation.w * ActiveRotAxis.z + 
            (ObjectToMirrorTo.rotation.w * (ActiveRotAxis.z - 1) * -1));

        //Apply transformations
        if (LocalPosSpace)
            ObjectToMirrorTo.localPosition = Pos;
        else
            ObjectToMirrorTo.position = Pos;

        if (LocalRotSpace)
            ObjectToMirrorTo.localRotation = Rot;
        else
            ObjectToMirrorTo.rotation = Rot;

        ObjectToMirrorTo.localScale = Scale;
    }
}
