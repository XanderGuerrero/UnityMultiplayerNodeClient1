using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoActivateMe : MonoBehaviour
{
    private string whoactivatedMe;

    public void SetActivator(string activator)
    {
        whoactivatedMe = activator;
    }

    public string GetActivator()
    {
        return whoactivatedMe;
    }
}
