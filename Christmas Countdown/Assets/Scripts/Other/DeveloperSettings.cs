using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperSettings : MonoBehaviour
{
    // Static instance for global access
    public static DeveloperSettings instance;


    // Private bools used as settings 

    [Tooltip("Do equipment relevant scripts automatically reference their components on start( ... GetComponent<> etc.)")]
    [SerializeField] private bool autoReferenceEquipmentComponents_OnStart;

    [Tooltip("Do equipment relevant scripts automatically add certain components on start (AddComponent<> etc.)")]
    [SerializeField] private bool autoAddEquipmentComponents_OnStart;

    [Tooltip("Are the UI elements of equipment disabled on start?")]
    [SerializeField] private bool disableEquipmentUI_OnStart; 


    /// <summary>
    /// Do equipment relevant scripts automatically reference their components on start( ... GetComponent<> etc.), read-only
    /// </summary>
    public bool AutoReferenceEquipmentComponents_OnStart { get { return autoReferenceEquipmentComponents_OnStart;} }

    /// <summary>
    /// Do equipment relevant scripts automatically add certain components on start (AddComponent<> etc.), read- only
    /// </summary>
    public bool AutoAddEquipmentComponents_OnStart { get { return autoAddEquipmentComponents_OnStart; } }

    /// <summary>
    /// Are the UI elements of equipment disabled on start? read-only
    /// </summary>
    public bool DisableEquipmentUI_OnStart { get { return disableEquipmentUI_OnStart;} }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else // Destroy if another instance is made
        {
            Destroy(gameObject);
        }
    }



}
