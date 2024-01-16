using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddActivation : MonoBehaviour
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
                AddWeaponActivation();
                break;
            case EquipmentType.Ammunition:
                break;
            case EquipmentType.Throwable:
                break;
            case EquipmentType.Tool:
                break;
            case EquipmentType.Apparel:
                break;
            default:
                break;
        }
    }
    private void AddWeaponActivation()
    {
        WeaponActivation activation = equipmentBehaviour.AddComponent<WeaponActivation>();
        equipmentBehaviour.activation = activation;

        activation.Initialize(weaponFirepoint, bullet);
    }
}
