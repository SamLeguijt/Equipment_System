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

    public KeyCode equipKeyLeft;
    public KeyCode equipKeyRight;

    private Dictionary<KeyCode, Hand> keyToHand;

    private List<EquipmentBehaviour> equipmentsInRange;

    public List<EquipmentBehaviour> EquipmentsInRange
    {
        get { return equipmentsInRange; }
        set { equipmentsInRange = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dictionary
        keyToHand = new Dictionary<KeyCode, Hand>
        {
            { equipKeyLeft, leftHand },
            { equipKeyRight, rightHand }
        };

        EquipmentsInRange = new List<EquipmentBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        /* Here: 
         * GetNearbyEquipment list. 
         * foreach item in list: if item.CanEquip
         * HandleEquipInput(leftKey) / rightKey
         */
    }

    private float DistanceFromPlayerToTarget(GameObject _target)
    {
        return (Vector3.Distance(gameObject.transform.position, _target.transform.position));
    }

    private void Equip(EquipmentBehaviour _item, Hand _hand)
    {
        if (!_item.GetComponent<EquipmentBehaviour>()) { return; }
        else
        {
            _item.transform.position = _hand.transform.position;
            _item.gameObject.transform.SetParent(_hand.transform, true);
            _item.IsEquipped = true;

            _hand.CurrentEquipment = _item.EquipmentData;
        }
    }

    private void Drop(GameObject _item)
    {
        _item.transform.parent = null;
        _item.transform.position = gameObject.transform.position;
    }


    private void HandleEquipInput(KeyCode _inputKey, EquipmentBehaviour equipment)
    {
        if (Input.GetKeyDown(_inputKey))
        {
            if (keyToHand.ContainsKey(_inputKey))
            {
                Hand targetHand = keyToHand[_inputKey];
                Equip(equipment, targetHand);
            }
        }
    }
}

/* Item object: Checks distance to player (equipsystem)
 * IF the distance is x, the item becomes available. 
 * It calls a method on the equipmentSystem, add to a list of EquipmentInRange
 * in this class: update: foreach item in equipment in range, if mouse hovers over it -> if EquipInput -> Equipitem()
 */