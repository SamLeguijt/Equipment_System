using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionActivation : MonoBehaviour, IEquipmentActivation
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    // Reference to the equipment controller
    private EquipmentSystemController equipmentController;

    // Reference to this equipment's behaviour
    private EquipmentBehaviour equipmentBehaviour;

    // Reference to this equipment's data
    private AmmunitionEquipmentObject ammoData;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /* ------------------------------------------  METHODS ------------------------------------------- */



    public void Initialize(EquipmentBehaviour _equipment)
    {
        // Set equipment's behaviour to received param
        equipmentBehaviour = _equipment;

        // Set the activation logic of the equipment to this to call this activate method
        equipmentBehaviour.activationLogic = this;

        // Get the equipmentController from the behaviour class
        equipmentController = equipmentBehaviour.EquipmentSystemController;

        // Get the script object from the behaviour class, casted to correct type
        ammoData = (AmmunitionEquipmentObject)equipmentBehaviour.EquipmentData;
    }


    public void Activate()
    {
        // Get the opposite hand of the hand this equipment is in
        Hand oppositeHand = GetOppositeHandOf(equipmentBehaviour.CurrentHand);

        // Check if the opposite hand is holdng a weapon
        if (equipmentController.IsEquipmentTypeInHandOf(EquipmentType.Weapon, oppositeHand))
        {
            // Get the weapon activation script to call reload method
            WeaponActivation weapon = oppositeHand.GetComponentInChildren<WeaponActivation>();

            // Check if weapon is not null, and if the targetweapon matches the weapon's type, or if the targetweapon is all 
            if (weapon != null && (weapon.WeaponData.WeaponType == ammoData.TargetWeapon || ammoData.TargetWeapon == TypeOfWeapon.All))
            {
                // Call method to reload the weapon, using this ammo's data
                weapon.Reload(ammoData);

                // Get parent object 
                GameObject parent = gameObject.transform.parent.gameObject;

                // Destroy the whole object upon reloading
                Destroy(parent, ammoData.DestroyAfterActivationDelay);
            }
        }
        else // No weapon in other hand, so return
        {
            return;
        }
    }

    /// <summary>
    /// Returns the Hand of the equipmentcontroller opposite of param _hand
    /// </summary>
    /// <param name="_hand"></param>
    /// <returns></returns>
    private Hand GetOppositeHandOf(Hand _hand)
    {
        // Param is left hand
        if (_hand == equipmentController.LeftHand)
        {
            // So return right hand
            return equipmentController.RightHand;
        }
        // Param is the right hand
        else if (_hand == equipmentController.RightHand)
        {
            // So return the left hand
            return equipmentController.LeftHand;
        }
        else // Param hand is not one of the equipmentController hands
        {
            Debug.LogError($"_hand param not recognized by equipmentController for {gameObject.name}, returning null");
            return null;
        }
    }
}
