using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.XR.Provider;
using UnityEngine.SceneManagement;

namespace Gadd420
{

    public class XRI_Manager : MonoBehaviour
    {
        //public XRNode m_rightControllerPos;
        public ActionBasedController m_leftController;
        public ActionBasedController m_rightController;
        public bool combineLeanAndSteering;
        public bool combineBrakeInputs;
        
        public float inputSmoothingTime = 0.5f;

        float vInputTime;
        float hzInputTime;

        //A&D
        protected float hzInput;
        public float HzInput
        {
            get { return hzInput; }
        }

        //W&S
        protected float vInput;
        public float VInput
        {
            get { return vInput; }
        }

        //LMouse & RMouse
        protected float leanInput;
        public float LeanInput
        {
            get { return leanInput; }
        }

        //LShift && LCtrl
        protected float wheelieInput;
        public float WheelieInput
        {
            get { return wheelieInput; }
        }

        //FrontBreak
        protected float frontBreakInput;
        public float FrontBreakInput
        {
            get { return frontBreakInput; }
        }

        private void Start()
        {
            vInput = 0;
            hzInput = 0;
            //m_leftController = find game object in persistent scene with tag "LeftController
            //m_rightController = find game object in persistent scene with tag "RightController
            m_leftController = GameObject.FindGameObjectWithTag("LeftController").GetComponent<ActionBasedController>();
            m_rightController = GameObject.FindGameObjectWithTag("RightController").GetComponent<ActionBasedController>();
        }

        void Update()
        {
            VerticalInput();
            HZInput();
            GetLeanValue();
            //GetLeanBackValue();
            FrontBreak();
        }



        protected virtual void VerticalInput()
        {
            vInputTime = Mathf.Clamp(vInputTime, 0, inputSmoothingTime);

            if (m_rightController.activateAction.action.ReadValue<float>() > 0.0f)
            {
                float duration = 1f;
                HapticController.SendHaptics(m_rightController, 0.5f, duration);
                HapticController.SendHaptics(m_leftController, 0.5f, duration);

                if (vInput < 0)
                {
                    vInputTime -= 2 * Time.deltaTime;
                    vInput = -Mathf.InverseLerp(0, inputSmoothingTime, vInputTime);
                }
                else
                {
                    vInputTime += 1 * Time.deltaTime;
                    vInput = Mathf.InverseLerp(0, inputSmoothingTime, vInputTime);
                }

            }

            else
            {
                //duration is null
                HapticController.SendHaptics(m_rightController, 0.0f, 0.0f);
                HapticController.SendHaptics(m_leftController, 0.0f, 0.0f);

                if (vInputTime > 0.01f)
                {
                    vInputTime -= 1 * Time.deltaTime;
                    if (vInput < 0)
                    {
                        vInput = -Mathf.InverseLerp(0, inputSmoothingTime, vInputTime);
                    }
                    if (vInput > 0)
                    {
                        vInput = Mathf.InverseLerp(0, inputSmoothingTime, vInputTime);
                    }
                }
                else
                {
                    vInputTime = 0;
                    vInput = 0;
                }
            }
        }

