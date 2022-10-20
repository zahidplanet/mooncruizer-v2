using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class HopOnHopOffMotorcycle : MonoBehaviour
{
    
    public GameObject baseXRRig;
    public GameObject riderXRRig;
    public GameObject m_cruiser;
    public GameObject m_chopper;
    //public Rigidbody m_MooncruiserRB;

    public void Awake()
    {
        //get mooncruiser and cruiser rigidbody 

    }
    //when SwitchRigs is invoked, find XRRig object in the persistent scene
    //if the gameobject in the persistent scene with tag "XRRig" is active, deactivate it
    //activate the gameobject with tag "RiderXRRig" in the current scene
    //else activate the gameobject with tag "XRRig" in the persistent scene
    //and deactivate the gameobject with tag "RiderXRRig" in the current scene
    



    /*public void SwitchRigs(Scene currentScene, Scene persistentScene)
    {
      
        if (baseXRRig.activeSelf)
        {
            baseXRRig.SetActive(false);
            riderXRRig.SetActive(true);
            //if m_cruiser gameobject position and rotation are locked, unlock position and rotation
            if (m_cruiser.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezePositionX | m_cruiser.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezePositionY | m_cruiser.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezePositionZ | m_cruiser.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeRotationX | m_cruiser.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeRotationY | m_cruiser.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeRotationZ)
            {
                m_cruiser.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            //if m_chopper gameobject position and rotation are locked, unlock position and rotation
            if (m_chopper.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezePositionX | m_chopper.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezePositionY | m_chopper.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezePositionZ | m_chopper.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeRotationX | m_chopper.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeRotationY | m_chopper.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeRotationZ)
            {
                m_chopper.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }




        }
        else
        {
            baseXRRig.SetActive(true);
            riderXRRig.SetActive(false);
            //lock m_cruiser and m_chopper gameobjects positions and rotations
            m_cruiser.transform.position = new Vector3(0, 0, 0);
            m_cruiser.transform.rotation = new Quaternion(0, 0, 0, 0);
            m_chopper.transform.position = new Vector3(0, 0, 0);
            m_chopper.transform.rotation = new Quaternion(0, 0, 0, 0);



        }
    }*/


    public void LogInteractionStarted()
    {
        Debug.Log("Interaction started");
    }
    
    public void LogInteractionEnded()
    {
        Debug.Log("Interaction ended");
    }


    public void SwitchRigs()
    {
        GameObject XRRig = GameObject.FindGameObjectWithTag("XRRig");
        GameObject RiderXRRig = GameObject.FindGameObjectWithTag("XRRigRider");
        
        if (XRRig.activeSelf)
        {
            XRRig.SetActive(false);
            RiderXRRig.SetActive(true);
        }
        else
        {
            XRRig.SetActive(true);
            RiderXRRig.SetActive(false);
        }
    }


}
