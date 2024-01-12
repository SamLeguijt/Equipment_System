using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    [Tooltip("TMP object attached to Canvas")]
    [SerializeField] private TextMeshProUGUI textObject;

    [Tooltip("EquipmentSystemController object in scene")]
    [SerializeField] private EquipmentSystemController controller;

    [Tooltip("Desired width of the RectTransform of the TMP object")]
    [SerializeField] private float rectTransformWidth;

    [Tooltip("Desired height of the RectTransform of the TMP object")]
    [SerializeField] private float rectTransformHeight;

    [Tooltip("Desired position of the RectTransform of the TMP object")]
    [SerializeField] private Vector3 rectTargetPosition;

    // The current equipment being targeted by the UI (mouse, name, collider)
    private EquipmentBehaviour currentEquipmentTarget;

    // Reference to the RectTransform component of the TMP object
    private RectTransform rectTransform;

    // String that holds a reference to the name of the current equipment, ui displays the name
    private string currentEquipmentNameToDisplay;

    // string that holds the instructions for player interaction, displayed below the name of equipment
    private string interactInstructionsString;

    // Private bool to use for optimizing 
    private bool isTextEnabled;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Read only property for reference to the TMP object
    /// </summary>
    public TextMeshProUGUI TextObject
    {
        get { return textObject; }  
    }

    /// <summary>
    /// Read-only property to get the width of the RectTransform
    /// </summary>
    public float RectTransformWidth
    {
        get { return rectTransformWidth; }
    }

    /// <summary>
    /// Read-only property to get the height of the RectTransform
    /// </summary>
    public float RectTransformHeight
    {
        get { return rectTransformHeight; }
    }

    /// <summary>
    /// Read-only property to get the target position of the RectTransform
    /// </summary>
    public Vector3 RectTargetPosition
    {
        get { return rectTargetPosition; }
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


    /* ------------------------------------------  METHODS ------------------------------------------- */



    private void Start()
    {
        InitializeEquipmentUI();
    }

    public void InitializeEquipmentUI()
    {
        rectTransform = textObject.GetComponent<RectTransform>();

        SetTransformProperties();

        DisableEquipmentUI();
    }


    private bool IsMouseOverCollider(Collider _colliderToCheck)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider == _colliderToCheck)
        {
            return true;
        }

        return false;
    }


    private void Update()
    {
        if (currentEquipmentTarget != null)
        {
            if (!IsMouseOverCollider(currentEquipmentTarget.MouseDetectCollider))
            {
                currentEquipmentTarget = null;
                DisableEquipmentUI();
            }
            else
            {
                if (currentEquipmentTarget.IsOnGround)
                {
                    EnableEquipmentUI(currentEquipmentNameToDisplay, interactInstructionsString);
                }
            }
        }
   
    }


    /// <summary>
    /// Public method to enable the UI text for equipment <br/>
    /// Specific for equipment only, because it updates the text obj to the name of equipment and equipText string
    /// </summary>
    public void EnableEquipmentUI(string _equipmentName, string _instructionsText)
    {
        if (isTextEnabled)
        {
            return;
        }
        else
        {
            UpdateEquipmentText(_equipmentName, _instructionsText);
            isTextEnabled = true;
        }
    }

    /// <summary>
    /// Public method to Disable the UI text for the equipment <br/>
    /// Specific for equipment UI only, bacause it updates the text to empty strings 
    /// </summary>
    public void DisableEquipmentUI()
    {
        if (!isTextEnabled)
        {
            return;
        }
        else
        {
            UpdateEquipmentText(string.Empty, string.Empty);
            isTextEnabled = false;
        }
    }

    private string GetInstructionsString()
    {
        string leftHandText = GetEquipOrSwapString(controller.LeftHand);
        string rightHandText = GetEquipOrSwapString(controller.RightHand);

        string fullString = $" {controller.LeftHandInputKey} {leftHandText} Left   |   {controller.RightHandInputKey} {rightHandText} Right";

        return fullString;
    }

    public void UpdateEquipmentInfo(EquipmentBehaviour _equipment)
    {
        if (currentEquipmentTarget != _equipment)
        {
            currentEquipmentTarget = _equipment;

            // Displlay the equipments name
            currentEquipmentNameToDisplay = _equipment.EquipmentData.EquipmentName;

            interactInstructionsString = GetInstructionsString();
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Returns a string either saying "Equip" or "Swap" based on current hand equipment for specified hand
    /// </summary>
    /// <param name="_hand"></param>
    /// <returns></returns>
    private string GetEquipOrSwapString(Hand _hand)
    {
        if (_hand.CurrentEquipment == null)
        {
            return "Equip";
        }
        else
        {
            return "Swap";
        }
    }

    /// <summary>
    /// Method that sets the text of the Text object to the given parameters
    /// </summary>
    /// <param name="_equipmentName"> name of the equipment to display</param>
    /// <param name="_instructionsText"> String to display under the equipment name </param>
    private void UpdateEquipmentText(string _equipmentName, string _instructionsText)
    {
        textObject.SetText($"{_equipmentName} \n {_instructionsText} ");
    }



    /// <summary>
    /// Method that sets the transform properties of the Text object to place in right position
    /// </summary>
    private void SetTransformProperties()
    {
        // Set the width and height of the RectTransform on both axis
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransformWidth);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransformHeight);

        // Set the position to the desired target position (position based on anchors)
        rectTransform.anchoredPosition = rectTargetPosition;
    }
}
