using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class KeyBindingsUI : MonoBehaviour
{

    public Hand hand;
    public TMP_InputField equipLeft;
    public TMP_InputField swapModeKey;


    public TMP_Dropdown keyDropdown;
    public TMP_InputField activationInputField;

    private KeyCode stagedActivationKey;
    private KeyCode stagedEquipKey;
    private KeyCode stagedSwapModeKey;


    private void Start()
    {
        keyDropdown.options.Add(new TMP_Dropdown.OptionData(SettingsManager.MOUSE_LEFT_STRING));
        keyDropdown.options.Add(new TMP_Dropdown.OptionData(SettingsManager.MOUSE_RIGHT_STRING));
        keyDropdown.options.Add(new TMP_Dropdown.OptionData(SettingsManager.MANUAL_INPUT_STRING));


        // Subscribe to the dropdown's value changed event
        keyDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Initially hide the manual input field
        activationInputField.gameObject.SetActive(false);
    }

    private void OnDropdownValueChanged(int index)
    {
        string selectedOption = keyDropdown.options[index].text;

        // Handle the selected option
        switch (selectedOption)
        {
            case SettingsManager.MOUSE_LEFT_STRING:
                activationInputField.gameObject.SetActive(false);
                stagedActivationKey = KeyCode.Mouse0;
                break;
            case SettingsManager.MOUSE_RIGHT_STRING:
                activationInputField.gameObject.SetActive(false);
                stagedActivationKey = KeyCode.Mouse1;
                break;
            case SettingsManager.MANUAL_INPUT_STRING:
                // Show the manual input field
                activationInputField.gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning("Invalid option selected");
                break;
        }
    }

    public void OnInputActivtionKey(TMP_InputField _inputField)
    {
        // Set to Upper to convert to KeyCodes (lower case != keycode)
        string keyString = _inputField.text.ToUpper();

        // Convert the text to a KeyCode
        if (System.Enum.TryParse(keyString, out KeyCode newKey))
        {
            if (IsAlphabeticKey(newKey))
            {
                Debug.Log("New staged key");
                // You can use the 'newKey' variable in your logic here
                stagedActivationKey = newKey;
            }
        }
        else
        {
            stagedActivationKey = KeyCode.None;
        }
    }

    private bool IsAlphabeticKey(KeyCode keyCode)
    {
        return (keyCode >= KeyCode.A && keyCode <= KeyCode.Z);
    }
    public void ApplyKeyBindings()
    {
        HandKeyBindings newKeyBindings = new HandKeyBindings(stagedActivationKey, stagedEquipKey, stagedSwapModeKey);

        hand.UpdateKeyBindings(newKeyBindings);

      //  stagedActivationKey = KeyCode.None;
        stagedEquipKey = KeyCode.None;
        stagedSwapModeKey = KeyCode.None;   
    }


    public void OnEquipInputChange()
    {
        Debug.Log("equip key chage");

    }
    public void OnSwapModeInputChange()
    {
        Debug.Log("Swap mode chage");

    }
}
