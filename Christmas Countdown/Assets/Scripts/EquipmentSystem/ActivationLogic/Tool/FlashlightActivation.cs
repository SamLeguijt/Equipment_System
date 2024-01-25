using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FlashlightActivation : ToolActivation
{
    // Reference to the ScriptableObject data for the light
    private FlashlightObject flashlightData;

    // Reference to the equipment behaviour, to see if equipment is still equipped
    private EquipmentBehaviour equipmentBehaviour;

    // Reference to the light component for this object
    private Light myLight;

    public Light MyLight {  get { return myLight; } }   

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

        // Instantiate new light object on the firepoint pos and rotation, as child of the firepoint
        GameObject lightPrefab = Instantiate(flashlightData.LightObjectPrefab, _lightFirepoint.position, _lightFirepoint.rotation, _lightFirepoint);

        // Get the light component
        Light light = lightPrefab.GetComponent<Light>();

        // Change the values accordign to data
        myLight = GetLightValues(light);
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
