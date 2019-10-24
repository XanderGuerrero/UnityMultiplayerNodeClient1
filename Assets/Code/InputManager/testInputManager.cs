using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInputManager : MonoBehaviour
{

    // Update is called once per frame
    private void Update()
    {
        if (InputManager.AButton())
        {
            Debug.Log(InputManager.MainJoystick());
        }
        if (InputManager.BButton())
        {
            Debug.Log(InputManager.MainJoystick());
        }
        if (InputManager.RightTrigger() ==1)
        {
            Debug.Log(InputManager.MainJoystick());
        }
    }
}
