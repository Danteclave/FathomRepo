using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public CapsuleCollider capCollider;
    public uint playerHealth;
    public GameObject flashlight;
    public SoundManager sm;
    private bool healing;

    private void Start()
    {
        flashlight.SetActive(false);
        playerHealth = 2;
        cc = GetComponent<CharacterController>();
        capCollider = GetComponent<CapsuleCollider>();

        GameEventSystem.Instance.OnPlayerTakeDamage += PlayerGetDamage;
        GameEventSystem.Instance.OnPlayerRespawn += PlayerRespawn;
        GameEventSystem.Instance.OnPlayerHeal += PlayerHeal;

        healing = false;
    }

    private void PlayerHeal()
    {
        playerHealth++;
        if (playerHealth >= 2)
        {
            GameEventSystem.Instance.PlayerStopHeartbeatEffect();
            sm.Stop(gameObject,"PlayerHitBreath");
        }
    }

    private void PlayerRespawn()
    {
        sm.Stop(gameObject, "PlayerHurtBreath");
        sm.Stop(gameObject, "PlayerHitBreath");
        playerHealth = 2;
        cc.enabled = false;
        cc.gameObject.transform.position = new Vector3(-35f, 5, -18f);
        cc.enabled = true;
        GameEventSystem.Instance.PlayerStopHeartbeatEffect();
    }

    private void PlayerGetDamage()
    {
        playerHealth--;
        sm.Play(gameObject, "PlayerHitBreath");
        if(playerHealth <2)
        {
            GameEventSystem.Instance.PlayerStartHeartbeatEffect();
            if(!sm.IsSoundPlaying(gameObject, "PlayerHurtBreath"))
            {
                StartCoroutine(breathCoroutine());
            }
        }
    }

    private IEnumerator healCoroutine()
    {
        healing = true;
        yield return new WaitForSeconds(5f);
        PlayerHeal();
        healing = false;
    }

    private IEnumerator breathCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        sm.Play(gameObject, "PlayerHurtBreath");
    }


    public CharacterController cc
    {
        get; set;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            GameEventSystem.Instance.FlashlightClick();
            flashlight.SetActive(!flashlight.activeSelf);
        }
        if (playerHealth == 2)
        {
            GameEventSystem.Instance.PlayerStopHeartbeatEffect();
            sm.Stop(gameObject, "PlayerHurtBreath");
            sm.Stop(gameObject, "PlayerHitBreath");
        }
        if (playerHealth == 0)
        {
            GameEventSystem.Instance.PlayerDeath();
        }
        if(playerHealth <2 && !healing)
        {
            StartCoroutine(healCoroutine());
        }
    }
}
