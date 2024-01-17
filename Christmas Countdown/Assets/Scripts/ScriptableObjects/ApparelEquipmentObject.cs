using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating new Apparel equipment scriptable objects
/// Deriving from base equipment scriptable object class
/// Sets type automatically on enable
/// </summary>
[CreateAssetMenu(fileName = "New Apparel Equipment", menuName = "Equipment/Apparel")]
public class ApparelEquipmentObject : BaseEquipmentObject
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    [Space]
    [Header("-----------------------  ToolEquipment specifics   -----------------------------------")]
    [Space]

    [Tooltip("Vector representing the position offset of this object when on head")]
    [SerializeField] private Vector3 onHeadPositionOffset;

    [Tooltip("Vector representing the rotation of this object when on head")]
    [SerializeField] private Vector3 onHeadRotation;

    [Tooltip("Vector representing the scale of this object when on head")]
    [SerializeField] private Vector3 onHeadScale;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Position offset of this object when on head (read-only)
    /// </summary>
    public Vector3 OnHeadPositionOffset { get { return onHeadPositionOffset; } }

    /// <summary>
    /// Rotation of this object when on head (read-only)
    /// </summary>
    public Vector3 OnHeadRotation { get { return onHeadRotation; } }

    /// <summary>
    /// Scale of this object when on head (read-only)
    /// </summary>
    public Vector3 OnHeadScale { get { return onHeadScale; } }



    /* ------------------------------------------  METHODS ------------------------------------------- */


    /// <summary>
    /// Implement abstract OnEnable function from base class to set type when creating object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Apparel;
    }
}
