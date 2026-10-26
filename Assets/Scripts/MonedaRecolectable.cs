using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MonedaRecolectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.AgregarMoneda();
        }

        Destroy(gameObject);
    }
}
