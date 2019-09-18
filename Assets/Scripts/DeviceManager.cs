using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class DeviceManager : MonoBehaviour
{
    public GameObject imcompatible;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        return;
#endif
        print(Device.generation);
        switch (Device.generation)
        {
            case DeviceGeneration.Unknown:
            case DeviceGeneration.iPhone6:
            case DeviceGeneration.iPhone6S:
            case DeviceGeneration.iPhone6Plus:
            case DeviceGeneration.iPhone6SPlus:
            case DeviceGeneration.iPhoneSE1Gen:
            case DeviceGeneration.iPhone7:
            case DeviceGeneration.iPhone7Plus:
            case DeviceGeneration.iPhone8:
            case DeviceGeneration.iPhone8Plus:
            case DeviceGeneration.iPhoneX:
            case DeviceGeneration.iPhoneXR:
            case DeviceGeneration.iPhoneXS:
            case DeviceGeneration.iPhoneXSMax:
            case DeviceGeneration.iPodTouch6Gen:
            case DeviceGeneration.iPhone5:
            case DeviceGeneration.iPhone5C:
            case DeviceGeneration.iPhone5S:
                break;
            default:
                GameObject.Find("Swipable").SetActive(false);
                GameObject.Find("GameSpaceObjects").SetActive(false);
                imcompatible.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
