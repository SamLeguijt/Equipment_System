using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// Class for creating new Tool equipment scriptable objects
/// Deriving from base equipment scriptable object class
/// Sets type automatically on enable
/// </summary>
[CreateAssetMenu(fileName = "New Tool Equipment", menuName = "Equipment/Tool")]
public class ToolEquipmentObject : BaseEquipmentObject
{


    [Space]
    [Header("-----------------------  WeaponEquipment specifics   -----------------------------------")]
    [Space]

    private UnityEngine.LightType lightType;
    private float lightRange;
    private float spotAngle;
    private LightMode lightMode;
    private float lightIntensity;
    private float indirectMultiplier;
    
    LightShadows lightShadowType;
    private bool drawLightHalo;

private bool drawShadows;



    /// <summary>
    /// Implement abstract OnEnable function from base class to set type when creating object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Tool;

    }
}
