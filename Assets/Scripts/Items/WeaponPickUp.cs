using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPickUp : Interactable
{
    public WeaponItem weaponItem;

    public override void Interact(Player.PlayerManager playerManager)
    {
        base.Interact(playerManager);
        PickUpWeapon(playerManager);
    }

    private void PickUpWeapon(Player.PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        Player.PlayerLocomotion playerLocomotion;
        Player.PlayerAnimatorHandler animatorHandler;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<Player.PlayerLocomotion>();
        animatorHandler = playerManager.GetComponentInChildren<Player.PlayerAnimatorHandler>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation("Pick Up Item", true);
        playerInventory.weaponsInventory.Add(weaponItem);
        playerManager.itemInteractabmeGameObject.GetComponentInChildren<TMP_Text>().text = weaponItem.itemName;
        playerManager.itemInteractabmeGameObject.GetComponentsInChildren<RawImage>()[1].texture = weaponItem.itemIcon.texture;
        playerManager.itemInteractabmeGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
