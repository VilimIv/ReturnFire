using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(FPSCharacterManager)), RequireComponent(typeof(Health))]
public class ReSpawnHandler : MonoBehaviour
{
    public bool Respawn = true;
    public float RespwanTime = 5f;
    [Header("Other Refrences")]

    public TextMeshProUGUI CountdownText;

    [HideInInspector] public bool RespawnInProcess;
    FPSCharacterManager characterManager;
    float CountdownTimer;
    // Start is called before the first frame update
    void Start()
    {
        characterManager = GetComponent<FPSCharacterManager>();
        if (CountdownText != null)
            CountdownText.gameObject.SetActive(false);

        CountdownTimer = RespwanTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Respawn)
        {
            if (RespawnInProcess)
            {
                if(CountdownText !=  null)
                    CountdownText.gameObject.SetActive(true);


                CountdownTimer -= Time.deltaTime;

                if (CountdownText != null)
                    CountdownText.text = CountdownTimer.ToString("f0");

                if (CountdownTimer <= 0)
                {
                    ReSpawn();
                    RespawnInProcess = false;

                    if (CountdownText != null)
                        CountdownText.gameObject.SetActive(false);

                    CountdownTimer = RespwanTime;
                }
            }
        }
        else
        {
            if (RespawnInProcess)
            {
                RespawnInProcess = false;
            }
        }
    }

    void ReSpawn()
    {
        int Rand = Random.Range(0, GameManager.S_RespawnPoints.Count);
        transform.position = GameManager.S_RespawnPoints[Rand].position;
        transform.rotation = GameManager.S_RespawnPoints[Rand].rotation;

        GetComponent<Health>().CurrentHealth.Value = 100;
        GetComponent<Health>().isDead = false;

        characterManager.enabled = true;
        characterManager.Refrences.CharcaterAniamtor.SetBool("Death_b", false);
        characterManager.GrabWeapon(0);
        GetComponent<Health>().ShowCharacterServerRPC();

        ResetPlayerPositionServerRPC();
        characterManager.Refrences.CharcaterAniamtor.transform.localPosition = Vector3.zero;

        if (GetComponent<CameraMovement>() != null)
            GetComponent<CameraMovement>().enabled = true;

        if (GetComponent<PlayerController>() != null)
        {
            var playerController = GetComponent<PlayerController>();
            playerController.enabled = true;

            playerController.Velocity = Vector3.zero;

            CharacterController controller = playerController.Controller;
            if (controller != null)
            {
                controller.enabled = false;
                controller.transform.position = transform.position;
                controller.enabled = true;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ResetPlayerPositionServerRPC()
    {
        Debug.Log("POsition Rested Once again :(");
        GetComponent<FPSCharacterManager>().Refrences.CharcaterAniamtor.transform.localPosition = Vector3.zero;
    }
}