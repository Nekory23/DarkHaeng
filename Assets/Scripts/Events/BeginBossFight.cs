using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events {
    public class BeginBossFight : MonoBehaviour
    {
        WorldEventManager worldEventManager;

        private void Awake() 
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.CompareTag("Player"))
            {
                worldEventManager.ActivateBossFight();
            }
        }
    }
}
