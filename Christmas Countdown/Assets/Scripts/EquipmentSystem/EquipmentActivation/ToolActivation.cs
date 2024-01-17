using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolActivation : MonoBehaviour, IEquipmentActivation
{
    private bool isLightOn;

    private Light lightObject;

    private ToolEquipmentObject toolData;

    public void Initialize(Light _light)
    {
        // First get our equipmentBehaviour attached to same object
        EquipmentBehaviour behaviour = GetComponent<EquipmentBehaviour>();

        // Set our weapon data according to our equipment behaviour, casted in correct type
        toolData = (ToolEquipmentObject)behaviour.EquipmentData;

        SetLightSettings(_light);
    }

    public void Activate()
    {
        isLightOn = true;
    }

    private Light SetLightSettings(Light _light)
    {
        _light.type = LightType.Spot;
        

        return _light;
    }
}
