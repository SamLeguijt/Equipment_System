using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyCodeConverter : MonoBehaviour
{
    public TextMeshProUGUI text;

    public KeyCode key;

    private void Update()
    {
       text.SetText(key.ToString());    
    }
}
