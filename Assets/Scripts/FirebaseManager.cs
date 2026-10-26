
using System.Collections.Generic;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private FirebaseFirestore db;
    private bool firebaseListo = false;

    public int monedasTotales = 0;
    public int nivelAlcanzado = 1;

    [Header("Escenas del juego")]
    [SerializeField] private string escenaNivel1 = "nivel1";
    [SerializeField] private string escenaNivel2 = "nivel2";
    [SerializeField] private string escenaNivel3 = "nivel3";

    private string jugadorId = "jugador_prueba";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("FirebaseManager iniciado.");

        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError(
                        "Error al comprobar Firebase: " + task.Exception);
                    return;
                }

                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError(
                        "Firebase no está disponible: " + task.Result);
                    return;
                }

                db = FirebaseFirestore.DefaultInstance;

                Debug.Log("Firebase conectado correctamente.");

                CargarProgreso();
            });
    }

    public void CargarProgreso()
    {
        if (db == null)
        {
            Debug.LogError("Firestore no está inicializado.");
            return;
        }

        db.Collection("jugadores")
          .Document(jugadorId)
          .GetSnapshotAsync()
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsFaulted || task.IsCanceled)
              {
                  Debug.LogError(
                      "Error al cargar progreso: " + task.Exception);
                  return;
              }

              DocumentSnapshot documento = task.Result;

              if (documento.Exists)
              {
                  if (documento.ContainsField("monedas"))
                      monedasTotales =
                          documento.GetValue<int>("monedas");

                  if (documento.ContainsField("nivel"))
                      nivelAlcanzado =
                          documento.GetValue<int>("nivel");
              }

              firebaseListo = true;

              Debug.Log(
                  "Progreso cargado. Monedas: " + monedasTotales +
                  " | Nivel: " + nivelAlcanzado);

              CargarNivelGuardado();
          });
    }

    public void CargarNivelGuardado()
    {
        string escena;

        switch (nivelAlcanzado)
        {
            case 1:
                escena = escenaNivel1;
                break;

            case 2:
                escena = escenaNivel2;
                break;

            case 3:
                escena = escenaNivel3;
                break;

            default:
                escena = escenaNivel1;
                Debug.LogWarning(
                    "Nivel guardado no reconocido. Se cargará el nivel 1.");
                break;
        }

        if (SceneManager.GetActiveScene().name != escena)
        {
            Debug.Log("Cargando escena guardada: " + escena);
            SceneManager.LoadScene(escena);
        }
    }

    public void GuardarProgreso()
    {
        if (!firebaseListo || db == null)
        {
            Debug.LogWarning(
                "Firebase todavía no está listo. No se guardó el progreso.");
            return;
        }

        Dictionary<string, object> datos =
            new Dictionary<string, object>
            {
                { "monedas", monedasTotales },
                { "nivel", nivelAlcanzado }
            };

        db.Collection("jugadores")
          .Document(jugadorId)
          .SetAsync(datos)
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsCompletedSuccessfully)
              {
                  Debug.Log("Progreso guardado en Firestore.");
              }
              else
              {
                  Debug.LogError(
                      "Error al guardar progreso: " + task.Exception);
              }
          });
    }

    public void AgregarMoneda()
    {
        monedasTotales++;
        Debug.Log("Monedas totales: " + monedasTotales);
        GuardarProgreso();
    }

    public void GuardarNivel(int nivel)
    {
        if (nivel > nivelAlcanzado)
        {
            nivelAlcanzado = nivel;
        }

        GuardarProgreso();
    }
}
