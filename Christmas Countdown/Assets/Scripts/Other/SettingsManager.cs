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

    public List<EquipmentBehaviour> equipmentsInScene;

    // Private bools used as settings 

    [Tooltip("Do equipment relevant scripts automatically reference their components on start( ... GetComponent<> etc.)")]
    [SerializeField] private bool autoReferenceEquipmentComponents_OnStart;

    [Tooltip("Do equipment relevant scripts automatically add certain components on start (AddComponent<> etc.)")]
    [SerializeField] private bool autoAddEquipmentComponents_OnStart;

    [Tooltip("Are the UI elements of equipment disabled on start?")]
    [SerializeField] private bool disableEquipmentUI_OnStart;

    public GameObject panelSettingsUI;

    public KeyCode toggleSettingsKey;

    public GameObject canvas;

    public Transform player;

    public Hand leftHand;

    public TMP_InputField activationLeft;

    public bool unlimitedAmmo;
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

        //canvas.transform.LookAt(player);

    }

    private void Start()
    {
        ToggleSettingsPanel(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(toggleSettingsKey))
        {
            ToggleSettingsPanel(!panelSettingsUI.activeSelf);
        }   
        
        HandleMouseState();

        Camera camera = Camera.main;

        canvas.transform.LookAt(canvas.transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
    }

    public void OnUnlimitedAmmoToggle()
    {
        unlimitedAmmo = !unlimitedAmmo;
    }

    private void ReloadScene()
    {
        // Get the current active scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the current scene again
        SceneManager.LoadScene(currentSceneName);
    }

    public void OnRestartSceneButtonClick()
    {
        Debug.Log("Call apply");

        ReloadScene();
    }

    private void ToggleSettingsPanel(bool _active)
    {
        panelSettingsUI.SetActive(!panelSettingsUI.activeSelf);
    }

    private void HandleMouseState()
    {
        if (panelSettingsUI.activeSelf)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }

    public bool IsOpenSettingsMenu()
    {
        return panelSettingsUI.activeSelf;
    }
}
