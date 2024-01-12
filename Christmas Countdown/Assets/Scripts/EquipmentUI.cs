using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{

    public TextMeshProUGUI textObject;

    [SerializeField] private EquipmentSystemController controller;

    private RectTransform rectTransform;

    [SerializeField] private float transformWidth;
    [SerializeField] private float transformHeight;

    [SerializeField] private Vector3 targetPosition;

    private bool isTextDisplayed;

    private string equipmentNameToDisplay;

    private string fullString;
    public string EquipmentNameToDisplay
    {
        get { return equipmentNameToDisplay; }
    }

    public EquipmentBehaviour currentEquipmentTarget;

    bool canDisplay;
    private void Start()
    {
        InitializeEquipmentUI();
    }
    public void InitializeEquipmentUI()
    {
        rectTransform = textObject.GetComponent<RectTransform>();

        string equipOrSwapLeft = GetEquipOrSwapString(controller.LeftHand);
        string equipOrSwapRight = GetEquipOrSwapString(controller.RightHand);

        fullString = $" {controller.LeftHandInputKey} {equipOrSwapLeft} Left   |   {controller.RightHandInputKey} {equipOrSwapRight} Right";

        SetTransformProperties();

        DisableEquipmentUI();
    }


    private bool IsMouseOverCollider(Collider _colliderToCheck)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast a ray from the mouse position
        if (Physics.Raycast(ray, out hit) && hit.collider == _colliderToCheck)
        {
            // Mouse is over the specified collider
            return true;
        }

        // Mouse is not over the specified collider
        return false;
    }


    private void Update()
    {
        if (currentEquipmentTarget != null)
        {
            if (!IsMouseOverCollider(currentEquipmentTarget.MouseDetectCollider))
            {
                DisableEquipmentUI();
            }
            else
            {
                if (currentEquipmentTarget.IsOnGround)
                {
                    EnableEquipmentUI(equipmentNameToDisplay, fullString);
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
        if (isTextDisplayed)
        {
            return;
        }
        else
        {
            UpdateEquipmentText(_equipmentName, _instructionsText);
            isTextDisplayed = true;
        }
    }

    /// <summary>
    /// Public method to Disable the UI text for the equipment <br/>
    /// Specific for equipment UI only, bacause it updates the text to empty strings 
    /// </summary>
    public void DisableEquipmentUI()
    {
        if (!isTextDisplayed)
        {
            return;
        }
        else
        {
            UpdateEquipmentText(string.Empty, string.Empty);
            isTextDisplayed = false;
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
            equipmentNameToDisplay = _equipment.EquipmentData.EquipmentName;

            fullString = GetInstructionsString();
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
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, transformWidth);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, transformHeight);

        // Set the position to the desired target position (position based on anchors)
        rectTransform.anchoredPosition = targetPosition;
    }
}
