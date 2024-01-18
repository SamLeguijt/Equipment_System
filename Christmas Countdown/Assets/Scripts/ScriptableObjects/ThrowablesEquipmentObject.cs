using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating new Throwables equipment scriptable objects
/// Deriving from base equipment scriptable object class
/// Sets type automatically on enable
/// </summary>
[CreateAssetMenu(fileName = "New Throwable Equipment", menuName = "Equipment/Throwable")]
public class ThrowablesEquipmentObject : BaseEquipmentObject
{

    public Vector3 throwForce;

    public float throwDelaySeconds; 

    public float throwForceValue;

    public float MaxThrowDistance;

    public float distanceDivider;
    /// <summary>
    /// Implement abstract OnEnable function from base class to set type
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Throwable;
    }
}
