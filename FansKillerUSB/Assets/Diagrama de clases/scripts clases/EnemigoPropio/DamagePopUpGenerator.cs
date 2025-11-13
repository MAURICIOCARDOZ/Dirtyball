using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator current;
    public GameObject prefab;

    private void Awake()
    {
        current = this;
    }
    
    public void CreatePopUp(Vector3 position, string text)
    {
        Debug.Log("CREANDO POPUP DE DAÑO EN: " + position + " CON TEXTO: " + text);
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        Destroy(popup, 0.5f);
    }
}
