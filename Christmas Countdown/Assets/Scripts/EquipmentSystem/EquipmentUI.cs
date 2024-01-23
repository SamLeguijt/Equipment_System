using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Header("Component references")]
    [Space]

    [Tooltip("TMP object attached to Canvas")]
    [SerializeField] private TextMeshProUGUI equipTextObject;

    [Tooltip("TMP object attached to Canvas")]
    [SerializeField] private TextMeshProUGUI leftHandAmmoTextObject;

    [Tooltip("TMP object attached to Canvas")]
    [SerializeField] private TextMeshProUGUI rightHandAmmoTextObject;
    [SerializeField] private TextMeshProUGUI leftFireModeTextObject;
    [SerializeField] private TextMeshProUGUI rightFireModeTextObject;
    [Space]

    [Tooltip("EquipmentSystemController object in scene")]
    [SerializeField] private EquipmentSystemController equipSystemController;

    [Space]
    [Header("TMP equip object position properties")]
    [Space]

    [Tooltip("Desired width of the RectTransform of the TMP object")]
    [SerializeField] private float equipTextObjectWidth;

    [Tooltip("Desired height of the RectTransform of the TMP object")]
    [SerializeField] private float equipTextObjectHeight;

    [Tooltip("Desired position of the RectTransform of the TMP object")]
    [SerializeField] private Vector3 equipTextObjectPosition;


    [Space]
    [Header("Displayed text properties")]
    [Space]

    [Tooltip("The word being displayed as text when hand is free ( {inputKey} to x Left hand/Right hand")]
    [SerializeField] private string equipActionWord;

    [Tooltip("The word being displayed as text when hand is not free ( {inputKey} to x Left hand/Right hand")]
    [SerializeField] private string swapActionWord;

    [Tooltip("Amount of empty lines between the weapon name and instructions string")]
    [SerializeField] private int newLinesAmount = 1; // Default to 1

    [Space]
    [Header("TMP ammo object transform properties")]
    [Space]

    [Tooltip("Desired width of the RectTransform of the ammo TMP object")]
    [SerializeField] private float ammoTextObjectWidth;

    [Tooltip("Desired height of the RectTransform of the ammo TMP object")]
    [SerializeField] private float ammoTextObjectHeight;

    [Tooltip("Desired position of the RectTransform of the leftAmmo TMP object")]
    [SerializeField] private Vector3 leftAmmoTextObjectPosition;

    [Tooltip("Desired position of the RectTransform of the rightAmmo TMP object")]
    [SerializeField] private Vector3 rightAmmoTextObjectPosition;

    [Space]
    [Header("TMP FireMode object transform properties")]
    [Space]

    [Tooltip("Desired width of the RectTransform of the fire mode TMP object")]
    [SerializeField] private float fireModeTextWidth;

    [Tooltip("Desired height of the RectTransform of the fire mode TMP object")]
    [SerializeField] private float fireModeTextHeight;

    [Tooltip("Desired position of the RectTransform of the left fire mode TMP object")]
    [SerializeField] private Vector3 leftFireModeTextPosition;

    [Tooltip("Desired position of the RectTransform of the right fire mode TMP object")]
    [SerializeField] private Vector3 rightFireModeTextPosition;

    // The current equipment being targeted by the UI (mouse, name, collider)
    private EquipmentBehaviour currentEquipmentTarget;

    // Reference to the RectTransform component of the TMP object
    private RectTransform equipTextTransform;
    private RectTransform leftTextTransform;
    private RectTransform rightTextTransform;
    private RectTransform leftFireModeTextTransform;
    private RectTransform rightFireModeTextTransform;

    // String that holds a reference to the name of the current equipment, ui displays the name
    private string currentEquipmentNameToDisplay;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Read only property for reference to the TMP object
    /// </summary>
    public TextMeshProUGUI TextObject
    {
        get { return equipTextObject; }
    }

    /// <summary>
    /// Read-only property to get the width of the RectTransform
    /// </summary>
    public float RectTransformWidth
    {
        get { return equipTextObjectWidth; }
    }

    /// <summary>
    /// Read-only property to get the height of the RectTransform
    /// </summary>
    public float RectTransformHeight
    {
        get { return equipTextObjectHeight; }
    }

    /// <summary>
    /// Read-only property to get the target position of the RectTransform
    /// </summary>
    public Vector3 RectTargetPosition
    {
        get { return equipTextObjectPosition; }
    }

    /// <summary>
    /// Read only reference to the EquipmentBehaviour that's being targeted by this ui 
    /// </summary>
    public EquipmentBehaviour CurrentEquipmentTarget
    {
        get { return currentEquipmentTarget; }
    }

    /// <summary>
    /// Read only string property to get the name of the current equipment thats being displayed
    /// </summary>
    public string CurrentEquipmentNameString
    {
        get { return currentEquipmentNameToDisplay; }
    }

    /// <summary>
    /// Public property to get or set the word being displayed to equip/pick up equipment in either hand
    /// </summary>
    public string EquipActionWord
    {
        get { return equipActionWord; }
        set { equipActionWord = value; }
    }

    /// <summary>
    /// Public property to get or set the word being displayed to swap an equipment in either hand
    /// </summary>
    public string SwapActionWord
    {
        get { return swapActionWord; }
        set { swapActionWord = value; }
    }



    /* ------------------------------------------  METHODS ------------------------------------------- */


    /// <summary>
    /// Handles getting components and Initializing the UI object, or else logging an error and disabling the gameobject
    /// </summary>
    private void Start()
    {
        // Get RectTransform from TMP objects
        equipTextTransform = equipTextObject.GetComponent<RectTransform>();
        leftTextTransform = leftHandAmmoTextObject.GetComponent<RectTransform>();
        rightTextTransform = rightHandAmmoTextObject.GetComponent<RectTransform>();
        leftFireModeTextTransform = leftFireModeTextObject.GetComponent <RectTransform>();
        rightFireModeTextTransform = rightFireModeTextObject.GetComponent<RectTransform>();


        if (equipTextObject != null && equipTextTransform != null && leftTextTransform != null && rightTextTransform != null && equipSystemController != null)
        {
            // Call method to initialize if all references are found 
            InitializeEquipmentUI();
        }
        else // One or more missing references, log error and disable this object
        {
            gameObject.SetActive(false);
            throw new System.Exception($"One or more components and references are missing from {gameObject.name}! Please assign components, then re-run. Disabling object for now.");
        }
    }

    /// <summary>
    /// Method that sets up the script by getting components and setting values of the transform
    /// </summary>
    public void InitializeEquipmentUI()
    {
        // Set the RectTransform properties to set UI text at correct position in game view
        SetTransformProperties(equipTextTransform, equipTextObjectPosition, equipTextObjectWidth, equipTextObjectHeight);

        SetTransformProperties(leftTextTransform, leftAmmoTextObjectPosition, ammoTextObjectWidth, ammoTextObjectHeight);
        SetTransformProperties(rightTextTransform, rightAmmoTextObjectPosition, ammoTextObjectWidth, ammoTextObjectHeight);

        SetTransformProperties(leftFireModeTextTransform, leftFireModeTextPosition, fireModeTextWidth, fireModeTextHeight);
        SetTransformProperties(rightFireModeTextTransform, rightFireModeTextPosition, fireModeTextWidth, fireModeTextHeight);

        DisableEquipText();

        // Disable the UI components and this if dev settings wants
        if (DeveloperSettings.instance.DisableEquipmentUI_OnStart)
        {
            equipTextObject.gameObject.SetActive(false);
            leftHandAmmoTextObject.gameObject.SetActive(false);
            rightHandAmmoTextObject.gameObject.SetActive(false);
            leftFireModeTextObject.gameObject.SetActive(false);
            rightFireModeTextObject.gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    /// <summary>
    /// Method that sets the transform properties of the Text object to place in the right position
    /// </summary>
    private void SetTransformProperties(RectTransform _target, Vector3 _targetPosition, float _targetWidth, float _targetHeight)
    {
        // Set the width and height of the RectTransform on both axis
        _target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _targetWidth);
        _target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _targetHeight);

        // Set the position to the desired target position (position based on anchors)
        _target.anchoredPosition = _targetPosition;
    }

    /// <summary>
    /// Handles the different Texts being displayed 
    /// </summary>
    private void Update()
    {
        // Call method to handle the text for equipping/swapping items
        HandleEquipText();

        // Call method to handle the display of current and max ammo of weapons
        HandleAmmoText(equipSystemController.LeftHand, leftHandAmmoTextObject);
        HandleAmmoText(equipSystemController.RightHand, rightHandAmmoTextObject);
       
        // Call method to handle the display of weapon fire types
        HandleFireModeText(equipSystemController.LeftHand, leftFireModeTextObject);
        HandleFireModeText(equipSystemController.RightHand, rightFireModeTextObject);
    }

    /// <summary>
    /// Method that updates the CurrentEquipment to a new equipment object <br/>
    /// Sets name to new equipments name, and gets a new Instruction string to display
    /// </summary>
    /// <param name="_newEquipment"> The equipment info being used to update the variables, called in EquipmentBehaviour class upon mouse collision and in range </param>
    public void UpdateEquipmentInfo(EquipmentBehaviour _newEquipment)
    {
        // No equipment is being targeted currently
        if (CurrentEquipmentTarget == null)
        {
            // So set target to the new equipment 
            currentEquipmentTarget = _newEquipment;

            // Store the name of the new equipment by looking at its scriptable object
            currentEquipmentNameToDisplay = _newEquipment.EquipmentData.EquipmentName;
        }
        else // _newEquipment is already the current target, so no need to update everything
        {
            return;
        }
    }

    /// <summary>
    /// Method to display the ammo text of a hand if holding a weapon
    /// </summary>
    /// <param name="_handToCheck"></param>
    /// <param name="_handAmmoTMP"></param>
    private void HandleAmmoText(Hand _handToCheck, TextMeshProUGUI _handAmmoTMP)
    {
        // Return if the hand to check is not holding an equipment of type weapon
        if (!equipSystemController.IsEquipmentTypeInHandOf(EquipmentType.Weapon, _handToCheck))
        {
            // Set the ammo text object inactive if its active
            if (_handAmmoTMP.gameObject.activeSelf) _handAmmoTMP.gameObject.SetActive(false);

            // Early return
            return;
        }
        else // Weapon is in hand
        {
            // Get the weaponactivation script for its current info
            WeaponActivation weaponInfo = _handToCheck.CurrentEquipment.GetComponentInChildren<WeaponActivation>();

            // Prevent error if  activation script is not found
            if (weaponInfo != null)
            {
                // Set the object active if not active
                if (_handAmmoTMP.gameObject.activeSelf == false) _handAmmoTMP.gameObject.SetActive(true);
                else // Set object's text to the current ammo of the weapon being held every frame
                {
                    // Get the text to display
                    string textToDisplay = GetAmmoTextString(weaponInfo);

                    // Call method to display the text for the correct object, with the color of the current bullet
                    UpdateText(_handAmmoTMP, textToDisplay, weaponInfo.CurrentBulletColor);
                }
            }
        }
    }


    /// <summary>
    /// Method that handles displaying the fire mode text
    /// </summary>
    /// <param name="_hand"></param>
    /// <param name="_fireModeTMP"></param>
    private void HandleFireModeText(Hand _hand, TextMeshProUGUI _fireModeTMP)
    {
        // Check if hand is holding a weapon object
        if (equipSystemController.IsEquipmentTypeInHandOf(EquipmentType.Weapon, _hand))
        {
            // Get the activation script for weapon infp
            WeaponActivation weaponInfo = _hand.CurrentEquipment.GetComponentInChildren<WeaponActivation>();

            // Prevent error if not found
            if (weaponInfo != null)
            {
                // Update text of the tmp object tot the current fire mode as string
                string fireModeString = GetFireModeTextString(weaponInfo.CurrentFireMode);

                UpdateText(_fireModeTMP, fireModeString, weaponInfo.CurrentBulletColor);

                // Set TMP object active if not yet
                if (!_fireModeTMP.gameObject.activeSelf)
                {
                    _fireModeTMP.gameObject.SetActive(true);
                }
            }
        }
        else // Not holding a weapon object, so disable the UI object
        {
            _fireModeTMP.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Handles displaying the  equip UI text based on conditions
    /// </summary>
    private void HandleEquipText()
    {
        // Return immediatily if there is no equipment target
        if (currentEquipmentTarget == null)
        {
            return;
            //DisableEquipText();
        }
        else // There is a target
        {
            // Check if the mouse is over the MouseDetectCollider of the equipment, and if the equipment is grounded (prevent ui display when object is in air)
            if (IsMouseOverCollider(currentEquipmentTarget.MouseDetectCollider) && (currentEquipmentTarget.IsOnGround))
            {
                // Enable the UI text by calling method, passing the current equipment name and the interact string as text
                EnableEquipText();
            }
            else // Mouse is not over collider, or equipment is not grounded so dont display text 
            {
                currentEquipmentTarget = null;
                DisableEquipText();
            }
        }
    }

    /// <summary>
    ///  Public method to enable the equipment UI text <br/>
    ///  Receives two parameters as display text
    /// </summary>
    /// <param name="_equipmentName"> Name of the equipment to display </param>
    /// <param name="_interactInstructions"> String holding instructions for interacting </param>
    private void EnableEquipText()
    {
        // Prevent unnecessary calls if already enabled 
        if (equipTextObject.gameObject.activeSelf) return;
        else // Text is not yet enabled
        {
            // Get the full string to display
            string fullEquipString = GetFullEquipTextString(newLinesAmount);

            // Call method to update the text with the new string
            UpdateText(equipTextObject, fullEquipString, equipTextObject.color);

            // Set object active to display
            equipTextObject.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Method that disables the equipment UI text by updating the text with empty strings
    /// </summary>
    private void DisableEquipText()
    {
        // Prevent unnecessary calls if already disabled 
        if (!equipTextObject.gameObject.activeSelf) return;
        else // Not disabled yet
        {
            equipTextObject.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Method that updates the text of param _targetObject to param _targetText, updating color to _targetColor if not current color
    /// </summary>
    /// <param name="_targetTextObject"></param>
    /// <param name="_targetText"></param>
    /// <param name="_targetColor"></param>
    private void UpdateText(TextMeshProUGUI _targetTextObject, string _targetText, Color _targetColor)
    {
        if (_targetTextObject.GetComponent<TMP_Text>().color != _targetColor)
        {
            UpdateTextColor(_targetTextObject, _targetColor);
        }

        // Set the text for the object to the target text
        _targetTextObject.SetText(_targetText);
    }

    /// <summary>
    /// Method that updates the text color of param _TMP object to param _targetColor
    /// </summary>
    /// <param name="_targetObject"></param>
    /// <param name="_targetColor"></param>
    private void UpdateTextColor(TextMeshProUGUI _targetObject, Color _targetColor)
    {
        TMP_Text text = _targetObject.GetComponent<TMP_Text>();

        text.color = _targetColor;
    }
    /// <summary>
    /// Returns a string containing the current ammo and max ammo of the param _weapon
    /// </summary>
    /// <param name="_weaponInfo"></param>
    /// <returns></returns>
    private string GetAmmoTextString(WeaponActivation _weaponInfo)
    {
        // Get current ammo as string
        string currentAmmoString = _weaponInfo.CurrentAmmoCapacity.ToString();

        // Get max ammo capacity of the weapon as string
        string maxAmmoString = _weaponInfo.WeaponData.MaxAmmoCapacity.ToString();

        // Make full string combining the two strings, with a ' /'  in between for clarity
        string fullString = $"{currentAmmoString} / {maxAmmoString}";

        // Return the full string
        return fullString;
    }

    /// <summary>
    /// Returns the full string being displayed when mouse is over equipment <br/>
    /// Combines the equipment name with x new lines and an instruction string
    /// </summary>
    /// <param name="_newLinesAmount"></param>
    /// <returns></returns>
    private string GetFullEquipTextString(int _newLinesAmount)
    {
        string equipmentName = currentEquipmentNameToDisplay;
        string instructionText = GetInteractionInstructionString();

        // Create new empty string
        string newLines = "";

        // Loop through the new lines amount
        for (int i = 0; i < _newLinesAmount; i++)
        {
            // Add new line to the string
            newLines += "\n";
        }

        string fullString = $"{equipmentName} {newLines} {instructionText}";

        return fullString;
    }

    /// <summary>
    /// Returns the fire mode as string, adjusted for readability
    /// </summary>
    /// <param name="_fireMode"></param>
    /// <returns></returns>
    private string GetFireModeTextString(WeaponFireMode _fireMode)
    {
        string fullString = "";

        // Switch between the types of firemodes
        // Return the fire mode, but hardcoded rename that fits better (can't use '-' in enum)
        switch (_fireMode)
        {
            case WeaponFireMode.SemiAutomatic:

                fullString = "Semi-Auto";
                break;
            case WeaponFireMode.FullAutomatic:
                fullString = "Full-Auto";
                break;
            case WeaponFireMode.BurstFire:
                fullString = "Burst-Fire";
                break;
            default:
                fullString = _fireMode.ToString();
                break;
        }

        // Return the string
        return fullString;
    }

    /// <summary>
    /// Returns a string holding interact instructions to the hands of the player <br/>
    /// Gets the input key for both hands, as well as a text based on the equipment status of the hand itself ("Equip", "Swap") <br/>
    /// Gets called when new equipment object gets targeted 
    /// </summary>
    /// <returns></returns>
    private string GetInteractionInstructionString()
    {
        // Get string based on equip status for both hands
        string leftHandText = GetEquipOrSwapString(equipSystemController.LeftHand); // Uses the references in the EquipmentSystemController to both hands
        string rightHandText = GetEquipOrSwapString(equipSystemController.RightHand);

        // Make the full string by combining the input keys for the hands, with the equip status, and the type of hand, with a symbol between them for eyecandy 
        string fullString = $" {equipSystemController.LeftHandEquipDropKey} {leftHandText} Left   |   {equipSystemController.RightHandEquipDropKey} {rightHandText} Right";

        // Return the full string as Instruction
        return fullString;
    }


    /// <summary>
    /// Returns a string containing string to "equip" or "swap", based on equipment status of the param hand <br/>
    /// Gets called when getting a new Instructions string
    /// </summary>
    /// <param name="_hand"> The hand to look at it's CurrentEquipment </param>
    /// <returns></returns>
    private string GetEquipOrSwapString(Hand _hand)
    {
        // If its null, there is no equipment yet
        if (_hand.CurrentEquipment == null)
        {
            // So return the string holding the word for equip (or similar)
            return equipActionWord;
        }
        else // The hand is holding an equipment
        {
            // So return string holding the word for swap (or similar)
            return swapActionWord;
        }
    }

    /// <summary>
    /// Returns true if the mouse is over the param collider
    /// </summary>
    /// <param name="_colliderToCheck"></param>
    /// <returns></returns>
    private bool IsMouseOverCollider(Collider _colliderToCheck)
    {
        // Shoot a ray from the camera through the mouse 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If the ray hits something, and the collider of the hit is same as our param
        if (Physics.Raycast(ray, out hit) && hit.collider == _colliderToCheck)
        {
            // The mouse is over the collider 
            return true;
        }

        // Ray did not hit our collider
        return false;
    }

}
