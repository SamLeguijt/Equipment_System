using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Component references")]
    [Space]

    [Tooltip("Reference to the equipment system controller")]
    [SerializeField] private EquipmentSystemController equipmentSystemController;

    [Tooltip("The used to detect if the button can be active")]
    [SerializeField] private Collider detectCollider;

    [Tooltip("The collider from the button object itself")]
    [SerializeField] private Collider buttonCollider;

    [Tooltip("The object to disable upon interacting with the button")]
    [SerializeField] private GameObject objectToRemoveOnButtonPress;

    [Tooltip("The button object itself")]
    [SerializeField] private GameObject buttonObject;

    [Tooltip("Reference to the player")]
    [SerializeField] private GameObject player;

    [Space]
    [Header("Custom values")]
    [Space]

    [Tooltip("The target equipment to look for in player hands")]
    [SerializeField] private EquipmentType targetEquipmentType;

    [Tooltip("Range to detect the button object")]
    [SerializeField] private float detectRange;

    [Tooltip("Range to interact with the button object")]
    [SerializeField] private float interactRange;

    [Tooltip("Base color of the button")]
    [SerializeField] private Color baseButtonColor;

    [Tooltip("Color when interacting with button is possible")]
    [SerializeField] private Color interactButtonColor;

    // Keep track if the flashlight is enabled
    private bool isLightEnabled;

    // Reference to the flashlight
    private FlashlightActivation flashlight;

    // Start is called before the first frame update
    void Start()
    {
        buttonObject.SetActive(false);
        buttonObject.GetComponent<Renderer>().material.color = baseButtonColor;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFlashlightStatus();

        // Disble button object and return early if light is not on
        if (!isLightEnabled)
        {
            buttonObject.SetActive(false);
            return;
        }

        
        // If light is on, player is within detect range and the mouse is over either collider, the button may appear
        if (DistanceFromPlayer() < detectRange && (IsMouseOverCollider(detectCollider) || IsMouseOverCollider(buttonCollider)))
        {
            buttonObject.SetActive(true);

            // Then, if the player is within interact range and if the mouse if over the button itself
            if (DistanceFromPlayer() < interactRange && IsMouseOverCollider(buttonCollider))
            {
                // The color gets set to different color to telegraph interact possibility
                buttonObject.GetComponent<Renderer>().material.color = interactButtonColor;

                // Then wait for interact key input to remove the wall
                if (Input.GetKey(SettingsManager.instance.InteractKey))
                {
                    objectToRemoveOnButtonPress.gameObject.SetActive(false);
                }
            }
            else // Player is not within interact range or mouse is not over button, set color to start color
            {
                buttonObject.GetComponent<Renderer>().material.color = baseButtonColor;
            }
        }
        else
        {
            buttonObject.SetActive(false);  
        }
    }

    /// <summary>
    /// Handles the status of the ligt based on the flashlight equipment 
    /// </summary>
    private void HandleFlashlightStatus()
    {
        // Early return if the tool equipment is not in hand
        if (!IsEquipmentOfTypeInHand(targetEquipmentType)) return;
        else
        {
            // Assign the flashlight in hand if not assigned yet (prevent assigning every frame)
            if (flashlight == null)
            {
                flashlight = player.GetComponentInChildren<FlashlightActivation>();
            }

            // Flip bool based on the light components enable status
            if (flashlight != null)
            {
                isLightEnabled = flashlight.LightComponent.enabled;
            }
        }
    }
    /// <summary>
    /// Returns true if the mouse is over the param collider
    /// </summary>
    /// <param name="_colliderToCheck"></param>
    /// <returns></returns>
    private bool IsMouseOverCollider(Collider _colliderToCheck)
    {
        // Shoot a ray from the camera through the mouse 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If the ray hits something, and the collider of the hit is same as our param
        if (Physics.Raycast(ray, out hit) && hit.collider == _colliderToCheck)
        {
            // The mouse is over the collider 
            return true;
        }

        // Ray did not hit our collider
        return false;


    }

    /// <summary>
    /// Returns the distance from this position to player position
    /// </summary>
    /// <returns></returns>
    private float DistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
    /// <summary>
    /// Checks if an equipment of type param is in either hand of the player
    /// </summary>
    /// <param name="_targetType"></param>
    /// <returns></returns>
    private bool IsEquipmentOfTypeInHand(EquipmentType _targetType)
    {
        if (equipmentSystemController.IsEquipmentTypeInHandOf(_targetType, equipmentSystemController.LeftHand)) return true;
        else if (equipmentSystemController.IsEquipmentTypeInHandOf(_targetType, equipmentSystemController.RightHand)) return true;

        else return false;
    }
}

