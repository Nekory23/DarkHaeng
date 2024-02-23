using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock on")]
    public Transform lockOnTransform;

    [Header("Movement flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
}
