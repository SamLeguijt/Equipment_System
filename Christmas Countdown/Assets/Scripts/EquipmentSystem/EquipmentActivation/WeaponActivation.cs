using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : EquipmentActivation
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
        Debug.Log("Call to WeaponActivate child Activation");
    }
}
