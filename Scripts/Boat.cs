using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region "AttacksClasses"
public class Attack
{
    public Vector3 target;
    public float speed;
}
#endregion

public class Boat : MonoBehaviour
{
    #region "MovementVariables"
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float accelerationSpeed = 2f;
    [SerializeField] protected float minSpeed = 1f;
    [SerializeField] protected float Speed = 1f;
    [SerializeField] protected float checkCollision = 2f;
    protected Vector2 targetPosition;
    protected Vector2 lookAtTarget;
    protected bool MoveOn = false;
    protected bool LookAtOn = false;
    protected bool OnCollision = false;
    #endregion
    #region "Attacks Variables"
    [Header("Attacks")]
    [SerializeField] protected float attacksDistance;
    [SerializeField] protected float attacksSpeed;
    [SerializeField] protected GameObject cannonBall;
    [SerializeField] protected List<Transform> spawnList;
    [SerializeField] protected string attacksTag;
    #endregion
    protected void OnDrawGizmos()
    {
        foreach (var s in spawnList)
        {
            var direction = s.transform.position + -s.transform.up * attacksDistance;
            Gizmos.DrawLine(s.transform.position, direction);
        }
    }
    #region "AttacksFunctions"
    protected void Attack(Transform transform)
    {
        var g = Instantiate(cannonBall);
        Attack attack1 = new Attack();
        attack1.speed = attacksSpeed;
        g.tag = attacksTag;
        attack1.target = (transform.position + -transform.up * (attacksDistance + Speed - minSpeed));
        g.transform.position = transform.position;
        g.GetComponent<CannonBall>().attack = attack1;
        g.SetActive(true);
    }
    protected void Attack1()
    {
        Attack(spawnList[0]);
    }
    protected void Attack2()
    {
        Attack(spawnList[0]);
        Attack(spawnList[1]);
        Attack(spawnList[2]);
    }
    protected void Attack3()
    {
        Attack(spawnList[0]);
        Attack(spawnList[3]);
        Attack(spawnList[4]);
    }
    #endregion
    #region "LifeVariables"
    [Header("Life")]
    [SerializeField] protected Slider lifeSlider;
    protected bool Dead = false;
    [SerializeField] protected float animDeadSpeed = 1f;
    [SerializeField] protected GameObject ExplosionPrefab;
    [SerializeField] protected float explosion = .075f;
    #endregion
    #region "LifeFunctions"
    protected IEnumerator _Dead()
    {
        Dead = true;
        lifeSlider.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        transform.Find("Circle").GetComponent<SpriteRenderer>().enabled = false;
        yield return true;
        var cs = GetComponent<ChangeSprite>();
        cs.ChangeSpriteRenderer(cs.spritesList[cs.spritesList.Count - 1 - (int)lifeSlider.minValue].sprite);

        var e = Instantiate(ExplosionPrefab);
        e.transform.position = transform.position;
        e.SetActive(true);
        yield return true;
        cs = e.GetComponent<ChangeSprite>();

        cs.ChangeSpriteRenderer(cs.spritesList[2].sprite);
        yield return true;
        yield return new WaitForSeconds(explosion);
        cs.ChangeSpriteRenderer(cs.spritesList[1].sprite);
        yield return true;
        yield return new WaitForSeconds(explosion);
        cs.ChangeSpriteRenderer(cs.spritesList[0].sprite);
        yield return true;
        yield return new WaitForSeconds(explosion);
        cs.ChangeSpriteRenderer(cs.spritesList[1].sprite);
        yield return true;
        yield return new WaitForSeconds(explosion);
        cs.ChangeSpriteRenderer(cs.spritesList[2].sprite);
        yield return true;
        yield return new WaitForSeconds(explosion);
        Destroy(e);

        var Renderer = GetComponent<SpriteRenderer>();
        while (Renderer.color.a > 0)
        {
            var color = Renderer.color;
            color.a -= Time.deltaTime * animDeadSpeed;
            Renderer.color = color;
            yield return true;
        }
        Destroy(this.gameObject);
    }
    #endregion

    #region "MovementFunctions"
    protected void Movement()
    {
        LookAt();
        MoveToTargetPosition();
    }
    protected void SetTargetPosition(Vector2 position)
    {
        targetPosition = Camera.main.ScreenToWorldPoint(position);
        lookAtTarget = Camera.main.ScreenToWorldPoint(position);
        MoveOn = true;
        LookAtOn = true;
        StartCoroutine(CheckOnCollision());
    }
    protected void MoveToTargetPosition()
    {
        if (MoveOn)
        {
            if (Speed < moveSpeed) Speed += Time.deltaTime * accelerationSpeed;
            else Speed = moveSpeed;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
            if (transform.position == (Vector3)targetPosition)
            {
                MoveOn = false;
                Speed = minSpeed;
            }
        }
    }
    IEnumerator CheckOnCollision()
    {
        yield return new WaitForSeconds(Time.deltaTime * checkCollision);
        MoveOn = !OnCollision;
    }
    protected void LookAt(Vector2 position)
    {
        lookAtTarget = Camera.main.ScreenToWorldPoint(position);
        LookAtOn = true;
    }
    protected void LookAt(Vector3 position)
    {
        lookAtTarget = (position);
        LookAtOn = true;
    }
    protected void LookAt()
    {
        if (LookAtOn)
        {
            Vector2 direction = lookAtTarget - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            LookAtOn = false;
        }
    }
    #endregion
    #region "LifeFunctions"
    protected void DamageLife()
    {
        lifeSlider.value -= 1;

        var cs = GetComponent<ChangeSprite>();
        if (lifeSlider.value >= lifeSlider.minValue && lifeSlider.value < cs.spritesList.Count)
            cs.ChangeSpriteRenderer(cs.spritesList[cs.spritesList.Count - 1 - (int)lifeSlider.value].sprite);

        if (lifeSlider.value <= lifeSlider.minValue && !Dead)
            StartCoroutine(_Dead());

    }
    #endregion


}
