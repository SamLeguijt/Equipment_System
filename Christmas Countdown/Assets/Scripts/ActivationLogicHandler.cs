using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ActivationLogicHandler : MonoBehaviour
{
    [Header("WeaponActivation variables")]
    [Space]
    [Tooltip("The firepoint for this weapon, attached as child of this object")]
    [SerializeField] private Transform weaponFirepoint;

    [Space]
    [Header("AmmunitionActivation variables")]
    [Space]

    [Space]
    [Header("ThrowableActivation variables")]
    [Space]

    [Space]
    [Header("ToolActivation variables")]
    [Space]

    [Space]
    [Header("ApparelActivation variables")]
    [Space]


    // Reference to the equipment behaviour attached on the EquipmentObject
    private EquipmentBehaviour equipmentBehaviour;
  
    // Start is called before the first frame update
    void Start()
    {
        // Get the equipment behaviour from this object
        equipmentBehaviour = GetComponentInParent<EquipmentBehaviour>();

        // Call method to add the correct activation logic
        AddActivationLogic(equipmentBehaviour);

        //Destroy this object after adding the activation logic since we have no use of it no more
        Destroy(gameObject);
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
        weaponFirepoint.SetParent(equipmentBehaviour.transform, true); // Keep world position so changes in edit mode are kept, firepoint stays at the same position ;)
        
        // Add a WeaponActivation to the equipmentBehaviour's gameobject
        WeaponActivation activationScript = equipmentBehaviour.gameObject.AddComponent<WeaponActivation>();

        // Set the equipmentBehaviour's reference to EquipmentActivation to the specific type of activation
        equipmentBehaviour.activationLogic = activationScript;
        
        // Call Initialize method from the specific activation script, passing in the firepoint as firepoint
        activationScript.Initialize(weaponFirepoint);
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
        // Add the specif script to the behaviour's gameobject
        ToolActivation activation = equipmentBehaviour.AddComponent<ToolActivation>();

        // Set the equipmentBehaviour's reference to activation interface to the specific activation script
       // equipmentBehaviour.activationLogic = activation;

        // Add a light component to the behaviour object
        // Call initialize method from specific script, sending the new light as reference
        activation.Initialize(equipmentBehaviour);
    }

    /// <summary>
    /// Method for Adding and initializing a ApparelActivation.cs script to the gameobject
    /// </summary>
    private void InitializeApparelActivation()
    {
        // Add the specif script to the behaviour's gameobject
        ApparelActivation activation = equipmentBehaviour.AddComponent<ApparelActivation>();

        // Set the equipmentBehaviour's reference to activation interface to the specific activation script
        equipmentBehaviour.activationLogic = activation;

        // Call initialize method from specific script
        activation.InitializeActivation();
    }
}