        protected virtual void HZInput()
        {




            //////THIS IS THE XR INPUT CODE

            hzInputTime = Mathf.Clamp(hzInputTime, 0, inputSmoothingTime);




            //get the y position across the local y-axis for the right XR controller
            float rightControllerY = m_rightController.transform.localPosition.y;

            //get the y position across the local y-axis for the left XR controller
            float leftControllerY = m_leftController.transform.localPosition.y;

            

            //debug the right controller y position
            // Debug.Log("right controller y position is" + rightControllerY);
            //debug the left controller y position
            //Debug.Log("left controller y position is" + leftControllerY);


            //if the rightController Y position is less than -0.2 turn right, if the left controller Y position is less than -0.2 turn left, if left and right y positions are less than -0.55 reset position of bike 
            if (rightControllerY < -0.2f)
            {
                hzInputTime += 1 * Time.deltaTime;
                hzInput = Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            }
            else if (leftControllerY < -0.2f)
            {
                hzInputTime += 1 * Time.deltaTime;
                hzInput = -Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            }
            else if (rightControllerY > -0.55f && leftControllerY > -0.55f)
            {
                hzInputTime = 0;
                hzInput = 0;
            }
            else
            {
                hzInputTime = 0;
                hzInput = 0;
            }
            
            
            



            //////THIS IS OLD CODE
            //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            //{
            //    if (Input.GetKey(KeyCode.D))
            //    {
            //
            //        if (hzInput < 0)
            //       {
            //            hzInputTime -= 2 * Time.deltaTime;
            //            hzInput = -Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            //        }
            //        else
            //        {
            //            hzInputTime += 1 * Time.deltaTime;
            //            hzInput = Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            //        }
            //    }
            //    if (Input.GetKey(KeyCode.A))
            //    {
            //        if (hzInput > 0.01f)
            //        {
            //            hzInputTime -= 2 * Time.deltaTime;
            //            hzInput = Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            //        }
            //        else
            //        {
            //            hzInputTime += 1 * Time.deltaTime;
            //            hzInput = -Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            //        }
            //    }
            //}

            //else
            //{
            //    if (hzInputTime > 0.01f)
            //    {
            //        hzInputTime -= 1 * Time.deltaTime;
            //        if (hzInput < 0)
            //        {
            //            hzInput = -Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            //        }
            //        if (hzInput > 0)
            //        {
            //            hzInput = Mathf.InverseLerp(0, inputSmoothingTime, hzInputTime);
            //        }
            //    }
            //    else
            //    {
            //        hzInputTime = 0;
            //        hzInput = 0;
            //    }
            //}

        }

        //KEYBOARD INPUTS
        protected virtual void GetLeanValue()
        {

            if (!combineLeanAndSteering)
            {
                if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        leanInput = -1;
                    }
                    if (Input.GetKey(KeyCode.Mouse1))
                    {
                        leanInput = 1;
                    }
                }
                else
                {
                    leanInput = 0;
                }
            }
            else
            {
                leanInput = hzInput;
            }
        }

        protected virtual void GetLeanBackValue()
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    wheelieInput = -1;
                }
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    wheelieInput = 1;
                }
            }
            else
            {
                wheelieInput = 0;
            }
        }




        //XR LEAN INPUTS
        //protected virtual void GetLeanValue()
        //{
        //    float rightRotationY = m_rightController.rotateAnchorAction.action.ReadValue<float>();
        //    float leftRotationY = m_leftController.rotateAnchorAction.action.ReadValue<float>();

        //    if (!combineLeanAndSteering)
        //    {
        //        if (leftRotationY < 0 || rightRotationY < 0)
        //        {
        //            if (leftRotationY < 0)
        //            {
        //                leanInput = -1;
        //            }
        //            if (rightRotationY < 0)
        //            {
        //                leanInput = 1;
        //            }
        //        }
        //        else
        //        {
        //            leanInput = 0;
        //        }
        //    }
        //    else
        //    {
        //        leanInput = hzInput;
        //    }
        //}

        //protected virtual void GetLeanBackValue()
        //{
        //    float selectAction = m_leftController.selectAction.action.ReadValue<float>();
        //    float uIAction = m_leftController.uiPressAction.action.ReadValue<float>();
        //    if ( selectAction > 0 || uIAction > 0)
        //    {
        //        if (selectAction > 0)
        //        {
        //            wheelieInput = -1;
        //        }
        //        if (uIAction > 0)
        //        {
        //            wheelieInput = 1;
        //        }
        //    }
        //    else
        //    {
        //        wheelieInput = 0;
        //    }
        //}



        protected virtual void FrontBreak()
        {
            if (m_leftController.activateAction.action.ReadValue<float>() > 0.0f)
            {
                //Debug.Log("Brake");
                frontBreakInput = 1;
            }
            else
            {
                frontBreakInput = 0;
            }
        ///    Keyboard Brake Inputs
        ///    if (Input.GetKey(KeyCode.Space))
        ///    {
        ///        frontBreakInput = 1;
        ///    }
        ///    else
        ///    {
        ///        frontBreakInput = 0;
        ///    }
        }
    }
}
