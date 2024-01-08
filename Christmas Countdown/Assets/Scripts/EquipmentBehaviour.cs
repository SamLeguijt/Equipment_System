using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentBehaviour : MonoBehaviour
{

    private BaseEquipmentObject equipmentData;
    public EquipmentSystemController equipmentSystemController;

    private bool isEquipped;
    private Transform player;

    private bool canDrop;

    public bool CanDrop
    {
        get { return canDrop; } 
        private set { canDrop = value; }
    }

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

    private Transform Player
    {
        // Get player if not null, else throw null reference exception with message
        get { return player ?? throw new System.NullReferenceException("Player tag is not assigned!"); }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

    }    

    private void OnMouseOver()
    {
        if (!IsEquipped)
        {
            equipmentSystemController.OnCursorOver(this);
        }
    }

    public void OnEquip()
    {
        if (!IsEquipped) StartCoroutine(SetDropStatus());

        IsEquipped = true;
    }

    public void OnDrop()
    {
        IsEquipped = false;
        CanDrop = false;    
    }

    private IEnumerator SetDropStatus()
    {
        yield return new WaitForEndOfFrame();
        CanDrop = true;
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
