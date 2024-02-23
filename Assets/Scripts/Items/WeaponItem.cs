using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public enum WeaponType
    {
        None,
        OneHanded,
        TwoHanded
    }
    public GameObject modelPrefab;
    public WeaponType weaponType = WeaponType.None;
    public bool isUnarmed;

    [Header("Idle Animations")]
    public string oneHandedRightIdle_1;
    public string oneHandedLeftIdle_1;
    public string twoHandedIdle_1;

    [Header("One Handed Attack Animations")]
    public string oneHandedLightAttack_1;
    public string oneHandedLightAttack_2;
    public string oneHandedHeavyAttack_1;
    public string oneHandedHeavyAttack_2;

    [Header("Two Handed Attack Animations")]
    public string twoHandedLightAttack_1;
    public string twoHandedLightAttack_2;
    public string twoHandedHeavyAttack_1;
    public string twoHandedHeavyAttack_2;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMutliplier;
    public float heavyAttackMultiplier;
}
