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
    private EquipmentBehaviour equipmentBehaviour;

    /// <summary>
    /// Overridden method to initialize this script, sets activation logic of behaviour to this and sets the light settings according to the data
    /// </summary>
    /// <param name="_myEquipment"></param>
    public override void Initialize(EquipmentBehaviour _myEquipment, Transform _lightFirepoint)
    {
        equipmentBehaviour = _myEquipment;

        // Set activation logic to this
        equipmentBehaviour.activationLogic = this;

        // Cast the data as FlashlightObject to access the specific properties
        flashlightData = (FlashlightObject)equipmentBehaviour.EquipmentData;

        // Add a new Light component to the transform of the behaviour script
        Light newLight = _lightFirepoint.AddComponent<Light>();

        // Set the light settings to script.obj values
        myLight = GetLightValues(newLight); // Method returns a light with settings adjusted according to data.

        myLight.enabled = false;
    }

    /// <summary>
    /// Method that activates specific equipment logic, in this case turning light on/off
    /// </summary>
    public override void Activate()
    {
        // Call method to toggle the light
        ToggleFlashlight();
    }

    /// <summary>
    /// Handles auto disabling light on drop
    /// </summary>
    private void Update()
    {
        // If we're equipped, do nothing and return
        if (equipmentBehaviour.IsEquipped) return;
        else // No longer equipped, so disable the light
        {
            // Disable light if enabled
            if (myLight.enabled)
            {
                myLight.enabled = false;
            }
        }
    }

    /// <summary>
    /// Method for turning the flashlight on/off, based on current status
    /// </summary>
    private void ToggleFlashlight()
    {
        myLight.enabled = !myLight.enabled;
    }

    /// <summary>
    /// Returns a Light component with properties set to the scriptable object reference
    /// </summary>
    /// <param name="_light"></param>
    /// <returns></returns>
    private Light GetLightValues(Light _light)
    {
        // Check if the data object is not null
        if (flashlightData != null)
        {
            // Set properties of light component to the data's values
            _light.spotAngle = flashlightData.SpotAngle;

            _light.range = flashlightData.LightRange;

            _light.intensity = flashlightData.LightIntensity;

            _light.type = flashlightData.LightType;

            _light.renderMode = flashlightData.LightRenderMode;

            _light.cullingMask = flashlightData.LightCullingMask;

            _light.color = flashlightData.LightColor;   
        }

        // Return the light with new settings
        return _light;
    }

}
