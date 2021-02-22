using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ContinuousTurn : MonoBehaviour
{
    public float speed = 1.0f;
    private InputDevice rightController;
    private InputDeviceCharacteristics rightControllerCharacteristics;
    private CharacterController characterController = null;
    // Start is called before the first frame update
    void Start()
    {
        rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        FindInputDevices();
    }
    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FindInputDevices()
    {
        List<InputDevice> devicesLeft = new List<InputDevice>();
        List<InputDevice> devicesRight = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devicesRight);

        if (devicesRight.Count > 0)
        {
            rightController = devicesRight[0];

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rightController == null)
        {
            FindInputDevices();
        }

        if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
        {
            characterController.transform.Rotate(new Vector3(0f, position.x * speed, 0f));
        }
    }
}
