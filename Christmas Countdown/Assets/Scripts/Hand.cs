using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private BaseEquipmentObject currentEquipment;
    

    public BaseEquipmentObject CurrentEquipment
    {
        get { return currentEquipment; }
        set { currentEquipment = value; }
    }
}
