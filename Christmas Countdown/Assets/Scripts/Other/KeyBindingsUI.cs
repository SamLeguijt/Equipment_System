using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class KeyBindingsUI : MonoBehaviour
{
    [Tooltip("Reference to the Dropdown UI element for this Setting")]
    [SerializeField] private TMP_Dropdown dropdownElement;

    [Tooltip("Reference to the input field UI element for this setting")]
    [SerializeField] private TMP_InputField inputFieldElement;

    [Tooltip("What type of key does this setting check and respond to")]
    [SerializeField] private KeyType targetType;

    // Constant string variables to set text of Mouse and manual keycodes 
    public const string MOUSE_LEFT_STRING = "Mouse0";
    public const string MOUSE_RIGHT_STRING = "Mouse1";
    public const string MANUAL_INPUT_STRING = "Manual";

    // Reference to the key to stage
    private KeyCode stagedKey;

    // Private enum to define the type of key 
    private enum KeyType
    {
        ActivationKey,
        EquipKey,
        FireModeSwapKey
    }

    private void Start()
    {
        // Add options to dropdown, based on the constant strings
        dropdownElement.options.Add(new TMP_Dropdown.OptionData(MOUSE_LEFT_STRING));
        dropdownElement.options.Add(new TMP_Dropdown.OptionData(MOUSE_RIGHT_STRING));
        dropdownElement.options.Add(new TMP_Dropdown.OptionData(MANUAL_INPUT_STRING));

        // Subscribe to the dropdown's value changed event
        dropdownElement.onValueChanged.AddListener(OnDropdownValueChanged);

        // Initially hide the manual input field
        inputFieldElement.gameObject.SetActive(false);
    }

    /// <summary>
    /// Gets called upon selecting a new value in the dropdown element
    /// </summary>
    /// <param name="index"></param>
    private void OnDropdownValueChanged(int index)
    {
        // Get the option as text from the selected index
        string selectedOption = dropdownElement.options[index].text;

        // Handle the selected option by comparing with our constants
        switch (selectedOption)
        {
            // Disable the input field, and stage the Mouse0 KeyCode
            case MOUSE_LEFT_STRING:
                inputFieldElement.gameObject.SetActive(false);
                stagedKey = KeyCode.Mouse0;
                break;
            // Disable the input field and stage the mouse1 keycode
            case MOUSE_RIGHT_STRING:
                inputFieldElement.gameObject.SetActive(false);
                stagedKey = KeyCode.Mouse1;
                break;
            // Manual so enable the input field to receive input
            case MANUAL_INPUT_STRING:
                // Show the manual input field
                inputFieldElement.gameObject.SetActive(true);
                break;
            default: // Selected option does not match one of the strings
                Debug.LogWarning("Invalid option selected");
                break;
        }
    }

    /// <summary>
    /// Gets called upon declaring a new value in the manual input field
    /// </summary>
    /// <param name="_inputField"></param>
    public void OnInputFieldUpdate(TMP_InputField _inputField)
    {
        // Set to Upper to convert to KeyCodes (lower case != keycode)
        string keyString = _inputField.text.ToUpper();

        // Convert the text to a KeyCode (try)
        if (System.Enum.TryParse(keyString, out KeyCode newKey))
        {
            // Check if the newkey is alphabetic (no special characters or 0-9)
            if (IsAlphabeticKey(newKey))
            {
                // The newkey can be used, so stage it 
                stagedKey = newKey;
            }
        }
        else
        {
            // Set stagedKey to null (KeyCode.None ~= null)
            stagedKey = KeyCode.None;
        }
    }

    /// <summary>
    /// Applies new key bindings to param _hand <br/>
    /// Gets called from button UI press
    /// </summary>
    /// <param name="_targetHand"></param>
    public void ApplyKeyBindings(Hand _targetHand)
    {
        // Make new HandKeyBindings local instance with .None keys for default
        HandKeyBindings newKeyBindings = new HandKeyBindings(KeyCode.None, KeyCode.None, KeyCode.None);

        // Switch between the target type of key we are updating for
        switch (targetType)
        {
            // Target is activation key, so set the activation key of the new bindings to the stagedKey
            case KeyType.ActivationKey:
                newKeyBindings.ActivationKey = stagedKey;
                break;

            // Target is activation key, so set the activation key of the new bindings to the stagedKey
            case KeyType.EquipKey:
                newKeyBindings.DropEquipKey = stagedKey;
                break;
            // Target is activation key, so set the activation key of the new bindings to the stagedKey
            case KeyType.FireModeSwapKey:
                newKeyBindings.FireModeSwapKey = stagedKey;
                break;
            default:
                break;
        }

        // Call method to update the key bindings for the target hand with the new bindings
        _targetHand.UpdateKeyBindings(newKeyBindings);
    }

    /// <summary>
    /// Returns true if the param _keyCode is within the alphabet
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    private bool IsAlphabeticKey(KeyCode _keyCode)
    {
        return (_keyCode >= KeyCode.A && _keyCode <= KeyCode.Z);
    }
}
