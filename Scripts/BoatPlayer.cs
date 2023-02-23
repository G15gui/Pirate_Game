using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatPlayer : Boat
{
    static BoatPlayer boatPlayer;
    static Vector3 savePlayerPositon;
    public static Vector3 playerPositon 
    {
        get 
        {
            if (boatPlayer)
                savePlayerPositon = boatPlayer.transform.position;
            return savePlayerPositon;
        } 
    }
    void Awake()
    {
        boatPlayer = this;
    }
    void Update()
    {
        if (Menu.GamePause) return;
        if (Dead) return;

        if (CheckMouse0Input()) { SetTargetPosition(Input.mousePosition); }
        if (CheckMouse1Input() && !MoveOn) { LookAt((Vector2)Input.mousePosition); }
        Movement();
        Attacks();
    }
    #region "CheckInputFunctions"
    bool CheckMouse0Input()
    {
        return Input.GetMouseButtonDown(0);
    }
    bool CheckMouse1Input()
    {
        return Input.GetMouseButtonDown(1);
    }
    #endregion
    #region "AttacksFunctions"
    void Attacks()
    {
        if (Input.GetKeyDown(KeyCode.E)) { Attack1(); }
        if (Input.GetKeyDown(KeyCode.W)) { Attack2(); }
        if (Input.GetKeyDown(KeyCode.Q)) { Attack3(); }
    }
    #endregion
    #region "CollisionFunctions"
    void OnCollisionEnter2D(Collision2D collision)
    {
        MoveOn = false;
        Speed = minSpeed;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            DamageLife();
        }
        if (collision.gameObject.CompareTag("BoatEnemy"))
        {
            DamageLife();
        }
    }
    void OnCollisionStay2D()
    {
        OnCollision = true;
    }
    void OnCollisionExit2D()
    {
        OnCollision = false;
    }
    #endregion
    private void OnDestroy()
    {
        if (Dead) Menu.PlayerDeath();
    }
}
