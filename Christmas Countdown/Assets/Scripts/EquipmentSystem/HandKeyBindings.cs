using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nested class for key bindings for each hand
/// Nested because we don't need access outside this class to the class
/// </summary>
public class HandKeyBindings
{
    // Reference to the hand
    private Hand hand;

    // Define private keycodes for activation and drop/equip keys
    private KeyCode activationKey;
    private KeyCode dropEquipKey;
    private KeyCode fireModeSwapKey;

    // Public read-only references to both keys (don't want to set them outside constructor)
    public KeyCode ActivationKey { get { return activationKey; } }
    public KeyCode DropEquipKey { get { return dropEquipKey; } }
    public KeyCode FireModeSwapKey { get { return fireModeSwapKey; } }

    // Constructor sets hand with activation and equip key to param values
    public HandKeyBindings(Hand _hand, KeyCode _activationKey, KeyCode _dropEquipKey, KeyCode _fireModeSwapKey)
    {
        this.hand = _hand;
        this.activationKey = _activationKey;
        this.dropEquipKey = _dropEquipKey;
        this.fireModeSwapKey = _fireModeSwapKey;
    }
}
