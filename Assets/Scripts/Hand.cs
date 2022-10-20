using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Animations.Rigging;

public enum HandType
{
    Left, 
    Right
};

public class Hand : MonoBehaviour
{
    public HandType type = HandType.Left;
    public bool isHidden { get; private set; } = false;

    public InputAction trackedAction = null;

    public InputAction gripAction = null;
    public InputAction triggerAction = null;
    public Animator handAnimator = null;
    int m_gripAmountParameter = 0;
    int m_pointAmountParameter = 0;

    bool m_isCurrentlyTracked = false;

    List<Renderer> m_currentRenderers = new List<Renderer>();

    Collider[] m_colliders = null;
    public bool isCollisionEnabled { get; private set; } = true;

    public XRBaseInteractor interactor = null;

    public bool enableDefaultHandAnimations = true;
    public bool hideOnTrackingLoss = true;
    public float poseAnimationSpeed = 10.0f;

    public GameObject handVisual = null;

    HandPose m_currentPose = null;
    Rig m_handPosingRig = null;
    float m_targetPoseWeight = 0.0f;
    Coroutine m_poseAnimationCoroutine = null;
    FingerPose[] m_fingers = null;
    Vector3 m_restorePosition = Vector3.zero;
    Quaternion m_restoreRotation = Quaternion.identity;
    Transform m_currentFixedAttachment = null;
    
    


    private void Awake()
    {
        if (interactor == null)
        {
            interactor = GetComponentInParent<XRBaseInteractor>();
        }

        if(m_handPosingRig == null)
        {
            m_handPosingRig = GetComponentInChildren<Rig>();
        }
        
    }

    private void OnEnable()
    {
        interactor?.onSelectEntered.AddListener(OnGrab);
        interactor?.onSelectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        interactor?.onSelectEntered.RemoveListener(OnGrab);
        interactor?.onSelectExited.RemoveListener(OnRelease);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        trackedAction.Enable();
        m_gripAmountParameter = Animator.StringToHash("GripAmount");
        m_pointAmountParameter = Animator.StringToHash("PointAmount");
        gripAction.Enable();
        triggerAction.Enable();

        if (m_handPosingRig != null) m_handPosingRig.weight = 0.0f;
        m_fingers = GetComponentsInChildren<FingerPose>();

        if (hideOnTrackingLoss) Hide();
    }

    void UpdateAnimations()
    {
        float pointAmount = triggerAction.ReadValue<float>();
        handAnimator.SetFloat(m_pointAmountParameter, enableDefaultHandAnimations ? pointAmount: 0.0f);

        float gripAmount = gripAction.ReadValue<float>();
        handAnimator.SetFloat(m_gripAmountParameter, enableDefaultHandAnimations ? Mathf.Clamp01(gripAmount + pointAmount) : 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float isTracked = trackedAction.ReadValue<float>();
        if(isTracked == 1.0f && !m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = true;
            Show();
        }
        else if(isTracked == 0 && m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = false;
            if(hideOnTrackingLoss) Hide();
        }
        UpdateAnimations();
        SyncRigToPose();
    }

    public void Show()
    {
        foreach (Renderer renderer in m_currentRenderers)
        {
            renderer.enabled = true;
        }
        isHidden = false;
        EnableCollisions(true);
    }

    public void Hide()
    {
        m_currentRenderers.Clear();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            renderer.enabled = false;
            m_currentRenderers.Add(renderer);
        }
        isHidden = true;
        EnableCollisions(false);
    }

    public void EnableCollisions(bool enabled)
    {
        if (isCollisionEnabled == enabled) return;

        isCollisionEnabled = enabled;
        foreach(Collider collider in m_colliders)
        {
            collider.enabled = isCollisionEnabled;
        }
    }

