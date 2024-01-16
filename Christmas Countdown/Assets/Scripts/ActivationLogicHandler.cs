using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivationLogicHandler : MonoBehaviour
{
    [Header("WeaponActivation variables")]
    [Space]
    [SerializeField] private Transform weaponFirepoint;
    [Space]

    [Header("AmmunitionActivation variables")]
    [Header("ThrowableActivation variables")]
    [Header("ToolActivation variables")]
    [Header("ApparelActivation variables")]



    private EquipmentBehaviour equipmentBehaviour;
  
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
        
        activation.Initialize(weaponFirepoint);
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
