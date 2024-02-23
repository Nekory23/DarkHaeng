using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private Vector3 respawnPosition;

    private void Start()
    {
        respawnPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Checkpoint")) {
            respawnPosition = transform.position;
        }
    }
}
