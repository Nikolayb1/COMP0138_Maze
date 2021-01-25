using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInit : MonoBehaviour
{
    private UIManager uim;
    public UIManager.MovementType selectedMovementType;
    private UIManager.MovementType currentMovementType;
    private InputManager im;
    // Start is called before the first frame update
    void Start()
    {
        uim = FindObjectOfType<UIManager>();
        im = FindObjectOfType<InputManager>();
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
}
