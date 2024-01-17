using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "New Flashlight Equipment", menuName = "Equipment/Tool/Flashlight")]
public class FlashlightObject : ToolEquipmentObject
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Tooltip("Light prefab to instantiate")]
    [SerializeField] private GameObject lightObjectPrefab;

    [Header("Light values:")]
    [Space]

    [Tooltip("The angle of the light spot")]
    [SerializeField] private float spotAngle;

    [Tooltip("Range of the light spot")]
    [SerializeField] private float lightRange;

    [Tooltip("Intensity value of the spot light")]
    [SerializeField] private float lightIntensity;

    [Tooltip("Color of the light")]
    [SerializeField] private Color lightColor;

    [Header("Light settings:")]
    [Space]

    [Tooltip("Type of this light")]
    [SerializeField] private UnityEngine.LightType lightType;

    [Tooltip("Render mode of the light")]
    [SerializeField] LightRenderMode lightRenderMode;

    [Tooltip("Culling mask of the light")]
    [SerializeField] LayerMask lightCullingmask;

    [SerializeField] private Vector3 lightRotation;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Light object to instantiate 
    /// </summary>
    public GameObject LightObjectPrefab { get { return lightObjectPrefab; } }

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
    /// Color of the light (read-only)
    /// </summary>
    public Color LightColor { get { return lightColor; } }  

    /// <summary>
    /// Type of the light (read_only)
    /// </summary>
    public UnityEngine.LightType LightType { get { return lightType; } }

    /// <summary>
    /// Render mode of the light (read_only)
    /// </summary>
    public LightRenderMode LightRenderMode { get { return lightRenderMode; } }

    /// <summary>
    /// Culling mask of the light (read_only)
    /// </summary>
    public LayerMask LightCullingMask {  get { return lightCullingmask; } }

    /// <summary>
    ///  Rotation of the Light component when attached to parent (read-only)
    /// </summary>
    public Vector3 LightRotation { get { return lightRotation; } }  

    /* ------------------------------------------  METHODS ------------------------------------------- */

    /// <summary>
    /// Sets type of tool on enable
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Tool;
        ToolType = TypeOfTool.Flashlight;
    }
}
