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
    public EquipmentUI equipmentUI;


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

        // Add a listener to the dropdown's OnValueChanged event

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
                keyToStage = KeyCode.Mouse0;
                break;
            case SettingsManager.MOUSE_RIGHT_STRING:
                inputFieldElement.gameObject.SetActive(false);
                keyToStage = KeyCode.Mouse1;
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
    public void OnInputFieldUpdate(TMP_InputField _inputField)
    {
        // Set to Upper to convert to KeyCodes (lower case != keycode)
        string keyString = _inputField.text.ToUpper();

        // Convert the text to a KeyCode
        if (System.Enum.TryParse(keyString, out KeyCode newKey))
        {
            if (IsAlphabeticKey(newKey))
            {
                // You can use the 'newKey' variable in your logic here
                keyToStage = newKey;
            }
        }
        else
        {
            keyToStage = KeyCode.None;
        }
    }

    private bool IsAlphabeticKey(KeyCode keyCode)
    {
        return (keyCode >= KeyCode.A && keyCode <= KeyCode.Z);
    }
    public void ApplyKeyBindings(Hand _hand)
    {
        HandKeyBindings KeyBindings = new HandKeyBindings(KeyCode.None, KeyCode.None, KeyCode.None);

        switch (targetType)
        {
            case KeyType.ActivationKey:

                KeyBindings.ActivationKey = keyToStage;


                break;
            case KeyType.EquipKey:
                KeyBindings.DropEquipKey = keyToStage;
                break;
            case KeyType.FireModeSwapKey:
                KeyBindings.FireModeSwapKey = keyToStage;
                break;
            default:
                break;
        }

        _hand.UpdateKeyBindings(KeyBindings);
    }
}
