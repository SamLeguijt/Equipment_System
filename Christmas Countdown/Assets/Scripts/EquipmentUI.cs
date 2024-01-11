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

    private string displayName;
    private string baseText;

    public string DisplayName
    { 
        get { return displayName; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = equipmentText.GetComponent<RectTransform>();
        baseText = $"  Equip Left {controller.LeftHandInputKey}  |  {controller.RightHandInputKey}  Equip Right";
        InitializeEquipmentUI("shotgun");
    }

    public void InitializeEquipmentUI(string _equipmentName)
    {
        displayName = _equipmentName;
        SetTransformProperties();
        UpdateText(displayName);
    }

    private void UpdateText(string _equipmentName)
    {
        equipmentText.SetText($"{_equipmentName} \n {baseText} ");
    }

    private void SetTransformProperties()
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, transformWidth);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, transformHeight);

        rectTransform.anchoredPosition = targetPosition;
    }
}
