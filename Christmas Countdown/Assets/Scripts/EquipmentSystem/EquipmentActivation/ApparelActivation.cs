using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparelActivation : MonoBehaviour, IEquipmentActivation
{

    public void InitializeActivation()
    {

    }
    public void Activate()
    {
        Debug.Log("Call to ApparalActivation child Activation");
    }

}
