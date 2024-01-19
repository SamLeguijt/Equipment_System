using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionActivation : MonoBehaviour, IEquipmentActivation
{

    private EquipmentSystemController equipmentController; 
    private EquipmentBehaviour equipmentBehaviour;
    private AmmunitionEquipmentObject ammoData;

    public void Initialize(EquipmentBehaviour _equipment)
    {
        equipmentBehaviour = _equipment;
        equipmentBehaviour.activationLogic = this;

        equipmentController = equipmentBehaviour.EquipmentSystemController;

        ammoData = (AmmunitionEquipmentObject)equipmentBehaviour.EquipmentData;
    }


    public void Activate()
    {
        /* Check if the opposite hand is equipped
         * If it is, check what type of equipment it is
         * If its a weapon, do something
         * 
         */

        if (equipmentController.IsEquippedInOppositeHandOf(equipmentBehaviour.CurrentHand))
        {
            Debug.Log("Activationv 1: Opposite hand is equipped");
        }
    }
}
