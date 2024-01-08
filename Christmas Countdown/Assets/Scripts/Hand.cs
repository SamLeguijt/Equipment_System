using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class for Hand object. <br/>
/// Has a reference to an Equipment object to see if hand is currently equipped. 
/// </summary>
public class Hand : MonoBehaviour
{
    private EquipmentBehaviour currentEquipment;

    public EquipmentBehaviour CurrentEquipment
    {
        get { return currentEquipment; }
        set { currentEquipment = value; }
    }
}
