using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class continuousMovement : LocomotionProvider
{
    public float speed = 1.0f;
    public List<XRController> controllers = null;

    public UIManager uIManager;
    public InputManager im;
    public ShaderChanger[] walls;
    public ShaderChanger floor;
    public GameObject ceilling;

    private bool movementBool = false;

    private CharacterController characterController = null;
    public Camera head = null;

    protected override void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
        ceilling.SetActive(false);
        //head = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    public void FindWalls()
    {
        walls = GameObject.FindObjectsOfType<ShaderChanger>();
    }

    public void ForgetWalls()
    {
        walls = null;
    }
        // Update is called once per frame
    void Update()
    {
        CheckForInput();
        //Debug.Log(walls.Length);
        if (walls == null || walls.Length == 0)
        {
            FindWalls();
            
        }

    }

    private void CheckForInput()
    {
        foreach(XRController controller in controllers)
        {
            if (controller.enableInputActions)
                CheckForMovement(controller.inputDevice);
        }
    }

    private void CheckForMovement(InputDevice device)
    {
        
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
        { 
            if (uIManager.GetWireframeMode() == UIManager.WireframeMode.Auto)
            {
  
                if (position.magnitude < 0.0001f && movementBool)
                {
                    Debug.Log("Standing");
                    movementBool = false;
                    if (walls != null)
                    {
                        foreach (ShaderChanger wall in walls)
                        {

                            wall.setMaterialRocks();

                        }
                    }
                    
                    //floor.setMaterialRocks();
                    //ceilling.SetActive(false);
                }
                else if (position.magnitude >= 0.0001f && !movementBool)
                {
                    Debug.Log("Moving");
                    movementBool = true;
                    foreach (ShaderChanger wall in walls)
                    {
                        
                        wall.setMaterialWireframe();
                    }
                    //ceilling.SetActive(true);
                    
                    //floor.setMaterialWireframe();
                }

                if (movementBool)
                {
                    StartMove(position);
                }
            }else if(uIManager.GetWireframeMode() == UIManager.WireframeMode.Off)
            {

                if (position.magnitude < 0.0001f && movementBool)
                {
                    Debug.Log("Standing");
                    im.SetFog(false);
                    movementBool = false;

                }
                else if (position.magnitude >= 0.0001f && !movementBool)
                {
                    Debug.Log("Moving");
                    im.SetFog(true);
                    movementBool = true;
                }

                if (movementBool)
                {
                    StartMove(position);
                }
            }



        }

          
    }

    private void StartMove(Vector2 position)
    {
        Vector3 direction = new Vector3(position.x, 0, position.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        direction = Quaternion.Euler(headRotation) * direction;

        Vector3 movement = direction * speed;
        characterController.Move(movement * Time.deltaTime);
        
    }
}
