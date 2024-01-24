using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    // Static instance for global access
    public static SettingsManager instance;

    [Tooltip("Reference to the UI Panel GameObject from canvas")]
    [SerializeField] private GameObject panelSettingsUI;

    [Tooltip("Key to toggle the UI panel with")]
    [SerializeField] private KeyCode toggleSettingsPanelKey;

    [Header("Developer settings")]

    [Tooltip("Do equipment relevant scripts automatically reference their components on start( ... GetComponent<> etc.)")]
    [SerializeField] private bool autoReferenceEquipmentComponents_OnStart;

    [Tooltip("Do equipment relevant scripts automatically add certain components on start (AddComponent<> etc.)")]
    [SerializeField] private bool autoAddEquipmentComponents_OnStart;

    [Tooltip("Are the UI elements of equipment disabled on start?")]
    [SerializeField] private bool disableEquipmentUI_OnStart;


    // Keep track of unlimited ammo value
    private bool isUnlimitedAmmo;

    /// <summary>
    /// Do equipment relevant scripts automatically reference their components on start( ... GetComponent<> etc.), read-only
    /// </summary>
    public bool AutoReferenceEquipmentComponents_OnStart { get { return autoReferenceEquipmentComponents_OnStart; } }

    /// <summary>
    /// Do equipment relevant scripts automatically add certain components on start (AddComponent<> etc.), read- only
    /// </summary>
    public bool AutoAddEquipmentComponents_OnStart { get { return autoAddEquipmentComponents_OnStart; } }

    /// <summary>
    /// Are the UI elements of equipment disabled on start? read-only
    /// </summary>
    public bool DisableEquipmentUI_OnStart { get { return disableEquipmentUI_OnStart; } }

    /// <summary>
    /// Setting for allowing unlimited ammo
    /// </summary>
    public bool IsUnlimitedAmmo { get {  return isUnlimitedAmmo; } }

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

    private void Start()
    {
        // Disable the settings panel on start
        ToggleSettingsPanel(false);
    }


    private void Update()
    {
        // Check for input to toggle the settings panel
        if (Input.GetKeyDown(toggleSettingsPanelKey))
        {
            ToggleSettingsPanel(!panelSettingsUI.activeSelf);
        }

        // Handle the mouse state based on the settings panel activeSelf
        HandleMouseState();
    }

    
    /// <summary>
    /// Toggles the bool for unlimited ammo, called via UI Toggle element click
    /// </summary>
    public void OnUnlimitedAmmoToggle()
    {
        isUnlimitedAmmo = !isUnlimitedAmmo;
    }

    /// <summary>
    /// Restarts the current scene upon UI Restart button click
    /// </summary>
    public void OnRestartSceneButtonClick()
    {
        // Get the current active scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the current scene again
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// Toggles the panel by setting object active based on param _value
    /// </summary>
    /// <param name="_active"></param>
    private void ToggleSettingsPanel(bool _value)
    {
        panelSettingsUI.SetActive(_value);
    }

    /// <summary>
    /// Handles the lock and visible state of the cursor, based on active setting panel yes/no
    /// </summary>
    private void HandleMouseState()
    {
        // Panel settings is open, so enable mouse and make visible
        if (panelSettingsUI.activeSelf)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = true;
        }
        else // Panel settings is closed, so lock cursor and dont show
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }

    /// <summary>
    /// Returns true if the settings panel is active
    /// </summary>
    /// <returns></returns>
    public bool IsOpenSettingsMenu()
    {
        return panelSettingsUI.activeSelf;
    }
}
