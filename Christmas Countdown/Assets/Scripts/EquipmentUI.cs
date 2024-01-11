using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI equipmentText;

    [SerializeField] private EquipmentSystemController controller;

     private RectTransform rectTransform;

    [SerializeField] private float transformWidth;
    [SerializeField] private float transformHeight;

    [SerializeField] private Vector3 targetPosition;

    private bool isEnabled; 

    private string displayNameUI;

    private string fullString;
    public string DisplayNameUI
    {
        get { return displayNameUI; }
    }

    public void InitializeEquipmentUI(EquipmentBehaviour equipmentBehaviour)
    {
        rectTransform = equipmentText.GetComponent<RectTransform>();

        displayNameUI = equipmentBehaviour.EquipmentData.EquipmentName;
        SetTransformProperties();
        EnableEquipmentUI();

        string equipOrSwapLeft = GetEquipOrSwapString(controller.LeftHand);
        string equipOrSwapRight = GetEquipOrSwapString(controller.RightHand);

        fullString = $" {controller.LeftHandInputKey} {equipOrSwapLeft} Left   |   {controller.RightHandInputKey} {equipOrSwapRight} Right";

        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        equipmentText.transform.SetParent(canvas.transform,true);
        equipmentText.transform.localPosition = targetPosition;
    }

    public void EnableEquipmentUI()
    {
        if (isEnabled)
        {
            return;
        } else
        {
            Debug.Log($"enable { gameObject.name} ");
            equipmentText.enabled = true;
            UpdateText(displayNameUI, fullString);
            //isEnabled = true;
        }
    }

    public void DisableEquipmentUI()
    {
        /*  if (!isEnabled)
          {
              return;
          }
          else
          {
              Debug.Log("disable");

              equipmentText.enabled = false;
              isEnabled = false;
          }*/

        UpdateText(string.Empty, string.Empty);
    }
    
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


    private void UpdateText(string _equipmentName, string underText)
    {
    
        equipmentText.SetText($"{_equipmentName} \n {underText} ");
    }

    private void SetTransformProperties()
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, transformWidth);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, transformHeight);

        rectTransform.anchoredPosition = targetPosition;
    }
}
