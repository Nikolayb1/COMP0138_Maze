using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    private InputDevice leftController;
    private InputDevice rightController;
    private InputDeviceCharacteristics leftControllerCharacteristics;
    private InputDeviceCharacteristics rightControllerCharacteristics;

    public Logger log;

    public GameObject XRRig;
    public GameObject mainCamera;
    private TeleportationProvider tp;
    private continuousMovement cm;

    private int movementType = 0;
    private int wireframeType = 0;

    private float[] rotation;
    private float[] position;

    public UIManager uiManager;

    private bool PrimaryButtonRightToggle;
    private bool SecondaryButtonRightToggle;
    private bool PrimaryButtonLeftToggle;

    public ShaderChanger[] walls;
    public ShaderChanger floor;
    public GameObject ceilling;

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
        ceilling.SetActive(false);
        leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        FindInputDevices();

        rotation = new float[3];
        position = new float[3];

        tp = XRRig.GetComponent<TeleportationProvider>();
        cm = XRRig.GetComponent<continuousMovement>();

        cm.enabled = false;

        walls = GameObject.FindObjectsOfType<ShaderChanger>();
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
        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonLeftValue);

        // Press A to change movement type
        if (primaryButtonRightValue && !PrimaryButtonRightToggle)
        {
            uiManager.ChangeMovementType();
            PrimaryButtonRightToggle = true;
            ChangeMovement();




        }

        if (!primaryButtonRightValue)
        {
            PrimaryButtonRightToggle = false;
        }

        // Press B to change Wireframe Mode
        if (secondaryButtonRightValue && !SecondaryButtonRightToggle)
        {
            uiManager.ChangeWireframeType();
            SecondaryButtonRightToggle = true;
            ChangeWireframe();


        }
        if (!secondaryButtonRightValue)
        {
            
            SecondaryButtonRightToggle = false;
        }

        // Press X to Change Fog
        if (primaryButtonLeftValue && !PrimaryButtonLeftToggle)
        {
            togglefog();
            PrimaryButtonLeftToggle = true;
        }
        if (primaryButtonLeftValue)
        {
            PrimaryButtonLeftToggle = false;
        }



        uiManager.UpdateRotationUI(GetRotation());
        uiManager.UpdatePositionUI(GetPosition());
        UpdateLog(GetPosition(), GetRotation());
    }

    void togglefog()
    {
        if (RenderSettings.fog)
        {
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
        }
    }

    public void ChangeWireframe()
    {
        switch (uiManager.GetWireframeMode())
        {
            case UIManager.WireframeMode.Off:
                Debug.Log("off");
                foreach (ShaderChanger wall in walls)
                {
                    wall.setMaterialRocks();

                }
                floor.setMaterialRocks();
                ceilling.SetActive(false);
                break;

            case UIManager.WireframeMode.On:
                Debug.Log("on");
                foreach (ShaderChanger wall in walls)
                {

                    wall.setMaterialWireframe();
                }
                ceilling.SetActive(true);
                floor.setMaterialWireframe();
                break;

            case UIManager.WireframeMode.Auto:
                Debug.Log("auto");
                foreach (ShaderChanger wall in walls)
                {
                    wall.setMaterialRocks();

                }
                floor.setMaterialRocks();
                ceilling.SetActive(false);
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
                break;

            case UIManager.MovementType.Dash:
                tp.setDash();
                break;

            case UIManager.MovementType.Walk:
                cm.enabled = true;
                tp.enabled = false;
                break;

        }
    }
}
