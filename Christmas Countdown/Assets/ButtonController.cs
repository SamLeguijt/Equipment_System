using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Collider buttonCollider;
    public GameObject wallToRemove;
    public GameObject buttonObject;

    public EquipmentSystemController equipmentSystemController;
    public float detectRange;

    public EquipmentType targetEquipmentType;

    // Start is called before the first frame update
    void Start()
    {
        buttonObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceFromPlayerCamera() < detectRange && IsEquipmentOfTypeInHand(targetEquipmentType))
        {
            buttonObject.SetActive(true);
        }
        else
        {
            buttonObject.SetActive(false);
        }


        if (IsMouseOverCollider(buttonCollider))
        {
            if (Input.GetKey(SettingsManager.instance.InteractKey))
            {
                wallToRemove.SetActive(false);
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

    private float DistanceFromPlayerCamera()
    {
        return Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    
    private bool IsEquipmentOfTypeInHand(EquipmentType _targetType)
    {
        if (equipmentSystemController.IsEquipmentTypeInHandOf(_targetType, equipmentSystemController.LeftHand)) return true;
        else if (equipmentSystemController.IsEquipmentTypeInHandOf(_targetType, equipmentSystemController.RightHand)) return true;
        else return false;
    }
        
}

