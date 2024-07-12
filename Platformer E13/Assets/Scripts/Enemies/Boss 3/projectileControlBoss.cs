using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileControlBoss : projectileControl
{

    public override void InitializeDirection(Vector2 direction)
    {
        targetDirection = direction.normalized;
        StartCoroutine(lifeSpanBossAttack());
    }

    IEnumerator lifeSpanBossAttack()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(this.gameObject);

    }

}
