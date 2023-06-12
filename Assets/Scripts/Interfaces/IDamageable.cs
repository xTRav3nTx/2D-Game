using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{ 
    float Health { get; }
    bool IsAlive { get; }
    void TakeDamage(float dmgAmt);
}
