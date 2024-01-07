using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentBehaviour : MonoBehaviour
{

    private BaseEquipmentObject equipmentData;
    public EquipmentSystemController equipmentSystemController;

    private bool isEquipped;
    private bool canEquip;

    private Transform player; 
    public BaseEquipmentObject EquipmentData
    {
        get { return equipmentData; }
        private set { equipmentData = value; }
    }
    public bool IsEquipped
    {
        get { return isEquipped; }
        set { isEquipped = value; }
    }

    public bool CanEquip
    {
        get { return canEquip; }
        private set { canEquip = value; }
    }

    private Transform Player
    {
        // Get player if not null, else throw null reference exception with message
        get { return player ?? throw new System.NullReferenceException("Player tag is not assigned!"); }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; 
    }

    private void OnMouseEnter()
    {
        Debug.Log("Enter");
        
        if (WithinEquipRange())
        {
            CanEquip = true;
        }
    }

    private void OnMouseExit()
    {
        Debug.Log("Exit");
        canEquip = false;
    }

    public bool WithinEquipRange()
    {
        return (DistanceFromPlayer() < equipmentSystemController.equipDistance);
    }
    private float DistanceFromPlayer()
    {
        return Vector3.Distance(Player.position, transform.position);
    }
}
