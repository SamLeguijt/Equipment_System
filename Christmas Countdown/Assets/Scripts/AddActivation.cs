using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddActivation : MonoBehaviour
{
    public EquipmentActivation activation;

    private EquipmentBehaviour equipmentBehaviour;

    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        /* Prefab object child of EquipmentObject 
 * 
 * Adds correct EquipmentActivation to the object, based on type
 * Initializes the Activation script, passing in firepoint transform (referenced on this object, as a child as well) 
 * Adds the activation to the EquipmentBheaviour's gameobject, then destroys the child object (this after initializing the activation)
 * 
 */

        equipmentBehaviour = GetComponentInParent<EquipmentBehaviour>();
        firePoint.SetParent(equipmentBehaviour.transform, true);
    }

    private void AddWeaponActivation()
    {
        WeaponActivation activation = equipmentBehaviour.AddComponent<WeaponActivation>();
        activation.Inittialize(firePoint);
    }
}
