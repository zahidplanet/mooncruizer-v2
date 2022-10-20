using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public AudioSource dink;
    public GameObject optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_leftController.uiPressAction.action.ReadValue<float>() > 0.0f)
        //{
        //    Debug.Log("Menu Button Pressed!");
        //    bool isActive = optionsMenu.activeSelf;

        //    optionsMenu.SetActive(!isActive);
        //}
    }

    public void ToggleOnOff()
    {
        //dink.Play();
        bool isActive = optionsMenu.activeSelf;

        optionsMenu.SetActive(!isActive);
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}
