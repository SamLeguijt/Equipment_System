using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nested class for key bindings for each hand
/// Nested because we don't need access outside this class to the class
/// </summary>
public class HandKeyBindings
{
    // Define private keycodes for activation and drop/equip keys
    private KeyCode activationKey;
    private KeyCode dropEquipKey;
    private KeyCode fireModeSwapKey;

    // Public read-only references to both keys (don't want to set them outside constructor)
    public KeyCode ActivationKey { get { return activationKey; } set { activationKey = value; } }
    public KeyCode DropEquipKey { get { return dropEquipKey; } set { dropEquipKey = value; } }
    public KeyCode FireModeSwapKey { get { return fireModeSwapKey; } set { fireModeSwapKey = value; } }

    // Constructor sets hand with activation and equip key to param values
    public HandKeyBindings(KeyCode _activationKey, KeyCode _dropEquipKey, KeyCode _fireModeSwapKey)
    {
        this.activationKey = _activationKey;
        this.dropEquipKey = _dropEquipKey;
        this.fireModeSwapKey = _fireModeSwapKey;
    }

    public void UpdateKeyBindings(HandKeyBindings _newKeyBindings)
    {
        if (_newKeyBindings.ActivationKey != KeyCode.None)
        {
            this.ActivationKey = _newKeyBindings.ActivationKey;
        }

/*        if (_newKeyBindings.DropEquipKey != KeyCode.None)
        {

        }*/
       // this.DropEquipKey = _newKeyBindings.DropEquipKey;
        //this.FireModeSwapKey = _newKeyBindings.FireModeSwapKey;
    }
}
