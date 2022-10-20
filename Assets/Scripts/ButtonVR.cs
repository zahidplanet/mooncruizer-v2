using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    
    public AudioSource ignitionSound;
    bool isPressed;
    public Rigidbody rb;
    public GameObject lS;

    private GameObject presser;


    private void Awake()
    {
        rb.isKinematic = true;
    }

    private void Start()
    {
        isPressed = false;
        lS = GameObject.FindGameObjectWithTag("LocomotionSystem");


    }

    //on awake, set rb to is kinematic
   

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.005f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
            //log button is pressed
            Debug.Log("Button is pressed!");
            //if rb is not kinematic set it to kinmeatic else leave it
           

        }
        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.02f, 0);
            onRelease.Invoke();
            isPressed = false;

        }
    }

    public void SwitchRigs()
    {

        //Debug.Log("Button is pressed!");
        
        //Finding Tagged XRRigs
        GameObject XRRig = GameObject.FindGameObjectWithTag("XRRig");
        GameObject RiderXRRig = GameObject.FindGameObjectWithTag("XRRigRider");

        //find object with tag XRRigOrigin
        GameObject XRRigOrigin = GameObject.FindGameObjectWithTag("XRRigOrigin");

      


        //if XRRig object is not a child of XRRigRider then align XRRig object with XRRigRider object and make child
        if (!XRRig.transform.IsChildOf(RiderXRRig.transform))
        {

            XRRig.transform.SetParent(RiderXRRig.transform);
            XRRig.transform.localPosition = Vector3.zero;
            XRRig.transform.localRotation = Quaternion.identity;
            rb.isKinematic = false;
            ignitionSound.Play();
            lS.SetActive(false);

        }
        //else align XRRig object with XRRigOrigin object and make child of XRRigOrigin object and move to the left
        else
        {
            //align XRRigOrigin object with RiderXRRig object
            XRRigOrigin.transform.position = RiderXRRig.transform.position;

            //MoveXRRigOrigin object 2f in local z-axis from current position
            XRRigOrigin.transform.Translate(-1f, 0, 0);
            rb.transform.localRotation = Quaternion.identity;
            rb.isKinematic = true;
            ignitionSound.Stop();
            lS.SetActive(true);

            //make XRRig object a child of XRRigOrigin object
            XRRig.transform.SetParent(XRRigOrigin.transform);
            XRRig.transform.localPosition = Vector3.zero;
            XRRig.transform.localRotation = Quaternion.identity;

            

        }


    }
}


