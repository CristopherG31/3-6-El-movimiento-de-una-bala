using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaNivel : MonoBehaviour
{
    private bool completado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completado || !other.CompareTag("Player"))
            return;

        completado = true;

        ControlNivel control = FindFirstObjectByType<ControlNivel>();

        if (control != null)
        {
            control.CompletarNivel();
        }
        else
        {
            Debug.LogError("No se encontró el objeto ControlNivel.");
        }
    }
}
