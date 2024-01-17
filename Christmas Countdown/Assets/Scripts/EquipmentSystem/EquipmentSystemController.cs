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
    /// <summary>
    /// Nested class for key bindings for each hand
    /// Nested because we don't need access outside this class to the class
    /// </summary>
    public class HandKeyBindings
    {
        // Reference to the hand
        public Hand hand;

        // Define private keycodes for activation and drop/equip keys
        private KeyCode activationKey;
        private KeyCode dropEquipKey;

        // Public read-only references to both keys (don't want to set them outside constructor)
        public KeyCode ActivationKey { get { return activationKey; } }
        public KeyCode DropEquipKey { get { return dropEquipKey; } }

        // Constructor sets hand with activation and equip key to param values
        public HandKeyBindings(Hand _hand, KeyCode _activationKey, KeyCode _dropEquipKey)
        {
            this.hand = _hand;
            this.activationKey = _activationKey;
            this.dropEquipKey = _dropEquipKey;
        }
    }


    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Space]
    [Space]
    [Header("Hand objects attached to player")]
    [Space]

    [Tooltip("Reference to the script on LeftHand of the player")]
    [SerializeField] private Hand leftHand;

    [Tooltip("Reference to the script on RightHand of the player")]
    [SerializeField] private Hand rightHand;

    [Space]
    [Space]
    [Header("Input keys for equipment system")]
    [Space]

    [Tooltip("Input key to equip/drop equipment to/from the left hand")]
    [SerializeField] private KeyCode leftHandEquipDropKey;

    [Tooltip("Input key to equip/drop equipment to/from the left hand")]
    [SerializeField] private KeyCode rightHandEquipDropKey;

    [Tooltip("Input key to activate equipment in left hand")]
    [SerializeField] private KeyCode leftHandActivationKey;

    [Tooltip("Input key to activate equipment in right hand")]
    [SerializeField] private KeyCode rightHandActivationKey;

    [Space]
    [Space]
    [Header("Other variables")]
    [Space]

    [Tooltip("Delay for equipping the new item when swapping in seconds (Drop and Equip simultaneously")]
    [SerializeField][Range(0, 1)] private float equipDelayWhenSwapping;

    /// <summary>
    /// Dictionary for associating a Hand object with its activtion and equip/drop keys <br/>
    /// Stores both hands and their keybindings in one dict, so full dict.
    /// </summary>
    private Dictionary<Hand, HandKeyBindings> fullHandKeyBindings;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Public read only reference to the left hand 
    /// </summary>
    public Hand LeftHand
    {
        get { return leftHand; }
    }

    /// <summary>
    /// Public read only reference to the right hand
    /// </summary>
    public Hand RightHand
    {
        get { return rightHand; }
    }

    /// <summary>
    /// Public property with private set used for getting and setting the value of the key to equip/drop items in left hand 
    /// </summary>
    public KeyCode LeftHandEquipDropKey
    {
        get { return leftHandEquipDropKey; }
        private set { leftHandEquipDropKey = value; }
    }

    /// <summary>
    /// Public property with private set used for getting and setting the value of the key to equip/drop items in right hand
    /// </summary>
    public KeyCode RightHandEquipDropKey
    {
        get { return rightHandEquipDropKey; }
        private set { rightHandEquipDropKey = value; }
    }

    /// <summary>
    /// Public get, private set property for getting the key to activate equipment in left hand
    /// </summary>
    public KeyCode LeftHandActivationKey
    {
        get { return LeftHandActivationKey; }
        private set { leftHandActivationKey = value; }
    }

    /// <summary>
    /// Public get, private set property for getting the key to activate equipment in right hand
    /// </summary>
    public KeyCode RightHandActivationKey
    {
        get { return RightHandActivationKey; }
        private set { RightHandActivationKey = value; }
    }

    /// <summary>
    /// Read only property to get the key bindings for all hands
    /// </summary>
    public Dictionary<Hand, HandKeyBindings> FullHandKeyBindings
    {
        get { return fullHandKeyBindings; }
    }
    /* ------------------------------------------  METHODS ------------------------------------------- */


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dictionary with its keys and values
        fullHandKeyBindings = new Dictionary<Hand, HandKeyBindings>();

        // Check if any necessary components are missing on start
        if (leftHand == null || rightHand == null || fullHandKeyBindings == null)
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
    /// Adds hand with its key bindings to full dict
    /// </summary>
    private void InitializeEquipmentSystemController()
    {
        fullHandKeyBindings.Add(leftHand, new HandKeyBindings(leftHand, leftHandActivationKey, LeftHandEquipDropKey));
        fullHandKeyBindings.Add(rightHand, new HandKeyBindings(rightHand, rightHandActivationKey, RightHandEquipDropKey));
    }

    // Update is called once per frame
    void Update()
    {
        // Handle the current equipment for both hands in update
        HandleCurrentEquipment(leftHand);
        HandleCurrentEquipment(rightHand);

    }

    public bool CheckEmptyHandActivationInput(Hand _hand)
    {
        // Returns if an empty hand presses the activation input
        if (_hand.CurrentEquipment == null)
        {
            KeyCode targetKey = FullHandKeyBindings[_hand].ActivationKey;

            if (Input.GetKeyDown(targetKey))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Private void method that handles the current equipment of the parameter hand <br/>
    /// Drops item on receiving input
    /// </summary>
    /// <param name="_hand"></param>
    private void HandleCurrentEquipment(Hand _hand)
    {
        // First check if the hand currently has an equipment equipped
        if (_hand.CurrentEquipment == null) return;
        else 
        {
            // Call method to drop the equipment when input is given
            CheckForDropInput(_hand, _hand.CurrentEquipment);

            // Call method to activate equipment on input
            CheckForActivationInput(_hand, _hand.CurrentEquipment);
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
    public void Equip(EquipmentBehaviour _equipment, Hand _hand)
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
    public void Drop(EquipmentBehaviour _equipment, Hand _hand, bool _applyDropForces = true)
    {
        // Call method from EquipmentBehaviour class to handle its own drop code
        _equipment.OnDrop(_applyDropForces);

        // Remove the hand's current equipment 
        _hand.CurrentEquipment = null;
    }

    /// <summary>
    /// Method that waits for input to activate an equipment item <br/>
    /// Gets the associated input key from the hand and waits for its input to activate the equipment 
    /// </summary>
    /// <param name="_hand"> Hand to drop from </param>
    /// <param name="_equipment"> Equipment to drop from the hand </param>
    private void CheckForActivationInput(Hand _hand, EquipmentBehaviour _equipment)
    {
        // First check if the equipment can be dropped and if the hand is present in the dictionary 
        if (fullHandKeyBindings.ContainsKey(_hand))
        {
            // Get the wanted input key by looking in the dict
            KeyCode targetKey = fullHandKeyBindings[_hand].ActivationKey;

            // Then check for its input
            if (Input.GetKeyDown(targetKey))
            {
                _equipment.activationLogic.Activate();
            }
        }
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
        if (_equipment.CanDrop && fullHandKeyBindings.ContainsKey(_hand))
        {
            // Get the wanted input key by looking in the dict
            KeyCode targetKey = fullHandKeyBindings[_hand].DropEquipKey;

            // Then check for its input
            if (Input.GetKeyDown(targetKey))
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
    public void CheckForEquipInput(Hand _hand, EquipmentBehaviour _equipment)
    {
        // First check if the hand param is in the dictionary 
        if (fullHandKeyBindings.ContainsKey(_hand))
        {
            // Get the key to check for input
            KeyCode targetKey = fullHandKeyBindings[_hand].DropEquipKey;

            // Wait for its input
            if (Input.GetKeyDown(targetKey))
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

