using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class handPresence : MonoBehaviour
{
    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerCharacteristics;
    private ShaderChanger[] walls;
    public List<GameObject> controllerPrefabs;
    public TeleportationProvider xrsystem;
    private bool PrimaryButtonToggle;
    private bool SecondaryButtonToggle;
    // Start is called before the first frame update
    void Start()
    {
        PrimaryButtonToggle = false;
        SecondaryButtonToggle = false;
        FindInputDevices();
        walls = GameObject.FindObjectsOfType<ShaderChanger>();
    }

    private void FindInputDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDevice == null)
        {
            FindInputDevices();
        }
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
        Debug.Log(targetDevice.characteristics);
        if (targetDevice.role == InputDeviceRole.LeftHanded)
        {
            if (primaryButtonValue && !PrimaryButtonToggle)
            {
                Debug.Log(primaryButtonValue);
                foreach (ShaderChanger wall in walls)
                {
                    wall.changeMaterial();
                }
                PrimaryButtonToggle = true;
            }

            if (!primaryButtonValue)
            {
                PrimaryButtonToggle = false;
            }

            if (secondaryButtonValue && !SecondaryButtonToggle)
            {
                xrsystem.changeType();
                SecondaryButtonToggle = true;
            }
            if (!secondaryButtonValue)
            {
                SecondaryButtonToggle = false;
            }
        }
        
            
        
    }
}
