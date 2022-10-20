using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public Transform m_steeringWheel;
    public Transform m_target;

    private Transform m_hand;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_hand = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_hand = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_hand)
        {
            m_target.position = m_hand.position;
            m_target.localPosition = new Vector3(m_target.localPosition.x, 0, m_target.localPosition.z);

            Vector3 dir = m_target.position - m_steeringWheel.position;

            Quaternion rot = Quaternion.LookRotation(dir, transform.up);

            m_steeringWheel.rotation = rot;
        }
    }
}
