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

    /// <summary>
    /// Updates the different keys to the new key bindings <br/>
    /// Only updates if param keys are not None, and if activation and equip keys are not the same
    /// </summary>
    /// <param name="_newKeyBindings"></param>
    public void UpdateKeyBindings(HandKeyBindings _newKeyBindings)
    {
        // Check if the new Activation key is not none (default value) and if its not binded by DropEquipKey
        if (_newKeyBindings.ActivationKey != KeyCode.None && _newKeyBindings.ActivationKey != DropEquipKey)
        {
            // Set this activation key to the new key
            this.ActivationKey = _newKeyBindings.ActivationKey;
        }

        // Check if the new DropEquip key is not none (Default) and if it is not binded by this ActivationKey
        if (_newKeyBindings.DropEquipKey != KeyCode.None && _newKeyBindings.DropEquipKey != ActivationKey)
        {
            // Set this drop/equip key to the new key
            this.DropEquipKey = _newKeyBindings.DropEquipKey;
        }

        // Check if the new FireModeSwapkey is not None( default value)
        if (_newKeyBindings.FireModeSwapKey != KeyCode.None)
        {
            // Assign new key to this key if not none
            this.FireModeSwapKey = _newKeyBindings.FireModeSwapKey;
        }
    }
}
