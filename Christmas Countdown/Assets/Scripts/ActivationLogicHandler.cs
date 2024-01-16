using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivationLogicHandler : MonoBehaviour
{
    /* Prefab object child of EquipmentObject 
* 
* Adds correct EquipmentActivation to the object, based on type
* Initializes the Activation script, passing in firepoint transform (referenced on this object, as a child as well) 
* Adds the activation to the EquipmentBheaviour's gameobject, then destroys the child object (this after initializing the activation)
* 
*/

    private EquipmentBehaviour equipmentBehaviour;
    public Transform weaponFirepoint;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        equipmentBehaviour = GetComponentInParent<EquipmentBehaviour>();
        weaponFirepoint.SetParent(equipmentBehaviour.transform, true);

        AddActivationLogic(equipmentBehaviour);
        Destroy(gameObject);
    }

    private void AddActivationLogic(EquipmentBehaviour _equipment)
    {
        switch (_equipment.EquipmentData.EquipmentType)
        {
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
                break;
        }
    }

    private void InitializeWeaponActivation()
    {
        WeaponActivation activation = equipmentBehaviour.AddComponent<WeaponActivation>();
        equipmentBehaviour.activationLogic = activation;

        activation.Initialize(weaponFirepoint, bullet);
    }

    private void InitializeAmmunitionActivation()
    {
        AmmunitionActivation activation = equipmentBehaviour.AddComponent<AmmunitionActivation>();
        equipmentBehaviour.activationLogic = activation;

        activation.InitializeActivation();
    }

    private void InitializeThrowableActivation()
    {
        ThrowableActivation activation = equipmentBehaviour.AddComponent<ThrowableActivation>();
        equipmentBehaviour.activationLogic = activation;

        activation.InitializeActivation();
    }

    private void InitializeToolActivation()
    {
        ToolActivation activation = equipmentBehaviour.AddComponent<ToolActivation>();
        equipmentBehaviour.activationLogic = activation;

        activation.InitializeActivation();
    }

    private void InitializeApparelActivation()
    {
        ApparelActivation activation = equipmentBehaviour.AddComponent<ApparelActivation>();
        equipmentBehaviour.activationLogic = activation;

        activation.InitializeActivation();
    }
}
