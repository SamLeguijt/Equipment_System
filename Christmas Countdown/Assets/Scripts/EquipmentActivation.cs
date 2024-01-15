using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentActivation : MonoBehaviour
{
    public GameObject bullet;

    public void Inititialize(GameObject _bullet)
    {
        bullet = _bullet;
    }
    /*
        public void Activate(GameObject _bullet)
        {
            Debug.Log("In EqipmentActivation class call Activate");

            GameObject bull = Instantiate(_bullet, transform.position, Quaternion.identity, transform);

            Rigidbody bulletRb = bull.GetComponent<Rigidbody>();

            bulletRb.AddForce(transform.forward * 5, ForceMode.Impulse);
        }*/

    public virtual void Activate()
    {
        Debug.Log("Parent call");

    }

    public void Test()
    {
        Debug.Log("test?");
    }

    /* Add ref for base activation script to behaviour
     * In behaviour, checktype of data, then add corresponding child activation
     * activation. activate 
     * 
     * weaponactivate has reference to bullet prefab. v
     * 
     * Behaviour has ref to base Activation, with method to Activate() <- Virtual method for child classes. 
     * This is so the Activate method gets called easily, and can be called by just the behaviour instead of referencing in other scripts to Activator.
     * Then, Child classes for weapons, Flashlight, etc. 
     * They implement the Activation method in their own ways.
     * For weapons, they need a reference to a firepoint. To do this, the script can look in parent for Firepoint object/script. 
     * This way, the weaponActivation script can be added automatically in script based on the EquipmentData type, so no mannualy adding to components
     * The WeaponActivation component gets added to the EquipmentBehaviour object.
     * To make this foolproof; the behaviour script should log a warning if the Base EquipmentActivation script is not attached to the same object. 
     * 
     * Behaviour class checks for type of equipment, then assigns appropriate Activation class (Type.Weapon, assign weapon., type.Throwable, assign throwable, appral, apparel etc)
     * The behaviour class assigns the reference for the BaseEquipmentActivation class based on what it added, then the Activation method can be called from outside Behaviour class by calling x method on Behaviour class
     * Base Activation has virtual method, so implementation is in the child class itself. 
     * Can also choose to not auto add the activation script, then devs should add the correct one themselves and behaviour class only gets it. 
     */
}
