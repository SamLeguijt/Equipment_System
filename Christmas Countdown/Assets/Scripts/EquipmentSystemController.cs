using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class for the system of equipping objects, dropping and swapping them. <br/> 
/// References the player's hands to equip types of EquipmentBehaviours to them when conditions are met
/// </summary>
public class EquipmentSystemController : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;

    public float equipDistance;

    public KeyCode leftEquipKey;
    public KeyCode rightEquipKey;

    private Dictionary<Hand, KeyCode> HandKey;

    // Start is called before the first frame update
    void Start()
    {
        HandKey = new Dictionary<Hand, KeyCode>
        {
            { leftHand, leftEquipKey },
            { rightHand, rightEquipKey }
        };
    }

    // Update is called once per frame
    void Update()
    {
        CurrentEquipmentHand(leftHand);
        CurrentEquipmentHand(rightHand);
    }

    private void CurrentEquipmentHand(Hand _hand)
    {
        if (_hand.CurrentEquipment != null)
        {
            DropFromHandOnInputs(_hand, _hand.CurrentEquipment);
        }
    }

    public void OnCursorOver(EquipmentBehaviour _equipment)
    {
        if (_equipment.IsWithinEquipRange())
        {
            HandleEquipInput(_equipment);
        }
    }
    private void Equip(EquipmentBehaviour _item, Hand _hand)
    {
        _item.transform.position = _hand.transform.position;
        _item.gameObject.transform.SetParent(_hand.transform, true);
        _item.OnEquip();

        _hand.CurrentEquipment = _item;
    }

    private void Drop(EquipmentBehaviour _item, Hand _hand)
    {
        _item.transform.parent = null;
        _item.transform.position = gameObject.transform.position;
        _item.OnDrop();
        _hand.CurrentEquipment = null;
    }
    
    private void DropFromHandOnInputs(Hand _hand, EquipmentBehaviour _equipment)
    {
        if (HandKey.ContainsKey(_hand))
        {
            KeyCode inputKey = HandKey[_hand];

            if (Input.GetKeyDown(inputKey) && _equipment.CanDrop)
            {
                Drop(_equipment, _hand);
            }
        }
    }
    
    private void HandleEquipInput(EquipmentBehaviour _equipment)
    {
        EquipToHandOnInput(leftHand, _equipment);
        EquipToHandOnInput(rightHand, _equipment);
    }


    private void EquipToHandOnInput(Hand _hand, EquipmentBehaviour _equipment)
    {

        if (HandKey.ContainsKey(_hand))
        {
            KeyCode inputKey = HandKey[_hand];

            if (Input.GetKeyDown(inputKey))
            {
                Equip(_equipment, _hand);
            }
        }
    } 
}
