using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "New Flashlight Equipment", menuName = "Equipment/Tool/Flashlight")]
public class FlashlightObject : ToolEquipmentObject
{
    [Header("Light values:")]
    [Space]

    [Tooltip("The angle of the light spot")]
    [SerializeField] private float lightRange;
    [SerializeField] private float spotAngle;
    [SerializeField] private float lightIntensity;
    [SerializeField] private float indirectMultiplier;

    [Header("Light settings:")]
    [Space]
    [SerializeField] private UnityEngine.LightType lightType;
    [SerializeField] private LightMode lightMode;
    [SerializeField] LightShadows lightShadowType;
    [SerializeField] private bool drawLightHalo;

    [SerializeField] LightRenderMode lightRenderMode;

    [SerializeField] LayerMask lightCullingmask;

    protected override void OnEnable()
    {
        ToolType = TypeOfTool.Flashlight;
    }
}
