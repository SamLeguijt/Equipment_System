using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlashlightActivation : ToolActivation
{
    // Reference to the light component for this object
    private Light myLight;

    // Reference to the ScriptableObject data for the light
    private FlashlightObject flashlightData;

    /// <summary>
    /// Overridden method to initialize this script, sets activation logic of behaviour to this and sets the light settings according to the data
    /// </summary>
    /// <param name="_myEquipment"></param>
    public override void Initialize(EquipmentBehaviour _myEquipment)
    {
        // Set activation logic to this
        _myEquipment.activationLogic = this;

        // Cast the data as FlashlightObject to access the specific properties
        flashlightData = (FlashlightObject)_myEquipment.EquipmentData;

        // Add a new Light component to the transform of the behaviour script
        Light newLight = _myEquipment.transform.AddComponent<Light>();

        // Set the light settings to script.obj values
        myLight = GetLightValues(newLight); // Method returns a light with settings adjusted according to data.
    }

    /// <summary>
    /// Returns a Light component with properties set to the param according to the scriptable object reference
    /// </summary>
    /// <param name="_light"></param>
    /// <returns></returns>
    private Light GetLightValues(Light _light)
    {
        if (flashlightData != null)
        {
            _light.type = LightType.Spot;

            _light.intensity = 30f;

        }


        return _light;
    }

    public override void Activate()
    {
        Debug.Log("Activate flashlight");
    }
}
