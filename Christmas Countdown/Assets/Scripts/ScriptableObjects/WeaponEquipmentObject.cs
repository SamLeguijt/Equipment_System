using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating Weapon scriptable objects
/// Automatically sets weapon type in OnEnable function
/// </summary>
[CreateAssetMenu(fileName = "New Weapon Equipment", menuName = "Equipment/Weapon")]
public class WeaponEquipmentObject : BaseEquipmentObject
{
    public GameObject prefab;

    public IEquipmentActivation activationLogic;

    /// <summary>
    /// OnEnable method to set weapon type automatically when creating new object of this type
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Weapon;
    }

    public override void Activate()
    {

    }


    /* Create base class for Activation
     * Make child clases for weapon and tools
     * Assign variable of type base class to Behaviour script
     * Check what activation child it is 
     * Cast? then call Activate method from the 
     * 
     * 
     * Base class has Activate method. 
     * Child classes implement in own way.
     * When calling the Activate from the base class,
     * it does the child's activate method 
     * 
     */
}
