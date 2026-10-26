using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlNivel : MonoBehaviour
{
    [SerializeField] private int numeroNivel = 1;
    [SerializeField] private string siguienteEscena;

    public void CompletarNivel()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.GuardarNivel(numeroNivel + 1);
        }

        if (!string.IsNullOrEmpty(siguienteEscena))
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
            Debug.Log("Nivel completado. No se configuró la siguiente escena.");
        }
    }
}
