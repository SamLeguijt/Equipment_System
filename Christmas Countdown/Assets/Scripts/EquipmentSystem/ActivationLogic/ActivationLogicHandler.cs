using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ActivationLogicHandler : MonoBehaviour
{
    [Tooltip("Reference to the transform attached to this object as child, used for many Activation scripts (firepoint i.e)")]
    [SerializeField] private Transform equipmentSpecificTransform;

    [Tooltip("Seconds before destroying this object after being initialized")]
    [SerializeField] private float destroyThisAfterSeconds;

    // Reference to the equipment behaviour attached on the EquipmentObject
    private EquipmentBehaviour equipmentBehaviour;

    /// <summary>
    /// Method gets called by EquipmentBehaviour class that passes itself as param for safety 
    /// </summary>
    /// <param name="_equipment"></param>
    public void Initialize(EquipmentBehaviour _equipment)
    {
        // Set reference to the param
        equipmentBehaviour = _equipment;

        // Call method to add the correct activation logic
        AddActivationLogic(equipmentBehaviour);

        //Destroy this object after adding the activation logic since we have no use of it no more
        Destroy(gameObject, destroyThisAfterSeconds);
    }

    /// <summary>
    /// Method that adds an activation script tot the behaviour's object, depending on the type of the equipment
    /// </summary>
    /// <param name="_equipment"></param>
    private void AddActivationLogic(EquipmentBehaviour _equipment)
    {
        // Switch statement to switch between the types of equipment
        switch (_equipment.EquipmentData.EquipmentType)
        {
            // Call specific methid to inittialize the corresponding Activation script for each case
            case EquipmentType.Weapon:
                InitializeWeaponActivation();
                break;
            case EquipmentType.Ammunition:
                InitializeAmmunitionActivation();
                break;
            case EquipmentType.Throwable:
                InitializeThrowableActivation();
                break;
            case EquipmentType.Tool:
                InitializeToolActivation();
                break;
            case EquipmentType.Apparel:
                InitializeApparelActivation();
                break;
            default:
                Debug.LogError($"Equipment type of {_equipment.gameObject.name} not recognized, can not activation script!");
                break;
        }
    }

    /// <summary>
    /// Method for Adding and initializing a WeaponActivation script to the gameobject
    /// </summary>
    private void InitializeWeaponActivation()
    {
        // Set the attached firpoint as parent to the gameobject of our behaviour 
        equipmentSpecificTransform.SetParent(equipmentBehaviour.transform, true); // Keep world position so changes in edit mode are kept, firepoint stays at the same position ;)

        // Add a WeaponActivation to the equipmentBehaviour's gameobject
        WeaponActivation activationScript = equipmentBehaviour.gameObject.AddComponent<WeaponActivation>();

        // Call Initialize method from the specific activation script, passing in the firepoint as firepoint
        activationScript.Initialize(equipmentBehaviour, equipmentSpecificTransform);
    }


    /// <summary>
    /// Method for Adding and initializing a AmmunitionActivation script to the gameobject
    /// </summary>
    private void InitializeAmmunitionActivation()
    {
        // Add the specif script to the behaviour's gameobject
        AmmunitionActivation activation = equipmentBehaviour.AddComponent<AmmunitionActivation>();

        // Set the equipmentBehaviour's reference to activation interface to the specific activation script
        equipmentBehaviour.activationLogic = activation;

        // Call initialize method from specific script
        activation.InitializeActivation();
    }

    /// <summary>
    /// Method for Adding and initializing a ThrowableActivation script to the gameobject
    /// </summary>
    private void InitializeThrowableActivation()
    {
        // Add the specif script to the behaviour's gameobject
        ThrowableActivation activation = equipmentBehaviour.AddComponent<ThrowableActivation>();

        // Set the equipmentBehaviour's reference to activation interface to the specific activation script
        equipmentBehaviour.activationLogic = activation;

        // Call initialize method from specific script
        activation.InitializeActivation();
    }

    /// <summary>
    /// Method for Adding and initializing a ToolActivation.cs script to the gameobject
    /// </summary>
    private void InitializeToolActivation()
    {
        // Set the attached firpoint as parent to the gameobject of our behaviour 
        equipmentSpecificTransform.SetParent(equipmentBehaviour.transform, true); // Keep world position so changes in edit mode are kept, firepoint stays at the same position ;)

        // Add the specif script to the behaviour's gameobject
        ToolActivation activation = equipmentBehaviour.AddComponent<ToolActivation>();

        // Call initialize method from specific script, sending the new light as reference
        activation.Initialize(equipmentBehaviour, equipmentSpecificTransform);
    }

    /// <summary>
    /// Method for Adding and initializing a ApparelActivation.cs script to the gameobject
    /// </summary>
    private void InitializeApparelActivation()
    {
        equipmentSpecificTransform.SetParent(equipmentBehaviour.transform, true); // Keep world position so changes in edit mode are kept, firepoint stays at the same position ;)

        // Add the specif script to the behaviour's gameobject
        ApparelActivation activation = equipmentBehaviour.AddComponent<ApparelActivation>();

        // Call initialize method from specific script
        activation.Initialize(equipmentBehaviour, equipmentSpecificTransform);
    }
}
