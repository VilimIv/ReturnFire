using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class FPSCharacterManager : NetworkBehaviour
{
    public List<GunDetails> Weapons;
    public List<ThrowableDetails> Throwables;
    public static List<GunDetails> S_Weapons;
    public OtherRefs Refrences;
    public static OtherRefs S_Refrences;
    public static int CurruntWeapon;
    public static bool CanFire;
    public static bool CanSwitch;

    void Awake()
    {
        CanFire = true;
        CanSwitch = true;
        S_Weapons = Weapons;
        S_Refrences = Refrences;
    }
    private void LateUpdate()
    {
        S_Refrences.WeaponRootPose.LookAt(S_Refrences.TargetPos);
        S_Refrences.WeaponRootPose.localEulerAngles = new Vector3(0, S_Refrences.WeaponRootPose.eulerAngles.x, 0);
        S_Refrences.WeaponRootObjcts.LookAt(S_Refrences.TargetPos);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        FillAmmo();
        GrabWeapon(0);
        CurruntWeapon = 0;
        UpdateAmmo();
        UpdateThrowAblesAmmo();
    }
    void FillAmmo()
    {
        foreach(GunDetails weapon in S_Weapons)
        {
            weapon.CurruntMagAmmo = weapon.MagCapacity;
        }
    }
    void Update()
    {
        SwitchGunChecks();
        ShootChecks();
        ThrowwableInputs();
    }

    #region Throables Input Checks
    void ThrowwableInputs()
    {
        for (int i = 0; i < Throwables.Count; i++)
        {
            if (CanFire)
            {
                if (Input.GetKeyDown(Throwables[i].InputKey))
                {
                    if (Throwables[i].Amount <= 0)
                        return;

                    S_Refrences.CharcaterAniamtor.SetInteger("WeaponType_int", Throwables[i].AnimatorType);
                    S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", true);
                    DIsableAllWeapons();
                    Throwables[i].ThrowableObject.gameObject.SetActive(true);
                    CanSwitch = false;
                    Throwables[i].Amount--;
                    UpdateThrowAblesAmmo();
                    CanFire = false;
                }
            }
        }
    }
    #endregion

    public static void GrabWeapon(int Index)
    {
        DIsableAllWeapons();
        S_Weapons[Index].GunObject.gameObject.SetActive(true);
        S_Refrences.CharcaterAniamtor.SetInteger("WeaponType_int", S_Weapons[Index].AnimatorType);
        S_Refrences.WeaponsAnimator.SetInteger("WeaponType_int", S_Weapons[Index].AnimatorType);
        S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", false);
        S_Refrences.CharcaterAniamtor.SetBool("FullAuto_b", false);
        S_Refrences.WeaponsAnimator.SetBool("Shoot_b", false);
        S_Refrences.WeaponsAnimator.SetBool("FullAuto_b", false);
        S_Refrences.CharcaterAniamtor.SetBool("Reload_b", false);
        S_Refrences.WeaponsAnimator.SetBool("Reload_b", false);
        UpdateAmmo();
        CanFire = true;
       // AdjustCamera();
    }
    void AdjustCamera()
    {
        switch (S_Weapons[CurruntWeapon].AnimatorType)
        {
            case 0:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.IdlePos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.IdlePos.rotation;
                break;
            case 1:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.HandGunPos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.HandGunPos.rotation;
                break;
            case 2:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.AssultPos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.AssultPos.rotation;
                break;
            case 3:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.AssultPos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.AssultPos.rotation;
                break;
            case 4:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.AssultPos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.AssultPos.rotation;
                break;
            case 5:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.SniperPos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.SniperPos.rotation;
                break;
            case 6:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.SniperPos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.SniperPos.rotation;
                break;
            case 7:
                S_Refrences.PlayerCamera.position = S_Refrences.CameraPositions.AssultPos.position;
                S_Refrences.PlayerCamera.rotation = S_Refrences.CameraPositions.AssultPos.rotation;
                break;
        }
    }
    static void DIsableAllWeapons()
    {
        for (int i = 0; i < S_Weapons.Count; i++)
        {
            S_Weapons[i].GunObject.gameObject.SetActive(false);
        }
    }
    #region Inputs
    void SwitchGunChecks()
    {
        if (!CanSwitch)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (CurruntWeapon + 1 < S_Weapons.Count)
            {
                CurruntWeapon++;
            }
            else
            {
                CurruntWeapon = 0;
            }
            GrabWeapon(CurruntWeapon);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (CurruntWeapon - 1 >= 0)
            {
                CurruntWeapon--;
            }
            else
            {
                CurruntWeapon = S_Weapons.Count - 1;
            }
            GrabWeapon(CurruntWeapon);
        }
    }
    float ShootTimer;
    IEnumerator StopSingleAnimas()
    {
        yield return new WaitForEndOfFrame();
        S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", false);
        S_Refrences.CharcaterAniamtor.SetBool("FullAuto_b", false);
        S_Refrences.WeaponsAnimator.SetBool("Shoot_b", false);
        S_Refrences.WeaponsAnimator.SetBool("FullAuto_b", false);
    }
    void ShootChecks()
    {
        if (!CanFire)
            return;

        if (Input.GetMouseButtonDown(0) && S_Weapons[CurruntWeapon].CurruntMagAmmo > 0)
        {
            S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", true);
            S_Refrences.WeaponsAnimator.SetBool("Shoot_b", true);
            if (Weapons[CurruntWeapon].Type == GunType.Auto)
            {
                S_Refrences.CharcaterAniamtor.SetBool("FullAuto_b", true);
                S_Refrences.WeaponsAnimator.SetBool("FullAuto_b", true);
            }
            if(S_Weapons[CurruntWeapon].CurruntMagAmmo > 0)
            {
                Shoot();
                if(Weapons[CurruntWeapon].Type == GunType.Single)
                {
                    StartCoroutine(StopSingleAnimas());
                    switch (S_Weapons[CurruntWeapon].AnimatorType)
                    {
                        case 1:
                            S_Refrences.CharcaterAniamtor.Play("Handgun_Shoot", 0, 0);
                            S_Refrences.WeaponsAnimator.Play("Handgun_Shoot", 0, 0);
                            break;
                        case 2:
                            S_Refrences.CharcaterAniamtor.Play("AK47_Auto_SingleShot", 0, 0);
                            S_Refrences.WeaponsAnimator.Play("AR01_Auto_SingleShot", 0, 0.4f);
                            break;
                        case 4:
                            S_Refrences.CharcaterAniamtor.Play("Shotgun_Shoot", 0, 0);
                            S_Refrences.WeaponsAnimator.Play("Shotgun_Shoot", 0, 0.4f);
                            break;

                    }
                }else if (S_Weapons[CurruntWeapon].Type == GunType.Sniper)
                {
                    CanFire = false;
                }
            }
            else
            {
                StartReload();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (S_Weapons[CurruntWeapon].Type == GunType.Auto)
            {
                if (S_Weapons[CurruntWeapon].CurruntMagAmmo > 0)
                {
                    ShootTimer += Time.deltaTime;
                    if (ShootTimer > S_Weapons[CurruntWeapon].firerate)
                    {
                        Shoot();
                        ShootTimer = 0;
                    }
                }
                else if (S_Weapons[CurruntWeapon].AmmoCapacity > 0)
                {
                    StartReload();
                }
                else
                {
                    S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", false);
                    S_Refrences.CharcaterAniamtor.SetBool("FullAuto_b", false);
                    S_Refrences.WeaponsAnimator.SetBool("Shoot_b", false);
                    S_Refrences.WeaponsAnimator.SetBool("FullAuto_b", false);
                }
            }
        }else if(!Input.GetMouseButtonUp(0))
        {
            S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", false);
            S_Refrences.CharcaterAniamtor.SetBool("FullAuto_b", false);
            S_Refrences.WeaponsAnimator.SetBool("Shoot_b", false);
            S_Refrences.WeaponsAnimator.SetBool("FullAuto_b", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (S_Weapons[CurruntWeapon].AmmoCapacity > 0)
            {
                StartReload();
            }
        }
    }
    #endregion

    #region Shooting System
    void Shoot()
    {
        S_Weapons[CurruntWeapon].CurruntMagAmmo--;
        UpdateAmmo();
    }
    void StartReload()
    {
        S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", false);
        S_Refrences.CharcaterAniamtor.SetBool("FullAuto_b", false);
        S_Refrences.WeaponsAnimator.SetBool("Shoot_b", false);
        S_Refrences.WeaponsAnimator.SetBool("FullAuto_b", false);
        S_Refrences.CharcaterAniamtor.SetBool("Reload_b", true);
        S_Refrences.WeaponsAnimator.SetBool("Reload_b", true);
    }
    public static void Reload()
    {
        int missingAmmo = S_Weapons[CurruntWeapon].MagCapacity - S_Weapons[CurruntWeapon].CurruntMagAmmo;
        if (S_Weapons[CurruntWeapon].AmmoCapacity >= missingAmmo)
        {
            S_Weapons[CurruntWeapon].CurruntMagAmmo = S_Weapons[CurruntWeapon].MagCapacity;
            S_Weapons[CurruntWeapon].AmmoCapacity -= missingAmmo;
        }
        else if (S_Weapons[CurruntWeapon].AmmoCapacity > 0)
        {
            S_Weapons[CurruntWeapon].CurruntMagAmmo += S_Weapons[CurruntWeapon].AmmoCapacity;
            S_Weapons[CurruntWeapon].AmmoCapacity = 0;
        }
        UpdateAmmo();
        S_Refrences.CharcaterAniamtor.SetBool("Reload_b", false);
        S_Refrences.WeaponsAnimator.SetBool("Reload_b", false);

        if (Input.GetMouseButton(0) && S_Weapons[CurruntWeapon].CurruntMagAmmo > 0)
        {
            S_Refrences.CharcaterAniamtor.SetBool("Shoot_b", true);
            S_Refrences.CharcaterAniamtor.SetBool("FullAuto_b", true);
            S_Refrences.WeaponsAnimator.SetBool("Shoot_b", true);
            S_Refrences.WeaponsAnimator.SetBool("FullAuto_b", true);
        }
    }
    #endregion

    #region Ui Update
    static void UpdateAmmo()
    {
        if(S_Refrences.AmmoText != null)
        {
            S_Refrences.AmmoText.text = S_Weapons[CurruntWeapon].CurruntMagAmmo.ToString() + "/" + S_Weapons[CurruntWeapon].AmmoCapacity.ToString();
        }
    }

    void UpdateThrowAblesAmmo()
    {
        for (int i = 0; i < Throwables.Count; i++)
        {
            if (Throwables[i].UiText != null)
            {
                Throwables[i].UiText.text = Throwables[i].Amount.ToString();
            }
        }
    }
    #endregion
}
[System.Serializable]
public class GunDetails
{
    public string WeaponName;
    public Transform GunObject;
    public GunType Type;
    public float firerate;
    public int MagCapacity;
    public int AmmoCapacity;
    public float Aim_OffsetAngle;
    public int AnimatorType;
    [HideInInspector] public int CurruntMagAmmo;
}
public enum GunType
{
    Auto,
    Single,
    Sniper
}

[System.Serializable]
public class OtherRefs
{
    public Transform PlayerCamera;
    public Animator CharcaterAniamtor, WeaponsAnimator;
    public TextMeshProUGUI AmmoText;
    public CameraSetup CameraPositions;
    public Transform WeaponRootPose;
    public Transform TargetPos;
    public Transform WeaponRootObjcts;
}
[System.Serializable]
public class CameraSetup
{
    public Transform AssultPos,
        HandGunPos,
        IdlePos,
        SniperPos;
}

[System.Serializable]
public class ThrowableDetails
{
    public string Name;
    public Transform ThrowableObject;
    public float Amount;
    public int AnimatorType;
    public KeyCode InputKey;
    public TextMeshProUGUI UiText;
}