using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagableEntity
{
    public float VitalPoints { set; get;}

    public float MaxHealth { set; get;}

    public float MaxPoise { set; get;}

    public float Health { set; get;}

    public float Poise { set; get;}

    public float IsAlive { set; get;}

    public float IsStunned { set; get;}

    void Update();

    public void handleStun();

    public IEnumerator stunned(float duration);

    public void onHit(float damage,float poiseDamage);


}
