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
        print(Device.generation);
        switch (Device.generation)
        {
            case DeviceGeneration.iPhone6:
            case DeviceGeneration.iPhone6S:
            case DeviceGeneration.iPhone6Plus:
            case DeviceGeneration.iPhone6SPlus:
            //case DeviceGeneration.iPhoneSE1Gen:
            case DeviceGeneration.iPhone7:
            case DeviceGeneration.iPhone7Plus:
            case DeviceGeneration.iPhone8:
            case DeviceGeneration.iPhone8Plus:
            case DeviceGeneration.iPhoneX:
            case DeviceGeneration.iPhoneXR:
            case DeviceGeneration.iPhoneXS:
            case DeviceGeneration.iPhoneXSMax:
                break;
            default:
                AppManager.Prefabs[0].SetActive(false);
                imcompatible.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
