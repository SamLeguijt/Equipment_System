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
    [Header("-----------------------  ApparelEquipment specifics   -----------------------------------")]
    [Space]

    [Tooltip("Type of this tool")]
    [SerializeField] private TypeOfApparel apparelType;



    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Type of this apparel
    /// </summary>
    public TypeOfApparel ApparelType
    {
        get { return apparelType; } 
        protected set { apparelType = value; } 
    }


    /* ------------------------------------------  METHODS ------------------------------------------- */


    /// <summary>
    /// Implement abstract OnEnable function from base class to set type when creating object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Apparel;
    }
}

/// <summary>
/// Public enum for types of apparel, outside of class for global access
/// </summary>
public enum TypeOfApparel
{
    Hat,
    Other
}
