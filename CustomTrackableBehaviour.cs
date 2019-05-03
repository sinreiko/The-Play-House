/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class CustomTrackableBehaviour : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES
    [SerializeField] GameObject mainObstacle; //main Obstacle
    [SerializeField] private GameObject obstacle; //prefab

    private TrackableBehaviour mTrackableBehaviour;
    private TrackableBehaviour.Status m_PreviousStatus;
    private TrackableBehaviour.Status m_NewStatus;
    public GameObject scanner;
    private InstructionManager m_InsManager;
    private SceneController m_SceneController;

    private ImageTargetBehaviour m_ImageTargetBehaviour;
    private PositionalDeviceTracker m_PositionDeviceTracker;
    private DeviceTracker m_DeviceTracker;
    private Vector3 objectPosition;
    private bool IsTracking, IsExtendedTracking;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        m_DeviceTracker = TrackerManager.Instance.InitTracker<PositionalDeviceTracker>();
        m_InsManager = FindObjectOfType<InstructionManager>();
        m_SceneController = FindObjectOfType<SceneController>();
        m_ImageTargetBehaviour = GetComponent<ImageTargetBehaviour>();
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        mainObstacle = GameObject.FindGameObjectWithTag("Obstacle");
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    /// 
    public void Update()
    {
        Debug.Log("IsTracking: " + IsTracking);
        Debug.Log("IsExtending: " + IsExtendedTracking);
        if (IsTracking) OnTrackingFound();
        if (IsTracking && m_SceneController.gameStart)
        {
            //objectPosition = mainObstacle.transform.localPosition;
            m_DeviceTracker.Start();
            //Destroy(mainObstacle);
            //Transform img = gameObject.GetComponent<Transform>();
            //gameObject.transform.position = new Vector3(0, 0, 0);
            //mainObstacle = Instantiate(obstacle, img, false);
            //mainObstacle.transform.position = objectPosition;
            //if (m_ImageTargetBehaviour.enabled == true)
                //m_ImageTargetBehaviour.enabled = false;
        }

        if (IsExtendedTracking && m_SceneController.gameReset)
        {
            m_DeviceTracker.Stop();

        }
    }
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            IsTracking = true;
            scanner.SetActive(false);
            if (!m_SceneController.gameReset) OnTrackingFound();
            if (m_SceneController.sceneName == "Tutorial")
            {
                if (mTrackableBehaviour.TrackableName == "AR3_BW" && m_InsManager.ins.Length == 6)
                {
                    m_InsManager.Next();
                }
            }
        } else if (newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED){
            Debug.Log("Extended tracking is on");
            IsExtendedTracking = true;
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
            m_InsManager.allowTap = false;
            if (scanner == null)
            {
                scanner.SetActive(true);
            }
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            IsExtendedTracking = false;
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PROTECTED_METHODS
}
