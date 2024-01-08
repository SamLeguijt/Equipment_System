using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private EquipmentBehaviour currentEquipment;

    public EquipmentBehaviour CurrentEquipment
    {
        get { return currentEquipment; }
        set { currentEquipment = value; }
    }
/*
    private BaseEquipmentObject currentEquipmentData;

    public BaseEquipmentObject CurrentEquipmentData
    {
        get { return CurrentEquipment.EquipmentData; }
        set { currentEquipmentData = value; }
    }*/
}
