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

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Space]
    [Header("-----------------------  ThrowableEquipment specifics   -----------------------------------")]
    [Space]

    [Tooltip("Delay when activating equipment to throwing it, in seconds")]
    [SerializeField] private float throwDelaySeconds;

    [Tooltip("Force to throw the equipment with")]
    [SerializeField] private float throwForceValue;

    [Tooltip("Maximum distance the equipment can get a point to throw to")]
    [SerializeField] private float maxThrowDistance;

    [Tooltip("Equipment curve gets measured by (distance / this), low= high curve, high= low curve")]
    [SerializeField] private float distanceDivider;



    /* ------------------------------------------  PROPERTIES ------------------------------------------- */



    /// <summary>
    /// Secondds delay when activating to throwing equipment, read-only
    /// </summary>
    public float ThrowDelaySeconds { get { return throwDelaySeconds;  } }

    /// <summary>
    /// Force to throw equipment with, read-only
    /// </summary>
    public float ThrowForceValue { get { return throwForceValue; } }    

    /// <summary>
    /// Maximum distance to get a point to throw to, read-only
    /// </summary>
    public float MaxThrowDistance { get { return maxThrowDistance; } }

    /// <summary>
    /// Divider to the distance when calculating Vector3.Up force apply, read-only
    /// </summary>
    public float DistanceDivider {  get { return distanceDivider; } }


    /* ------------------------------------------  METHODS ------------------------------------------- */



    /// <summary>
    /// Implement abstract OnEnable function from base class to set type
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Throwable;
    }
}
