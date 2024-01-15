using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEquipmentActivate : EquipmentActivation
{

    public override void Activate()
    {
        Debug.Log("Call from child!");
    }
}
