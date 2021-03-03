using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    private GameObject menu;

    private Text movementTypeText;
    private Text wireframeModeText;
    private Text rotationText;
    private Text positionText;

    private string movementTypeValueString;
    private string wireframeModeValueString;
    private string rotationValueString;
    private string positionValueString;

    private static string movementTypeStringBegining = "Movement Type: ";
    private static string wireframeModeStringBegining = "Wireframe Mode: ";
    private static string rotationStringBegining = "Rotaion: ";
    private static string positionStringBegining = "Position: ";

    private MovementType currentMovementType;
    private WireframeMode currentWireframeMode;
    

    private Vector3 origin = new Vector3(0f, 0.55f, -2.5f);
    private float setTime;

    private float lastGoal;

    public enum MovementType
    {
        Teleport,
        Dash,
        Walk,
        Fog
    };

    public enum WireframeMode
    {
        Off,
        On,
        Auto
    };

    private MovementType GetNextMovementType(MovementType cur)
    {
        return (from MovementType val in Enum.GetValues(typeof(MovementType))
                where val > cur
                orderby val
                select val).DefaultIfEmpty().First();
    }

    private WireframeMode GetNextWireframeMode(WireframeMode cur)
    {
        return (from WireframeMode val in Enum.GetValues(typeof(WireframeMode))
                where val > cur
                orderby val
                select val).DefaultIfEmpty().First();
    }

    public WireframeMode GetWireframeMode()
    {
        return currentWireframeMode;
    }

    public void SetWireframeMode(WireframeMode mode)
    {
        currentWireframeMode = mode;
    }

    public void SetMovementType(MovementType mode)
    {
        currentMovementType = mode;
    }

    public MovementType GetMovementMode()
    {
        return currentMovementType;
    }

    public void ChangeMovementType()
    {
        currentMovementType = GetNextMovementType(currentMovementType);
    }

    public void ChangeWireframeType()
    {
        currentWireframeMode = GetNextWireframeMode(currentWireframeMode);
    }

    public void UpdatePositionUI(float[] position)
    {
        positionValueString = position[0].ToString() + ", " + position[1].ToString() + ", " + position[2].ToString();
    }

    public void UpdateRotationUI(float[] rotation)
    {
        rotationValueString = rotation[0].ToString() + ", " + rotation[1].ToString() + ", " + rotation[2].ToString();
    }



    public void setLastGoal(float t)
    {
        lastGoal = t;
    }

    public bool canChange()
    {
        if (Time.fixedTime - lastGoal > 3)
        {
            return true;
        }
        return false;
    }

    public void setPosTime(float t)
    {
        setTime = t;
    }

    public bool didSet(Vector3 pos)
    {
        //Debug.Log(Time.fixedTime - setTime);
        if (Time.fixedTime - setTime < 4)
        {
            if (origin == pos && Time.fixedTime - setTime > 3)
            {
                return true;
            }
            return false;
        }
        return true;
    }

    private void GetUI()
    {
        Text[] newText;
        menu = GameObject.FindGameObjectWithTag("RIghtUI");
        newText = menu.GetComponentsInChildren<Text>();

        movementTypeText = newText[0];
        //wireframeModeText = newText[1];
        //rotationText = newText[2];
        //positionText = newText[3];
    }
    // Start is called before the first frame update
    void Start()
    {
        GetUI();
        currentMovementType = MovementType.Teleport;
        currentWireframeMode = WireframeMode.Off;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (menu == null)
        {
            GetUI();
        }
        else
        {
            switch (currentMovementType)
            {
                case MovementType.Teleport:
                    movementTypeValueString = "Teleport";

                    break;

                case MovementType.Dash:
                    movementTypeValueString = "Dash";

                    break;

                case MovementType.Walk:
                    movementTypeValueString = "Blur";

                    break;
                case MovementType.Fog:
                    movementTypeValueString = "Fog";

                    break;
            }

            switch (currentWireframeMode)
            {
                case WireframeMode.On:
                    wireframeModeValueString = "On";

                    break;

                case WireframeMode.Off:
                    wireframeModeValueString = "Off";

                    break;

                case WireframeMode.Auto:
                    wireframeModeValueString = "Auto";

                    break;
            }
            movementTypeText.text = movementTypeStringBegining + movementTypeValueString;
            //wireframeModeText.text = wireframeModeStringBegining + wireframeModeValueString;
            //rotationText.text = rotationStringBegining+ rotationValueString;
            //positionText.text = positionStringBegining + positionValueString;
            
        }
    }


}
