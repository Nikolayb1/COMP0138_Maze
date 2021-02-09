using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    public bool isUIToggle;
    public bool isTutorial;

    private InputDevice leftController;
    private InputDevice rightController;
    private InputDeviceCharacteristics leftControllerCharacteristics;
    private InputDeviceCharacteristics rightControllerCharacteristics;
    public GameObject endMessage;
    public Text endMessageText;
    public Logger log;

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

    private string[] notifications = new string[] { "\tPlease take off your VR headset and complete the next page of the online form.\n\n\tWhen you are done put the VR headset back on and press A.",
                                                    "If you would like to play the tutorial again please press B. To continue press A"};

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
        endMessageText.text = notifications[i];
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
    public void UpdateLog(float[] position, float[] rotation)
    {
        log.addRecord(position[0].ToString(), position[1].ToString(), position[2].ToString(), rotation[0].ToString(), rotation[1].ToString(), rotation[2].ToString());
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

        // Press B to replay tutorial
        if (secondaryButtonRightValue && !PrimaryButtonRightToggle && isTutorial)
        {
            GL = FindObjectOfType<GoalLogic>();
            GL.ResetTutorial();
            SecondaryButtonRightToggle = true;
        }

        if (!secondaryButtonRightValue)
        {
            SecondaryButtonRightToggle = false;
        }

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
                cm.enabled = false;
                cm.ForgetWalls();
                tp.enabled = true;
                tp.setTeleportaion();
                SetFog(false);
                break;

            case UIManager.MovementType.Dash:
                tp.setDash();
                SetFog(false);
                break;

            case UIManager.MovementType.Walk:
                cm.enabled = true;
                tp.enabled = false;
                SetFog(false);
                break;

            case UIManager.MovementType.Fog:
                cm.enabled = true;
                tp.enabled = false;
                SetFog(true);
                break;

        }
    }
}
