using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolActivation : MonoBehaviour, IEquipmentActivation
{
    // Reference to the ToolEquipmentObject ScriptableObject for data
    private ToolEquipmentObject toolData;

    /// <summary>
    /// Virtual method to override in children classes <br/>
    /// Sets the toolData to the behaviours data if tool != flashlight, else adds flashlightActivation and removes this 
    /// </summary>
    /// <param name="_myEquipment"></param>
    public virtual void Initialize(EquipmentBehaviour _myEquipment, Transform _lightFirepoint)
    {
        // Get the tooldata in correct type by casting the behaviour's data reference
        toolData = (ToolEquipmentObject)_myEquipment.EquipmentData;

        // Check if the type of tool is a flahlight
        if (toolData is FlashlightObject) // We use 'is' instead of checking ToolType to get the type of scriptable object
        {            
            // If it is, add a FlashLightActivation to the behaviour's object, and
            FlashlightActivation light = _myEquipment.AddComponent<FlashlightActivation>();

            // Call method to initialize the FlashlightActivation
            light.Initialize(_myEquipment, _lightFirepoint);
            
            // Disable this script since we're using the child script 
            this.enabled= false;
        }
        else // This tool is not a flashlight
        {
            // Set activationlogic of behaviour to this so it will call this activaiton method
            _myEquipment.activationLogic = this;
        }
    }

    /// <summary>
    /// Implement method from interface
    /// </summary>
    public virtual void Activate() { }
}
