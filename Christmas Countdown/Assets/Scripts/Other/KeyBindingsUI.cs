using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class KeyBindingsUI : MonoBehaviour
{

    public Hand targetHand;

    public TMP_Dropdown dropdownElement;
    public TMP_InputField inputFieldElement;

    private KeyCode stagedActivationKey;
    private KeyCode stagedEquipKey;
    private KeyCode stagedFireModeSwapKey;
    private KeyCode keyToStage;

    public enum KeyType
    {
        ActivationKey,
        EquipKey,
        FireModeSwapKey
    }

    public KeyType targetType;
    private void Start()
    {
        dropdownElement.options.Add(new TMP_Dropdown.OptionData(SettingsManager.MOUSE_LEFT_STRING));
        dropdownElement.options.Add(new TMP_Dropdown.OptionData(SettingsManager.MOUSE_RIGHT_STRING));
        dropdownElement.options.Add(new TMP_Dropdown.OptionData(SettingsManager.MANUAL_INPUT_STRING));


        // Subscribe to the dropdown's value changed event
        dropdownElement.onValueChanged.AddListener(OnDropdownValueChanged);

        // Initially hide the manual input field
        inputFieldElement.gameObject.SetActive(false);
    }

    private void OnDropdownValueChanged(int index)
    {
        string selectedOption = dropdownElement.options[index].text;

        // Handle the selected option
        switch (selectedOption)
        {
            case SettingsManager.MOUSE_LEFT_STRING:
                inputFieldElement.gameObject.SetActive(false);
                stagedActivationKey = KeyCode.Mouse0;
                break;
            case SettingsManager.MOUSE_RIGHT_STRING:
                inputFieldElement.gameObject.SetActive(false);
                stagedActivationKey = KeyCode.Mouse1;
                break;
            case SettingsManager.MANUAL_INPUT_STRING:
                // Show the manual input field
                inputFieldElement.gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning("Invalid option selected");
                break;
        }
    }

    public void OnInputFireModeKey(TMP_InputField _inputField)
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

    public void OnInputEquipKey(TMP_InputField _inputField)
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
                stagedEquipKey = newKey;
            }
        }
        else
        {
            stagedActivationKey = KeyCode.None;
        }
    }

    public void OnInputFieldUpdate(TMP_InputField _inputField)
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
                keyToStage = newKey;
            }
        }
        else
        {
            keyToStage = KeyCode.None;
        }
    }
/*
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
                stagedFireModeSwapKey = newKey;
            }
        }
        else
        {
            stagedActivationKey = KeyCode.None;
        }
    }*/

    private bool IsAlphabeticKey(KeyCode keyCode)
    {
        return (keyCode >= KeyCode.A && keyCode <= KeyCode.Z);
    }
    public void ApplyKeyBindings()
    {
        if (targetType == KeyType.ActivationKey)
        {

            HandKeyBindings KeyBindings = new HandKeyBindings(keyToStage, KeyCode.None, KeyCode.None);

            targetHand.UpdateKeyBindings(KeyBindings);

        }
        else if (targetType == KeyType.EquipKey)
        {
            HandKeyBindings KeyBindings = new HandKeyBindings(KeyCode.None, keyToStage, KeyCode.None);

            targetHand.UpdateKeyBindings(KeyBindings);

        }

        /* Update the key of type KeyType from targetHand.KeyBindings to the keyToStage
         * 
         */


        //   HandKeyBindings newKeyBindings = new HandKeyBindings(stagedActivationKey, stagedEquipKey, stagedFireModeSwapKey);
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
