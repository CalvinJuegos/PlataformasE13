using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashBehaviour : MonoBehaviour
{

    private dealsDamage DealsDamage;
    private GameObject player;
    private playerHealth playerHP;
    public boss3Behaviour Boss3Behaviour;

    public GameObject flashObj;
    public float flashDamage = 5;
    public bool playerCovered = false;
    public float flashDuration = 5f;

    public Animator flashAnim;

    public void flash()
    {
        flashAnim = this.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        flashObj.SetActive(true);
        StartCoroutine(handleFlash());
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player is safe");
            playerCovered = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player is no longer safe");
            playerCovered = false;
        }
    }

    private IEnumerator handleFlash()
    {
        float elapsed = 0f;

        while(elapsed < flashDuration) 
        {
            if (!playerCovered)
            {
                playerHP = player.GetComponent<playerHealth>();

                if (playerHP != null)
                {
                    playerHP.Hit(flashDamage);
                }

            }
            elapsed += Time.deltaTime;

            yield return null;
        }

        flashObj.SetActive(false);

        flashAnim.SetTrigger(animatorStrings.flashBomb);

        Boss3Behaviour = Boss3Behaviour.GetComponent<boss3Behaviour>();
        Boss3Behaviour.SetFinishedMainAttack(true);
        print("SeETTTTTTTTTTT");
    }
}
