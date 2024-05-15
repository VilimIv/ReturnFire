using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimationControlelrt : NetworkBehaviour
{
    void OnAutoReload()
    {
        FPSCharacterManager.Reload();
    }

    void RifleShoot()
    {
        FPSCharacterManager.CanFire = true;
    }

    void Onthrow()
    {
        FPSCharacterManager.CanFire = true;
        FPSCharacterManager.CanSwitch = true;
        FPSCharacterManager.S_Refrences.CharcaterAniamtor.SetBool("Shot_b", false);
        FPSCharacterManager.GrabWeapon(FPSCharacterManager.CurruntWeapon);
    }
}