using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentBehaviour : MonoBehaviour
{

    private BaseEquipmentObject equipmentData;
    public EquipmentSystemController equipmentSystemController;

    private bool isEquipped;
    private bool isMouseOver;

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

    public bool IsMouseOverEquipment
    {
        get { return isMouseOver; }
        private set { isMouseOver = value; }
    }

    private Transform Player
    {
        // Get player if not null, else throw null reference exception with message
        get { return player ?? throw new System.NullReferenceException("Player tag is not assigned!"); }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Add to equipSystems list of equipments
        equipmentSystemController.EquipmentsInScene.Add(this);
    }

    private void Update()
    {
        bool isListed = equipmentSystemController.EquipmentsInRange.Contains(this);


        if (isListed)
        {
            if (!IsWithinEquipRange())
            {
                equipmentSystemController.EquipmentsInRange.Remove(this);
            }

            return;
        }
        else // Not listed yet
        {
            if (IsWithinEquipRange())
            {
                equipmentSystemController.EquipmentsInRange.Add(this);
            }

            return;
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Enter");
        IsMouseOverEquipment = true;

    }

    private void OnMouseExit()
    {
        Debug.Log("Exit");
        IsMouseOverEquipment = false;
    }

    public bool IsWithinEquipRange()
    {
        return (DistanceFromPlayer() < equipmentSystemController.equipDistance);
    }
    private float DistanceFromPlayer()
    {
        return Vector3.Distance(Player.position, transform.position);
    }
}
