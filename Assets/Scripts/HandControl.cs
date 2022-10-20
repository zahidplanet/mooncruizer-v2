using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HandControl : MonoBehaviour
{
    public static void AlignHandToInteractableAttachment(Transform handroot, Transform handGrabAttach, Transform interactableAttach)
    {
        handroot.rotation = interactableAttach.rotation * Quaternion.Inverse(handGrabAttach.localRotation);
        handroot.position = interactableAttach.position + (handroot.position - handGrabAttach.position);
    }

    public bool hideHand = false;
    public bool disableCollisions = false;
    public HandPose leftHandPose = null;
    public HandPose rightHandPose = null;
    public Transform fixedAttachment;

    public HandPose GetPoseForHand(HandType hand)
    {
        switch (hand)
        {
            case HandType.Left: return leftHandPose;
            case HandType.Right: return rightHandPose;
            default: return null;
        }
    }

}
