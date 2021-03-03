using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterControllerPosition : MonoBehaviour
{
    private CharacterController characterController = null;
    public GameObject head = null;
    private float distance;
    private int platLayer;
    // Start is called before the first frame update
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
        distance = characterController.radius;
        platLayer = LayerMask.NameToLayer("Walls");
    }

    void Start()
    {
        PositionController();
    }

    // Update is called once per frame
    void Update()
    {
        PositionController();
        RaycastHit hit;

        //Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms. (Adjust to your needs)
        Vector3 p1 = head.transform.position + Vector3.up * 0.25f;
        //Top of controller
        Vector3 p2 = p1 + Vector3.up * characterController.height;

        //Check around the character in a 360, 10 times (increase if more accuracy is needed)
        for (int i = 0; i < 360; i += 36)
        {
            //Check if anything with the platform layer touches this object
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, distance, 1 << platLayer))
            {
                //If the object is touched by a platform, move the object away from it
                characterController.Move(hit.normal * (distance - hit.distance));
            }
        }

        //[Optional] Check the players feet and push them up if something clips through their feet.
        //(Useful for vertical moving platforms)
        //if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, 1, 1 << platLayer))
        //{
        //    characterController.Move(Vector3.up * (1 - hit.distance));
        //}
    }

    void FixedUpdate()
    {
        
    }

    private void PositionController()
    {
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2);
        characterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        characterController.center = newCenter;
        
    }
}
