using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class for the system of equipping objects, dropping and swapping them. <br/> 
/// References the player's hands to equip types of EquipmentBehaviours to them when conditions are met
/// </summary>
public class EquipmentSystemController : MonoBehaviour
{
    [Header("Hand objects attached to player")]
    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;

    [Header("Keys for equipping/dropping equipment")]
    [SerializeField] private KeyCode leftEquipKey;
    [SerializeField] private KeyCode rightEquipKey;

    /// <summary>
    /// Ditionary for associating a Hand object with an input key, vice versa <br/>
    /// Used for equipping objects to hand with its own input
    /// </summary>
    private Dictionary<Hand, KeyCode> inputToHandKeyBindings;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dictionary with its keys and values
        inputToHandKeyBindings = new Dictionary<Hand, KeyCode>
        {
            { leftHand, leftEquipKey },
            { rightHand, rightEquipKey }
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Handle the current equipment for both hands in update
        HandleCurrentEquipment(leftHand);
        HandleCurrentEquipment(rightHand);
    }

    /// <summary>
    /// Private void method that handles the current equipment of the parameter hand <br/>
    /// Drops item on receiving input
    /// </summary>
    /// <param name="_hand"></param>
    private void HandleCurrentEquipment(Hand _hand)
    {
        // First check if the hand currently has an equipment equipped
        if (_hand.CurrentEquipment != null)
        {
            // Call method to drop the equipment when input is given
            CheckForDropInput(_hand, _hand.CurrentEquipment);
        }
    }

    /// <summary>
    /// Public void method that gets called when the mouse cursor is over an equipment object <br/>
    /// Gets called from the EquipmentBehaviour class in 'OnMouseOver' function <br/>
    /// Calls method that equips the object when receiving input
    /// </summary>
    /// <param name="_equipment"></param>
    public void OnCursorOnEquipment(EquipmentBehaviour _equipment)
    {
        // First check if the equipment object is within it's equip range
        if (_equipment.IsWithinEquipRange())
        {
            // Call method to handle the inpiut for equipping the object
            HandleEquipInput(_equipment);
        }
    }

    /// <summary>
    /// Private void method that equips an equipment to a hand
    /// </summary>
    /// <param name="_equipment">  Equipment target </param>
    /// <param name="_hand"> Hand to put target in </param>
    private void Equip(EquipmentBehaviour _equipment, Hand _hand)
    {
        // Call method from EquipmentBehaviour class to set additional info
        _equipment.OnEquip(_hand);

        // Set the CurrentEquipment of the hand 
        _hand.CurrentEquipment = _equipment;
    }

    /// <summary>
    /// Private void method that drops an equipment from a hand
    /// </summary>
    /// <param name="_equipment"> Equipment to drop </param>
    /// <param name="_hand"> Hand to drop the equipment from </param>
    private void Drop(EquipmentBehaviour _equipment, Hand _hand)
    {
        // Call method from EquipmentBehaviour class to handle its own drop code
        _equipment.OnDrop(_hand);

        // Remove the hand's current equipment 
        _hand.CurrentEquipment = null;
    }
    
    /// <summary>
    /// Method that waits for input to drop an equipment item <br/>
    /// Gets the associated input key from the hand and waits for its input to drop the equipment 
    /// </summary>
    /// <param name="_hand"> Hand to drop from </param>
    /// <param name="_equipment"> Equipment to drop from the hand </param>
    private void CheckForDropInput(Hand _hand, EquipmentBehaviour _equipment)
    {
        // First check if the equipment can be dropped and if the hand is present in the dictionary 
        if (_equipment.CanDrop && inputToHandKeyBindings.ContainsKey(_hand))
        {
            // Get the wanted input key by looking in the dict
            KeyCode inputKey = inputToHandKeyBindings[_hand];

            // Then check for its input
            if (Input.GetKeyDown(inputKey))
            {
                // Drop the equipment on input 
                Drop(_equipment, _hand);
            }
        }
    }
    
    /// <summary>
    /// Method that handles the input for equipping items <br/>
    /// Waits for input for both hands with the _equipment to equip to either hand
    /// </summary>
    /// <param name="_equipment"> Equipment to be equipped </param>
    private void HandleEquipInput(EquipmentBehaviour _equipment)
    {
        // Call method to check for input for both hands
        CheckForEquipInput(leftHand, _equipment);
        CheckForEquipInput(rightHand, _equipment);
    }


    /// <summary>
    /// Private void method that waits for input to equip an item to a hand <br/>
    /// Looks in dict to find the binded key to the hand to equip.
    /// </summary>
    /// <param name="_hand"> Hand to check it's input </param>
    /// <param name="_equipment"> Equipment to be equipped in the hand</param>
    private void CheckForEquipInput(Hand _hand, EquipmentBehaviour _equipment)
    {
        // First check if the hand param is in the dictionary 
        if (inputToHandKeyBindings.ContainsKey(_hand))
        {
            // Get the key to check for input
            KeyCode inputKey = inputToHandKeyBindings[_hand];

            // Wait for its input
            if (Input.GetKeyDown(inputKey))
            {
                // Equip to the associated hand on input
                Equip(_equipment, _hand);
            }
        }
    } 
}
