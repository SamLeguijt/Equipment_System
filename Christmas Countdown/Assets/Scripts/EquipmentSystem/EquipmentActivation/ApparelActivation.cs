using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparelActivation : EquipmentActivation
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        Debug.Log("Call to ApparalActivation child Activation");
    }
}
