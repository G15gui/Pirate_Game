using System;
using System.Collections;
using UnityEngine;

public class BoatEnemy : Boat
{
    #region "CollisionFunctions"
    void OnCollisionEnter2D(Collision2D collision)
    {
        MoveOn = false;
        Speed = minSpeed;
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            DamageLife();
        }
        if (collision.gameObject.CompareTag("BoatPlayer"))
        {
            if(AIMode == AIModes.AIChaser) StartCoroutine(_Dead());
        }
    }
    void OnCollisionStay2D()
    {
        OnCollision = true;
    }
    void OnCollisionExit2D()
    {
        OnCollision = false;
        MoveOn = true;
    }
    #endregion


    [Header("AI")]
    [SerializeField] AIModes AIMode;
    [SerializeField] float attackDelay = 5f;
    [SerializeField] float safeDistance = 1f;
    bool canAttack = true;
    bool Stuck;
    enum AIModes { AIChaser, AIShooter }
    enum AttacksModes { Attack1, Attack2, Attack3 }

    void Update()
    {
        if (Menu.GamePause) { return; }
        if (Dead) { return; }

        if (AIMode == AIModes.AIChaser) _AIChaser();
        else if (AIMode == AIModes.AIShooter) _AIShooter();
    }
    void _AIChaser()
    {
        if (!OnCollision || Stuck)
        {
            targetPosition = BoatPlayer.playerPositon;
            MoveOn = true;
            LookAt(BoatPlayer.playerPositon);
            Movement();
            Stuck = false;
        }
        else
        {
            MoveOn = false;
            LookAt();
            Stuck = true;
        }
    }
    void _AIShooter()
    {
        var dist = Vector2.Distance(this.transform.position, BoatPlayer.playerPositon);
        if (dist > attacksDistance) { _AIChaser(); }
        else
        {
            if (dist > safeDistance)
                _AIChaser();

            LookAt(BoatPlayer.playerPositon);
            LookAt();
            AIAttacks();
        }
    }
    void AIAttacks()
    {
        if(canAttack)
        {
            canAttack = false;
            var randomAttack = UnityEngine.Random.Range(0, 3);
            var attackMode = (AttacksModes)randomAttack;
            Invoke(attackMode.ToString(), 0);
            StartCoroutine(Wait(attackDelay, delegate() { canAttack = true; }));
        }
    }
    IEnumerator Wait(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }
}
