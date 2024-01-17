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

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    [Space]
    [Header("-----------------------  ToolEquipment specifics   -----------------------------------")]
    [Space]

    [Tooltip("Type of this tool")]
    [SerializeField] private TypeOfTool toolType;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /// <summary>
    /// Get type of tool, protected set to set in children classes
    /// </summary>
    public TypeOfTool ToolType
    {
        get { return toolType; }
        protected set { toolType = value; }
    }


    /* ------------------------------------------  METHODS ------------------------------------------- */


    /// <summary>
    /// Implement abstract OnEnable function from base class to set type when creating object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Tool;
    }
}

/// <summary>
/// Enum for making types of tools, public and outside of class for global access 
/// </summary>
public enum TypeOfTool
{
    Other,
    Flashlight
}