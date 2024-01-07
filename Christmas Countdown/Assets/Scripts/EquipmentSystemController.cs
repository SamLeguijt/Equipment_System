using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EquipmentSystemController : MonoBehaviour
{

    public GameObject target;
    public Vector3 inHandPositionLeft;
    public Vector3 inHandPositionRight;

    public Transform leftHand;
    public Transform rightHand;

    public float equipDistance;

    private EquipmentBehaviour currentEquipmentLeft;
    private EquipmentBehaviour currentEquipmentRight;

    public KeyCode equipKeyLeft;
    public KeyCode equipKeyRight;

    private Dictionary<KeyCode, Transform> keyToHand;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dictionary
        keyToHand = new Dictionary<KeyCode, Transform>
        {
            { equipKeyLeft, leftHand },
            { equipKeyRight, rightHand }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DistanceFromPlayerToTarget(target) < equipDistance)
            {
                // TODO: FIX DROPPING ITEM FROM EQUIPPED HAND 
                if (currentEquipmentLeft == null)
                {
                    Equip(target, leftHand);
                }
                else if (currentEquipmentRight == null )
                {
                    Equip(target, rightHand);
                }
                else
                {
                    Drop(target);
                }
            }
        }
    }

    private float DistanceFromPlayerToTarget(GameObject _target)
    {
        return (Vector3.Distance(gameObject.transform.position, _target.transform.position));
    }

    private void Equip(GameObject _item, Transform _hand)
    {
        if (!_item.GetComponent<EquipmentBehaviour>()) { return; }
        else
        {
            _item.transform.position = _hand.transform.position;
            _item.gameObject.transform.SetParent(_hand, true);
        }
    }

    private void Drop(GameObject _item)
    {
        _item.transform.parent = null;
        _item.transform.position = gameObject.transform.position;
    }


    private void HandleEquipInput(KeyCode _inputKey)
    {
        if (keyToHand.ContainsKey(_inputKey))
        {
            Transform targetHand = keyToHand[_inputKey];
            Equip(target, targetHand);
        }
    }
}
