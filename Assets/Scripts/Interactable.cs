using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 0.6f;
    public string interactableText;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void Interact(Player.PlayerManager playerManager)
    {
    }
}
