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

    public GameObject XRRig;
    private TeleportationProvider tp;
    private continuousMovement cm;

    private int movementType = 0;

    private float[] rotation;
    private float[] position;

    public UIManager uiManager;

    private bool PrimaryButtonRightToggle;
    private bool SecondaryButtonRightToggle;

    private ShaderChanger[] walls;

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
        float rotationX = XRRig.transform.rotation.x;
        float rotationY = XRRig.transform.rotation.y;
        float rotationZ= XRRig.transform.rotation.z;

        return new float[3] { rotationX, rotationY, rotationZ };
    }

    private float[] GetPosition()
    {
        float positionX = XRRig.transform.position.x;
        float positionY = XRRig.transform.position.y;
        float positionZ = XRRig.transform.position.z;

        return new float[3] { positionX, positionY, positionZ };
    }

    // Start is called before the first frame update
    void Start()
    {
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

        // Press A to change movement type
        if (primaryButtonRightValue && !PrimaryButtonRightToggle)
        {
            movementType++;
            if (movementType == 3)
                movementType = 0;

            switch (movementType)
            {
                case 0:
                    cm.enabled = false;
                    tp.enabled = true;
                    tp.changeType();
                    break;

                case 1:
                    tp.changeType();
                    break;

                case 2:
                    cm.enabled = true;
                    tp.enabled = false;
                    break;

            }
            uiManager.ChangeMovementType();
            PrimaryButtonRightToggle = true;
        }

        if (!primaryButtonRightValue)
        {
            PrimaryButtonRightToggle = false;
        }

        // Press B to change Wireframe Mode
        if (secondaryButtonRightValue && !SecondaryButtonRightToggle)
        {
            uiManager.ChangeWireframeType();
            foreach (ShaderChanger wall in walls)
            {
                wall.changeMaterial();
            }
            SecondaryButtonRightToggle = true;
        }
        if (!secondaryButtonRightValue)
        {
            SecondaryButtonRightToggle = false;
        }

        uiManager.UpdateRotationUI(GetRotation());
        uiManager.UpdatePositionUI(GetPosition());
    }
}
