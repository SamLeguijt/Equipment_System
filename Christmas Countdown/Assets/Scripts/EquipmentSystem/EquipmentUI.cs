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
    [Header("TMP ammo object position properties")]
    [Space]

    [Tooltip("Desired width of the RectTransform of the ammo TMP object")]
    [SerializeField] private float ammoTextObjectWidth;

    [Tooltip("Desired height of the RectTransform of the ammo TMP object")]
    [SerializeField] private float ammoTextObjectHeight;

    [Tooltip("Desired position of the RectTransform of the leftAmmo TMP object")]
    [SerializeField] private Vector3 leftAmmoTextObjectPosition;

    [Tooltip("Desired position of the RectTransform of the rightAmmo TMP object")]
    [SerializeField] private Vector3 rightAmmoTextObjectPosition;

    // The current equipment being targeted by the UI (mouse, name, collider)
    private EquipmentBehaviour currentEquipmentTarget;

    // Reference to the RectTransform component of the TMP object
    private RectTransform equipTextTransform;
    private RectTransform leftTextTransform;
    private RectTransform rightTextTransform;

    // String that holds a reference to the name of the current equipment, ui displays the name
    private string currentEquipmentNameToDisplay;

    // string that holds the instructions for player interaction, displayed below the name of equipment
    private string interactInstructionsString;

    // Keep track of the current ammo text color
    private Color currentAmmoTextColor;

    // Private bool to use for optimizing 
    private bool isTextEnabled;


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
    /// Read only property to get the string holding the interact instructions
    /// </summary>
    public string InteractInstructionsString
    {
        get { return interactInstructionsString; }
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
        // Get RectTransform from TMP object
        equipTextTransform = equipTextObject.GetComponent<RectTransform>();
        leftTextTransform = leftHandAmmoTextObject.GetComponent<RectTransform>();
        rightTextTransform = rightHandAmmoTextObject.GetComponent<RectTransform>();


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
    }

    /// <summary>
    /// Handles the different Texts being displayed 
    /// </summary>
    private void Update()
    {
        DisplayAmmoText(equipSystemController.LeftHand, leftHandAmmoTextObject);
        DisplayAmmoText(equipSystemController.RightHand, rightHandAmmoTextObject);
        HandleEquipText();
    }

    /// <summary>
    /// Method to display the ammo text of a hand if holding a weapon
    /// </summary>
    /// <param name="_handToCheck"></param>
    /// <param name="_tmpObjectForHand"></param>
    private void DisplayAmmoText(Hand _handToCheck, TextMeshProUGUI _tmpObjectForHand)
    {
        // Return if the hand to check is not holding an equipment of type weapon
        if (!equipSystemController.IsEquipmentTypeInHandOf(EquipmentType.Weapon, _handToCheck))
        {
            // Set the ammo text object inactive if its active
            if (_tmpObjectForHand.gameObject.activeSelf) _tmpObjectForHand.gameObject.SetActive(false);

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
                if (_tmpObjectForHand.gameObject.activeSelf == false) _tmpObjectForHand.gameObject.SetActive(true);
                else // Set object's text to the current ammo of the weapon being held every frame
                {
                    // Get the text to display
                    string textToDisplay = GetAmmoTextString(weaponInfo);

                    // Call method to display the text for the correct object, with the color of the current bullet
                    UpdateText(_tmpObjectForHand, textToDisplay, weaponInfo.CurrentBulletColor);
                }
            }
        }
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

        // Keep track of the current color of the text
        currentAmmoTextColor = text.color;  
    }

    /// <summary>
    /// Method that updates the text of param _targetObject to param _targetText, updating color to _targetColor if not current color
    /// </summary>
    /// <param name="_targetTextObject"></param>
    /// <param name="_targetText"></param>
    /// <param name="_targetColor"></param>
    private void UpdateText(TextMeshProUGUI _targetTextObject, string _targetText, Color _targetColor)
    {
        // Update the text color if its a different color from the current one
        if (_targetColor != currentAmmoTextColor)
        {
            UpdateTextColor(_targetTextObject, _targetColor);
        }

        // Set the text for the object to the target text
        _targetTextObject.SetText(_targetText);
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


    private void HandleEquipText()
    {
        // Return immediatily if there is no equipment target
        if (currentEquipmentTarget == null) return;
        else // There is a target
        {
            // Check if the mouse is over the MouseDetectCollider of the equipment, and if the equipment is grounded (prevent ui display when object is in air)
            if (IsMouseOverCollider(currentEquipmentTarget.MouseDetectCollider) && (currentEquipmentTarget.IsOnGround))
            {
                // Enable the UI text by calling method, passing the current equipment name and the interact string as text
                EnableEquipmentUI(currentEquipmentNameToDisplay, interactInstructionsString);

            }
            else // Mouse is not over collider, or equipment is not grounded so dont displlay text 
            {
                // Remove the target (enables early return)
                currentEquipmentTarget = null;

                // Call method to disable the UI text
                DisableEquipmentUI();
            }
        }
    }

    /// <summary>
    ///  Public method to enable the equipment UI text <br/>
    ///  Receives two parameters as display text
    /// </summary>
    /// <param name="_equipmentName"> Name of the equipment to display </param>
    /// <param name="_interactInstructions"> String holding instructions for interacting </param>
    private void EnableEquipmentUI(string _equipmentName, string _interactInstructions)
    {
        // Prevent unnecessary calls if already enabled 
        if (isTextEnabled) return;
        else // Text is not yet enabled
        {
            // Call method to display the new text, passing in both strings
            UpdateTextBox(_equipmentName, _interactInstructions, newLinesAmount); // True to add a new line between strings

            // Set bool for early returns if more calls to this method
            isTextEnabled = true;
        }
    }


    /// <summary>
    /// Method that disables the equipment UI text by updating the text with empty strings
    /// </summary>
    private void DisableEquipmentUI()
    {
        // Prevent unnecessary calls if already disabled 
        if (!isTextEnabled) return;
        else // Not disabled yet
        {
            // Call method to update the text box with empty strings (works better than disabling the object, that causes lag on re-enabling)
            UpdateTextBox(string.Empty, string.Empty, 0);

            // Set bool for early returns when calling method again without changes
            isTextEnabled = false;
        }
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

            // Update the InteractInstruction string to display based on hand status'
            interactInstructionsString = GetInteractionInstructionString();
        }
        else // _newEquipment is already the current target, so no need to update everything
        {
            return;
        }
    }


    /// <summary>
    /// Method that sets the text of the TMP object to the two input strings params <br/>
    /// Adds empty lines according to param
    /// </summary>
    /// <param name="_string1"> First string to display </param>
    /// <param name="_string2"> Second string to display</param>
    /// <param name="_newLinesAmount"> How many empty lines should be added between the two strings </param>
    private void UpdateTextBox(string _string1, string _string2, int _newLinesAmount)
    {
        // Create new empty string
        string newLines = "";

        // Loop through the new lines amount
        for (int i = 0; i < _newLinesAmount; i++)
        {
            // Add new line to the string
            newLines += "\n";
        }

        // Set the text to the two strings, with the amount of new lines between them
        equipTextObject.SetText($"{_string1} {newLines} {_string2}");
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
}
