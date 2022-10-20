using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRigs : MonoBehaviour
{
    public Rigidbody rb;

    private void Awake()
    {
        rb.isKinematic = true;
    }

    public void HopOnHopOff()
    {
        GameObject XRRig = GameObject.FindGameObjectWithTag("XRRig");
        GameObject RiderXRRig = GameObject.FindGameObjectWithTag("XRRigRider");
        //find object with tag XRRigOrigin
        GameObject XRRigOrigin = GameObject.FindGameObjectWithTag("XRRigOrigin");
        GameObject lS = GameObject.FindGameObjectWithTag("LocomotionSystem");

        //if XRRig object is not a child of XRRigRider then align XRRig object with XRRigRider object and make child
        if (!XRRig.transform.IsChildOf(RiderXRRig.transform))
        {
            XRRig.transform.SetParent(RiderXRRig.transform);
            XRRig.transform.localPosition = Vector3.zero;
            XRRig.transform.localRotation = Quaternion.identity;
            rb.isKinematic = false;
            //lS.SetActive(false);
        }
        //else align XRRig object with XRRigOrigin object and make child of XRRigOrigin object and move to the left
        else
        {
            //align XRRigOrigin object with RiderXRRig object
            XRRigOrigin.transform.position = RiderXRRig.transform.position;

            //MoveXRRigOrigin object 2f in local z-axis from current position
            XRRigOrigin.transform.Translate(2f, 0, 0);
            rb.transform.localRotation = Quaternion.identity;

            //make XRRig object a child of XRRigOrigin object
            XRRig.transform.SetParent(XRRigOrigin.transform);
            XRRig.transform.localPosition = Vector3.zero;
            XRRig.transform.localRotation = Quaternion.identity;
            //FIX SWITCH SO THAT LOCOMOTION SYSTEM TURNS BACK ON WHEN YOU EXIT THE VEHICLE
            //lS.SetActive(true);


        }
    }

}
