using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class WaypointControl : MonoBehaviour
{
    public UnityEvent onEnter;
    public GameObject xrRig;


    // Start is called before the first frame update
    void Start()
    {
        xrRig = GameObject.FindGameObjectWithTag("XRRig");

    }

    private void OnTriggerEnter(Collider collider)
    {
        collider = xrRig.GetComponent<Collider>();

        if (collider.gameObject.tag == "XRRig")
        {
            Destroy(this.gameObject);
            onEnter.Invoke();
        }
        
    }

    public void Appear(GameObject waypoint)
    {
        if (!isActiveAndEnabled)
        {
            gameObject.SetActive(true);
        }

        else
        {
            return;
        }
    }

}
