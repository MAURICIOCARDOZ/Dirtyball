using UnityEngine;
using UnityEngine.UI; // Necesario para RawImage
using System.Collections.Generic;
using System.Linq; // Necesario para usar LINQ (.FirstOrDefault)

/// <summary>
/// Estructura para mapear el ID de un arma a su textura de icono.
/// [System.Serializable] permite ver y editar esta estructura en el Inspector de Unity.
/// </summary>
[System.Serializable]
public struct ArmaIcono
{
    public int ArmaID;
    public Texture Icono; // Usamos Texture para RawImage
}

public class UI_IconoArma : MonoBehaviour // ¡CRÍTICO: Hereda de MonoBehaviour!
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el GameObject que tiene el script AtributosPersonaje.")]
    public AtributosPersonaje atributosPersonaje;
    
    // El componente RawImage que vamos a actualizar.
    private RawImage rawImage;

    [Header("Mapeo de Iconos")]
    [Tooltip("Configura la lista de IDs de arma y sus texturas correspondientes.")]
    public List<ArmaIcono> mapeoIconos;

    private int currentWeaponId = -1; // Variable de seguimiento

    void Start()
    {
        // Obtener el componente RawImage
        rawImage = GetComponent<RawImage>();
        
        if (atributosPersonaje == null)
        {
            Debug.LogError("UI_IconoArma: No se ha asignado la referencia a 'AtributosPersonaje'.");
            enabled = false; // Deshabilitar script si falta la referencia principal
            return;
        }

        if (rawImage == null)
        {
            Debug.LogError("UI_IconoArma: El GameObject no tiene un componente RawImage.");
            enabled = false;
            return;
        }

        // Realizar la actualización inicial para mostrar el estado actual
        ActualizarIcono();
    }

    void Update()
    {
        // Solo actualizamos la RawImage si el ID del arma del personaje ha cambiado.
        if (atributosPersonaje.Arma != currentWeaponId)
        {
            ActualizarIcono();
        }
    }

    private void ActualizarIcono()
    {
        // 1. Obtener el ID del arma actual
        int newWeaponId = atributosPersonaje.Arma;
        
        // 2. Buscar el primer elemento en la lista 'mapeoIconos' cuyo ArmaID coincida con el newWeaponId
        ArmaIcono match = mapeoIconos.FirstOrDefault(a => a.ArmaID == newWeaponId);

        if (match.Icono != null)
        {
            // 3. Aplicar el icono encontrado
            rawImage.texture = match.Icono;
            rawImage.enabled = true; // Mostrar el icono
        }
        else
        {
            // 4. Si no se encuentra un icono (Ej: ID 0 = Desarmado), ocultar o limpiar la textura
            rawImage.texture = null; 
            rawImage.enabled = false; // Ocultar el RawImage
        }

        // 5. Guardar el nuevo ID para la próxima comprobación
        currentWeaponId = newWeaponId;
    }
}