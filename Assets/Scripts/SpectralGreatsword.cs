using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralGreatsword : MonoBehaviour
{
    public int damage = 20;
    public int combo = 0;
    public float attackRate = 0.5f;
    public float nextAttack;

    private Vector2 center;
    public float rotationRadius = 2.3f;
    public float rotateSpeed = 5f;
    private float tempSpeed = 0;
    private float angle;

    public float previousAngle;
    public float totalAngle = 0;
    private bool stopFlag = true;
    public Vector2 attackDirection;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 2.1f, 0);
        previousAngle = transform.eulerAngles.z;
        nextAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().nextAttack; // Get the nextAttack value from the playerCombat script
    }

    private void Update()
    {

        if (Time.time >= nextAttack)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameObject.SetActive(true);

                if (attackDirection.x >= 0.1)
                {
                    // Sword starts from the right
                    previousAngle = -12.5f;
                }
                if (attackDirection.x <= -0.1)
                {
                    // Sword starts from the left
                    previousAngle = 192f;
                }
                if (attackDirection.y >= 0.1)
                {
                    // Sword starts from above
                    previousAngle = 102f;
                }
                if (attackDirection.y <= -0.1)
                {
                    // Sword starts from below
                    previousAngle = 282f;
                }
                if (attackDirection.x == 0 && attackDirection.y == 0)
                {
                    // Sword starts at default stance
                    previousAngle = -12.5f;
                }
                nextAttack = Time.time + attackRate;
                totalAngle = 0;
                gameObject.GetComponent<Renderer>().enabled = true;
                gameObject.GetComponent<Collider2D>().enabled = true;
                rotateSpeed = tempSpeed;
                stopFlag = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyCombat enemy = collision.GetComponent<EnemyCombat>();
        BossFIght boss = collision.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        else if (boss != null && boss.CompareTag("Boss"))
        {
            boss.TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Have the sword sweep around the player
        center = GameObject.FindGameObjectWithTag("Player").transform.position;
        angle += rotateSpeed * Time.deltaTime * -1;
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * rotationRadius;
        transform.position = center + offset;

        // Rotates the Sword while its moving around the player
        Vector2 dir = (new Vector2(transform.position.x, transform.position.y) - center).normalized;
        float rotateAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward);

        // Track how many degrees the sword has been rotated
        var currentAngle = transform.eulerAngles.z;
        totalAngle += Mathf.Abs(currentAngle - previousAngle);
        previousAngle = currentAngle;

        // Did the angle reach 360 degrees?
        if (Mathf.Abs(totalAngle) >= 360f && stopFlag)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            tempSpeed = rotateSpeed;
            rotateSpeed = 0;
            stopFlag = false;
        }
    }
}
