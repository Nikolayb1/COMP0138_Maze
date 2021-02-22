using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInit : MonoBehaviour
{
    private UIManager uim;
    public UIManager.MovementType selectedMovementType;
    private UIManager.MovementType currentMovementType;
    private InputManager im;
    public Logger l;
    public GameObject tutorialMessage;
    public Text text;

    private string[] tutorialText = new string[] { "Locomotion Method: Teleport\n\nHow to use: Point with the right controller to where you want to move. If the line glows green, it means that you can move to that point, red, means you cannot. When you are ready to move press the right trigger. You can also use the right joystick to turn in place.",
    "Locomotion Method: Dash\n\nHow to use: Point with the right controller to where you want to move. If the line glows green, it means that you can move to that point, red, means you cannot. When you are ready to move press the right trigger. You can also use the right joystick to turn in place.",
    "Locomotion Method: Wireframe\n\nHow to use: Use the left joystick to glide in the direction you want. The direction you are looking in is always forwards. When you move the wall texture will turn blurry. You can also use the right joystick to turn in place.",
    "Locomotion Method: Fog\n\nHow to use: Use the left joystick to glide in the direction you want. The direction you are looking in is always forwards. When you move a cloud of fog will appear around your head. You can also use the right joystick to turn in place."};


    // Start is called before the first frame update
    void Start()
    {
        l = FindObjectOfType<Logger>();
        uim = FindObjectOfType<UIManager>();
        im = FindObjectOfType<InputManager>();
        activateTutorialMessage();
        CrossSceneData.CrossSceneId = Random.Range(0, 999999);
        l.LogEvent("ID", CrossSceneData.CrossSceneId.ToString());
        
    }

    public void nextMovementMethod()
    {
        if (selectedMovementType == UIManager.MovementType.Teleport)
        {
            selectedMovementType = UIManager.MovementType.Dash;
        }else if (selectedMovementType == UIManager.MovementType.Dash)
        {
            selectedMovementType = UIManager.MovementType.Walk;
        }else if (selectedMovementType == UIManager.MovementType.Walk)
        {
            selectedMovementType = UIManager.MovementType.Fog;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMovementType != selectedMovementType)
        {
            uim.SetMovementType(selectedMovementType);
            currentMovementType = uim.GetMovementMode();
            im.ChangeMovement();
        }
    }

    public void activateTutorialMessage()
    {
        tutorialMessage.SetActive(true);
        switch (selectedMovementType)
        {
            case UIManager.MovementType.Teleport:
                text.text = tutorialText[0];
                break;
            case UIManager.MovementType.Dash:
                text.text = tutorialText[1];
                break;
            case UIManager.MovementType.Walk:
                text.text = tutorialText[2];
                break;
            case UIManager.MovementType.Fog:
                text.text = tutorialText[3];
                break;
        }
        
    }

    public void deactivateTutorialMessage()
    {
        tutorialMessage.SetActive(false);
    }
}
