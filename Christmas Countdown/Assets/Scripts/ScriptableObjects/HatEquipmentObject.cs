using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hat Equipment", menuName = "Equipment/Apparel/Hat")]
public class HatEquipmentObject : ApparelEquipmentObject
{

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

    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Apparel;

        // Set type of apparel upon enabling
        ApparelType = TypeOfApparel.Hat;
    }

}
