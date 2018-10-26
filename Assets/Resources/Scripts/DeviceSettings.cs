using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceSettings : MonoBehaviour
{

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
