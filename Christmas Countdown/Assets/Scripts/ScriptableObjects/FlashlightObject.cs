using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "New Flashlight Equipment", menuName = "Equipment/Tool/Flashlight")]
public class FlashlightObject : ToolEquipmentObject
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    [Header("Light values:")]
    [Space]

    [Tooltip("The angle of the light spot")]
    [SerializeField] private float spotAngle;

    [Tooltip("Range of the light spot")]
    [SerializeField] private float lightRange;

    [Tooltip("Intensity value of the spot light")]
    [SerializeField] private float lightIntensity;

    [Tooltip("")]
    [SerializeField] private float indirectMultiplier;

    [Header("Light settings:")]
    [Space]

    [Tooltip("Type of this light")]
    [SerializeField] private UnityEngine.LightType lightType;

    [Tooltip("Mode of this light")]
    [SerializeField] private LightMode lightMode;

    [Tooltip("Shadow settings of the light")]
    [SerializeField] LightShadows lightShadowType;

    [Tooltip("Should this draw a halo?")]
    [SerializeField] private bool drawLightHalo;

    [Tooltip("Render mode of the light")]
    [SerializeField] LightRenderMode lightRenderMode;

    [Tooltip("Culling mask of the light")]
    [SerializeField] LayerMask lightCullingmask;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Angle of the spot (read-only)
    /// </summary>
    public float SpotAngle { get { return spotAngle; } }

    /// <summary>
    /// Range of the light (read_only)
    /// </summary>
    public float LightRange { get { return lightRange; } }

    /// <summary>
    /// Intensity of the light (read_only)
    /// </summary>
    public float LightIntensity { get { return lightIntensity; } }

    /// <summary>
    /// Indirect multiplier of the light (read_only)
    /// </summary>
    public float IndirectMultiplier { get { return indirectMultiplier; } }

    /// <summary>
    /// Type of the light (read_only)
    /// </summary>
    public UnityEngine.LightType LightType { get { return lightType; } }

    /// <summary>
    /// Mode of the light (read_only)
    /// </summary>
    public LightMode Lightmode { get { return lightMode; } }

    /// <summary>
    /// Shadowtype of the light (read_only)
    /// </summary>
    public LightShadows LightShadowType { get { return lightShadowType; } }

    /// <summary>
    /// Should draw a halo? (read_only)
    /// </summary>
    public bool DrawLightHalo {  get { return drawLightHalo; } }

    /// <summary>
    /// Render mode of the light (read_only)
    /// </summary>
    public LightRenderMode LightRenderMode { get { return lightRenderMode; } }

    /// <summary>
    /// Culling mask of the light (read_only)
    /// </summary>
    public LayerMask LightCullingMask {  get { return lightCullingmask; } } 


    /* ------------------------------------------  METHODS ------------------------------------------- */

    /// <summary>
    /// Sets type of tool on enable
    /// </summary>
    protected override void OnEnable()
    {
        ToolType = TypeOfTool.Flashlight;
    }
}