    void OnGrab(XRBaseInteractable grabbedObject)
    {
        HandControl ctrl = grabbedObject.GetComponent<HandControl>();
        if(ctrl != null)
        {
            if (ctrl.disableCollisions)
            {
                EnableCollisions(false);
            }

            if (ctrl.hideHand)
            {
                Hide();
            }
            else
            {
                HandPose pose = ctrl.GetPoseForHand(type);
                if (pose != null)
                {
                    m_currentPose = pose;
                    if (ctrl.fixedAttachment != null)
                    {
                        m_currentFixedAttachment = ctrl.fixedAttachment;
                        m_restorePosition = handVisual.transform.localPosition;
                        m_restoreRotation = handVisual.transform.localRotation;
                    }

                    AnimateHandPoseWeightTo(1.0f);
                }
            }
        }
    }

    void OnRelease(XRBaseInteractable releasedObject)
    {
        HandControl ctrl = releasedObject.GetComponent<HandControl>();
        if (ctrl != null)
        {
            if (ctrl.disableCollisions)
            {
                EnableCollisions(true);
            }

            if (ctrl.hideHand)
            {
                Show();
            }
            else if (m_currentPose != null)
            {
                AnimateHandPoseWeightTo(0.0f);

                if(m_currentFixedAttachment != null)
                {
                    handVisual.transform.localPosition = m_restorePosition;
                    handVisual.transform.localRotation = m_restoreRotation;
                    m_currentFixedAttachment = null;
                }
                m_currentPose = null;
            }
        }
    }

    IEnumerator AnimateHand()
    {
        enableDefaultHandAnimations = false;
        while(!Mathf.Approximately(m_handPosingRig.weight, m_targetPoseWeight))
        {
            m_handPosingRig.weight = Mathf.MoveTowards(m_handPosingRig.weight, m_targetPoseWeight, poseAnimationSpeed * Time.deltaTime);
            yield return null;
        }
        if(Mathf.Approximately(m_targetPoseWeight, 0.0f))
        {
            enableDefaultHandAnimations = true;
        }
    }

    void AnimateHandPoseWeightTo(float targetWeight)
    {
        if(m_poseAnimationCoroutine != null)
        {
            StopCoroutine(m_poseAnimationCoroutine);
        }
        m_targetPoseWeight = targetWeight;
        m_poseAnimationCoroutine = StartCoroutine(AnimateHand());
    }
    
    void SyncTransform(Transform finger, Pose fingerPose)
    {

        Vector3 position = interactor.attachTransform.TransformPoint(fingerPose.position);
        Quaternion rotation = interactor.attachTransform.rotation * fingerPose.rotation;
        finger.SetPositionAndRotation(position, rotation);
    }

    void SyncRigToPose()
    {
        if (interactor == null || m_currentPose == null) return;
        if (interactor.attachTransform == null)
        {
            Debug.LogError("Interactor is missing an attach transform!");
        }

        if(m_currentFixedAttachment != null)
        {
            HandControl.AlignHandToInteractableAttachment(handVisual.transform, interactor.attachTransform, m_currentFixedAttachment);
        }


        foreach (var finger in m_fingers)
        {
            switch (finger.finger)
            {
                case FingerId.Thumb:
                    SyncTransform(finger.transform, m_currentPose.thumb);
                    break;
                case FingerId.Index:
                    SyncTransform(finger.transform, m_currentPose.index);
                    break;
                case FingerId.Middle:
                    SyncTransform(finger.transform, m_currentPose.middle);
                    break;
                case FingerId.Ring:
                    SyncTransform(finger.transform, m_currentPose.ring);
                    break;
                case FingerId.Pinky:
                    SyncTransform(finger.transform, m_currentPose.pinky);
                    break;
            }
        }
    }

    public void InteractionCleanup()
    {
        XRDirectInteractor directInterator = interactor as XRDirectInteractor;
        if (directInterator != null)
        {
            List<XRBaseInteractable> validTargets = new List<XRBaseInteractable>();
            interactor.GetValidTargets(validTargets);
            interactor.interactionManager.ClearInteractorSelection(interactor);
            interactor.interactionManager.ClearInteractorHover(interactor, validTargets);

            foreach (var target in validTargets)
            {
                if (target.gameObject.scene != interactor.gameObject.scene)
                {
                    target.transform.position = new Vector3(100, 100, 100);
                }
            }
        }
    }


}
