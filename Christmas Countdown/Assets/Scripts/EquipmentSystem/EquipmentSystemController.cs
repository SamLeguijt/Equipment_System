using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Public class for the system of equipping objects, dropping and swapping them. <br/> 
/// References the player's hands to equip types of EquipmentBehaviours to them when conditions are met
/// </summary>
public class EquipmentSystemController : MonoBehaviour
{
    /* ------------------------------------------  VARIABLES ------------------------------------------- */
    
    [Space] [Space]
    [Header("Hand objects attached to player")]
    [Space]

    [Tooltip("Reference to the script on LeftHand of the player")]
    [SerializeField] private Hand leftHand;

    [Tooltip("Reference to the script on RightHand of the player")]
    [SerializeField] private Hand rightHand;

    [Space] [Space]
    [Header("Keys for equipping/dropping equipment")]
    [Space]

    [Tooltip("Input key to equip/drop equipment to/from the left hand")]
    [SerializeField] private KeyCode leftHandInputKey;

    [Tooltip("Input key to equip/drop equipment to/from the left hand")]
    [SerializeField] private KeyCode rightHandInputKey;

    [Space] [Space]
    [Header("Other variables")]
    [Space]

    [Tooltip("Delay for equipping the new item when swapping in seconds (Drop and Equip simultaneously")]
    [SerializeField] [Range(0, 1)] private float equipDelayWhenSwapping; 

    /// <summary>
    /// Ditionary for associating a Hand object with an input key, vice versa <br/>
    /// Used for equipping objects to hand with its own input
    /// </summary>
    private Dictionary<Hand, KeyCode> inputToHandKeyBindings;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /// <summary>
    /// Public property with private set used for getting and setting the value of the key to equip/drop items in left hand <br/> <br/>
    /// </summary>
    public KeyCode LeftHandInputKey
    {
        get { return leftHandInputKey; }
        private set { leftHandInputKey = value; }
    }

    /// <summary>
    /// Public property with private set used for getting and setting the value of the key to equip/drop items in right hand <br/> <br/>
    /// </summary>
    public KeyCode RightHandInputKey
    {
        get { return rightHandInputKey; }
        private set { rightHandInputKey = value; }
    }

    public Hand LeftHand
    {
        get { return leftHand; }
    }

    public Hand RightHand
    {
        get { return rightHand; }
    }
    /* ------------------------------------------  METHODS ------------------------------------------- */


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dictionary with its keys and values
        inputToHandKeyBindings = new Dictionary<Hand, KeyCode>();

        // Check if any necessary components are missing on start
        if (leftHand == null || rightHand == null || inputToHandKeyBindings == null)
        {
            // Disable object and throw message
            gameObject.SetActive(false);
            throw new System.Exception($"One or more components and references are missing from {gameObject.name}! Please assign components, then re-run. Disabling {this.name} for now.");
        }
        else // Everything is good, let's initialize
        {
            InitializeEquipmentSystemController();
        }
    }

    /// <summary>
    /// Method for initializing this instance <br/>
    /// Adds key-value pairs to dictionary
    /// </summary>
    private void InitializeEquipmentSystemController()
    {
        inputToHandKeyBindings.Add(leftHand, leftHandInputKey);
        inputToHandKeyBindings.Add(rightHand, rightHandInputKey);
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

            if (Input.GetKeyDown(KeyCode.X))
            {
                _hand.CurrentEquipment.activationLogic.Activate();
            }
        }
    }

    /// <summary>
    /// Public void method that gets called when the mouse cursor is over an equipment object, is within range, not equipped and on the ground<br/>
    /// Calls method that waits for equip input the object when receiving input
    /// </summary>
    /// <param name="_equipment"></param>
    public void TryEquip(EquipmentBehaviour _equipment)
    {
        // Call method to check for input for both hands
        CheckForEquipInput(leftHand, _equipment);
        CheckForEquipInput(rightHand, _equipment);
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
                // Check if the hand is not equipped 
                if (_hand.CurrentEquipment == null)
                {
                    // Equip to the associated hand on input if not equipped
                    Equip(_equipment, _hand);
                }
                else // Hand is already equipped
                {
                    // Use coroutine for small delay between swapping
                    StartCoroutine(SwapEquipment(_hand, _equipment));
                }
            }
        }
    } 

    /// <summary>
    /// Coroutine used for dropping current equipment from hand, then equipping new equipment 
    /// </summary>
    /// <param name="_hand"></param>
    /// <param name="_newEquipment"></param>
    /// <returns></returns>
    private IEnumerator SwapEquipment(Hand _hand, EquipmentBehaviour _newEquipment)
    {
        Drop(_hand.CurrentEquipment, _hand);

        yield return new WaitForSeconds(equipDelayWhenSwapping);

        Equip(_newEquipment, _hand);
    }
}
