using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    public bool isUIToggle;
    public bool isTutorial;
    public bool canChaneScene;
    public bool canChangeTutorial;

    public SnapTurnProvider stp;

    private InputDevice leftController;
    private InputDevice rightController;
    private InputDeviceCharacteristics leftControllerCharacteristics;
    private InputDeviceCharacteristics rightControllerCharacteristics;
    public GameObject endMessage;
    public Text endMessageText;
    public Logger l;

    public GameObject XRRig;
    public GameObject mainCamera;
    public GameObject rightControllerObject;
    private TeleportationProvider tp;
    private continuousMovement cm;
    
    public Rays r;

    private int movementType = 0;
    private int wireframeType = 0;

    private float[] rotation;
    private float[] position;

    private GoalLogic GL;
    private GameObject fog;
    public UIManager uiManager;

    private bool PrimaryButtonRightToggle;
    private bool SecondaryButtonRightToggle;
    private bool TriggerRightToggle;
    private bool PrimaryButtonLeftToggle;

    public ShaderChanger[] walls;
    public ShaderChanger floor;
    public GameObject ceilling;
    public GameObject teleportLine;

    private string[] notificationText = new string[] { "\tPlease take off your VR headset and complete the next page of the online form.\n\n\tWhen you are done put the VR headset back on and press A.",
                                                    "If you would like to play the tutorial again please press B. To continue press A",
    "If you would like to play the tutorial again please press B. To continue to the first part of the experiment press A",
    "\tPlease take off your VR headset and complete the next page of the online form.\n\n\tWhen you are done exit the application."};

       

    private void FindInputDevices()
    {
        List<InputDevice> devicesLeft = new List<InputDevice>();
        List<InputDevice> devicesRight = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devicesLeft);
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devicesRight);

        if (devicesLeft.Count > 0)
        {
            leftController = devicesLeft[0];

        }
        if (devicesRight.Count > 0)
        {
            rightController = devicesRight[0];

        }
    }

    public void activateEndMessage(int i)
    {
        endMessage.SetActive(true);
        endMessageText.text = notificationText[i];
    }

    public void deactivateEndMessage()
    {
        endMessage.SetActive(false);
    }

    private float[] GetRotation()
    {
        float rotationX = mainCamera.transform.rotation.x;
        float rotationY = mainCamera.transform.rotation.y;
        float rotationZ= mainCamera.transform.rotation.z;

        return new float[3] { rotationX, rotationY, rotationZ };
    }
    private float[] GetPosition()
    {
        float positionX = mainCamera.transform.position.x;
        float positionY = mainCamera.transform.position.y;
        float positionZ = mainCamera.transform.position.z;

        return new float[3] { positionX, positionY, positionZ };
    }

    // Start is called before the first frame update
    void Start()
    {
        l = FindObjectOfType<Logger>();
        endMessage = GameObject.FindGameObjectWithTag("endMessage");
        endMessage.SetActive(false);
        ceilling.SetActive(false);
        leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        FindInputDevices();

        rotation = new float[3];
        position = new float[3];

        tp = XRRig.GetComponent<TeleportationProvider>();
        cm = XRRig.GetComponent<continuousMovement>();

        cm.enabled = false;
        isTutorial = false;

        walls = GameObject.FindObjectsOfType<ShaderChanger>();

        fog = GameObject.FindGameObjectWithTag("Fog");
        fog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (stp.getDidRotation())
        {
            stp.setDidRotation(false);
            l.LogEvent("SR", "Current Rotation: "+stp.getCurrentTurnAmount().ToString());
        }
        if (leftController == null || rightController == null)
        {
            FindInputDevices();
        }

        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonRightValue);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonRightValue);
        rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerRightValue);
        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonLeftValue);

        // Press A to change movement type
        if (primaryButtonRightValue && !PrimaryButtonRightToggle && isUIToggle)
        {
            uiManager.ChangeMovementType();
            PrimaryButtonRightToggle = true;
            ChangeMovement();
        }
        if (!primaryButtonRightValue)
        {
            PrimaryButtonRightToggle = false;
        }

        if (primaryButtonRightValue && !PrimaryButtonRightToggle && !isUIToggle && canChaneScene)
        {
            // Next Scene
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            Debug.Log(nextSceneIndex);
            Debug.Log(SceneManager.sceneCount);
            if (4 > nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            PrimaryButtonRightToggle = true;
            
        }
        if (!primaryButtonRightValue)
        {
            PrimaryButtonRightToggle = false;
        }

        // Press A to progress tutorial
        if (primaryButtonRightValue && !PrimaryButtonRightToggle && isTutorial && canChangeTutorial)
        {
            Debug.Log("Hello");
            GL = FindObjectOfType<GoalLogic>();
            GL.progressTutorial();
            canChangeTutorial = false;
            PrimaryButtonRightToggle = true;
        }

        if (!primaryButtonRightValue)
        {
            PrimaryButtonRightToggle = false;
        }

        // Press B to replay tutorial
        if (secondaryButtonRightValue && !PrimaryButtonRightToggle && canChangeTutorial)
        {
            GL = FindObjectOfType<GoalLogic>();
            GL.ResetTutorial();
            canChangeTutorial = false;
            SecondaryButtonRightToggle = true;
        }

        if (!secondaryButtonRightValue)
        {
            SecondaryButtonRightToggle = false;
        }
        /*if (Input.GetKeyDown(KeyCode.A) && !isTutorial && canChaneScene)
        {
            // Next Scene
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            Debug.Log(nextSceneIndex);
            Debug.Log(SceneManager.sceneCount);
            if (4> nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && isTutorial && canChaneScene)
        {
            GL = FindObjectOfType<GoalLogic>();
            GL.progressTutorial();
        }

        if (Input.GetKeyDown(KeyCode.B) && isTutorial)
        {
            GL = FindObjectOfType<GoalLogic>();
            GL.ResetTutorial();
        }*/

        // Press B to change Wireframe Mode
        if (secondaryButtonRightValue && !SecondaryButtonRightToggle && isUIToggle)
        {
            uiManager.ChangeWireframeType();
            SecondaryButtonRightToggle = true;
            ChangeWireframe();


        }
        if (!secondaryButtonRightValue)
        {

            SecondaryButtonRightToggle = false;
        }

        // Press Right Trigger to select point
        // This should only trigger when pointer is enabled and teleporter is disabled
        if (triggerRightValue > 0 && !TriggerRightToggle && r.isPointer())
        {
            // Send data to Ray and finish the experiment;
            // Get ray value
            Ray ray = new Ray(rightControllerObject.transform.position, rightControllerObject.transform.forward);
            // Send it to ray.
            r.SetPointValue(ray.direction);
            l.LogEvent("PS", r.CalculateAngle().ToString());
            Debug.Log(r.CalculateAngle());
            r.ResetRays();
            GL = FindObjectOfType<GoalLogic>();
            GL.RotationReset();
            TriggerRightToggle = true;
        }
        if (triggerRightValue == 0)
        {

            TriggerRightToggle = false;
        }

        /*if (Input.GetKeyDown(KeyCode.A) && r.isPointer())
        {
            // Send data to Ray and finish the experiment;
            // Get ray value
            Ray ray = new Ray(rightControllerObject.transform.position, rightControllerObject.transform.forward);
            // Send it to ray.
            r.SetPointValue(ray.direction);
            l.LogEvent("PS", r.CalculateAngle().ToString());
            Debug.Log(r.CalculateAngle());
            r.ResetRays();
            GL = FindObjectOfType<GoalLogic>();
            GL.RotationReset();
        }*/


        // Press X to Change Fog
        if (primaryButtonLeftValue && !PrimaryButtonLeftToggle && isUIToggle)
        {

            PrimaryButtonLeftToggle = true;
        }
        if (primaryButtonLeftValue)
        {
            PrimaryButtonLeftToggle = false;
        }



        uiManager.UpdateRotationUI(GetRotation());
        uiManager.UpdatePositionUI(GetPosition());
        //UpdateLog(GetPosition(), GetRotation());
    }

    public void SetFog(bool b)
    {
        fog.SetActive(b);
    }

    public void ChangeWireframe()
    {
        switch (uiManager.GetWireframeMode())
        {
            case UIManager.WireframeMode.Off:
                Debug.Log("off");
                walls = GameObject.FindObjectsOfType<ShaderChanger>();
                foreach (ShaderChanger wall in walls)
                {
                    try
                    {
                        wall.setMaterialRocks();
                    }
                    catch (Exception e)
                    {

                    }

                }
                break;

            case UIManager.WireframeMode.On:
                Debug.Log("on");
                walls = GameObject.FindObjectsOfType<ShaderChanger>();
                foreach (ShaderChanger wall in walls)
                {

                    wall.setMaterialWireframe();
                }

                break;

            case UIManager.WireframeMode.Auto:
                Debug.Log("auto");
                walls = GameObject.FindObjectsOfType<ShaderChanger>();
                Debug.Log(walls.Length);
                foreach (ShaderChanger wall in walls)
                {
                    try
                    {
                        wall.setMaterialRocks();
                    }catch (Exception e)
                    {

                    }
                    

                }

                break;

        }
    }

    public void ChangeMovement()
    {
        switch (uiManager.GetMovementMode())
        {
            case UIManager.MovementType.Teleport:
                teleportLine.SetActive(true);
                cm.enabled = false;
                cm.ForgetWalls();
                tp.enabled = true;
                tp.setTeleportaion();
                SetFog(false);
                break;

            case UIManager.MovementType.Dash:
                teleportLine.SetActive(true);
                tp.setDash();
                SetFog(false);
                break;

            case UIManager.MovementType.Walk:
                teleportLine.SetActive(false);
                cm.enabled = true;
                tp.enabled = false;
                SetFog(false);
                break;

            case UIManager.MovementType.Fog:
                teleportLine.SetActive(false);
                cm.enabled = true;
                tp.enabled = false;
                SetFog(true);
                break;

        }
    }
}
