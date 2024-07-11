using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileControlBoss : projectileControl
{
    public boss3Behaviour Boss3Behaviour;

    public override void InitializeDirection(Vector2 direction)
    {
        targetDirection = direction.normalized;
        StartCoroutine(lifeSpanBossAttack());
    }

    IEnumerator lifeSpanBossAttack()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(this.gameObject);

        Debug.Log("EXEEEECUTEEE");
        
        // THIS DOES NOT CHANGE
        Boss3Behaviour = GameObject.FindWithTag("Boss").GetComponent<boss3Behaviour>();
        Boss3Behaviour.SetFinishedMainAttack(true);

        
    }

}
