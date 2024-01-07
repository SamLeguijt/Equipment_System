using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystemController : MonoBehaviour
{

    public GameObject target;
    public Vector3 inHandPositionLeft;
    public Vector3 inHandPositionRight;

    public Hand leftHand;
    public Hand rightHand;

    public float equipDistance;

    private EquipmentBehaviour currentEquipmentLeft;
    private EquipmentBehaviour currentEquipmentRight;

    public KeyCode leftEquipKey;
    public KeyCode rightEquipKey;

    private Dictionary<KeyCode, Hand> equipKeyToHandMap;

    private List<EquipmentBehaviour> equipmentsInScene;

    public List<EquipmentBehaviour> EquipmentsInRange;

    public List<EquipmentBehaviour> EquipmentsInScene
    {
        get { return equipmentsInScene; }
        set { equipmentsInScene = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dictionary
        equipKeyToHandMap = new Dictionary<KeyCode, Hand>
        {
            { leftEquipKey, leftHand },
            { rightEquipKey, rightHand }
        };

        EquipmentsInScene = new List<EquipmentBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EquipmentBehaviour equipment in EquipmentsInScene)
        {
            if (equipment.IsEquipped == false)
            {
                // Option 1
                if (!equipment.IsWithinEquipRange() || !equipment.IsMouseOverEquipment)
                {
                    continue;
                }
                else if (equipment.IsWithinEquipRange() && equipment.IsMouseOverEquipment)
                {
                    // Call method to listen to input
                    HandleEquipInput(leftEquipKey, equipment);
                    HandleEquipInput(rightEquipKey, equipment);
                }
            }
            else if (equipment.IsEquipped)
            {
                HandleDropInput(leftEquipKey, equipment);
                HandleDropInput(rightEquipKey, equipment);
            }
        }
    }

    private void Equip(EquipmentBehaviour _item, Hand _hand)
    {
        _item.transform.position = _hand.transform.position;
        _item.gameObject.transform.SetParent(_hand.transform, true);
        _item.IsEquipped = true;

        _hand.CurrentEquipment = _item.EquipmentData;
    }

    private void Drop(EquipmentBehaviour _item, Hand _hand)
    {
        _item.transform.parent = null;
        _item.transform.position = gameObject.transform.position;
        _item.IsEquipped = false;

        _hand.CurrentEquipment = null;
    }

    private void HandleDropInput(KeyCode _inputKey, EquipmentBehaviour _equipment)
    {
        if (Input.GetKeyDown(_inputKey))
        {
            if (equipKeyToHandMap.ContainsKey(_inputKey))
            {
                Hand targetHand = equipKeyToHandMap[_inputKey];
                Drop(_equipment, targetHand);
            }
            else
            {
                throw new KeyNotFoundException($"Input key {_inputKey} not found in keyToHand dictionary.");
            }
        }
    }


    private void HandleEquipInput(KeyCode _inputKey, EquipmentBehaviour _equipment)
    {
        if (Input.GetKeyDown(_inputKey))
        {
            if (equipKeyToHandMap.ContainsKey(_inputKey))
            {
                Hand targetHand = equipKeyToHandMap[_inputKey];
                Equip(_equipment, targetHand);
            }
            else
            {
                throw new KeyNotFoundException($"Input key {_inputKey} not found in keyToHand dictionary.");
            }
        }
    }
}

/* Item object: Checks distance to player (equipsystem)
 * IF the distance is x, the item becomes available. 
 * It calls a method on the equipmentSystem, add to a list of EquipmentInRange
 * in this class: update: foreach item in equipment in range, if mouse hovers over it -> if EquipInput -> Equipitem()
 */