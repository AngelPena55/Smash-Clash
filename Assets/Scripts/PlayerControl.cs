using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 mousePos;
    public Camera cam;
    public Animator runwalk;
    bool isMoving = false;
    public AudioSource walkingSrc;
    public Material newmat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // input
    void Update()
    {
        // deals with all movement and dnimations
        ProcessInputs();
        runwalk.SetFloat("Horizontal", moveDirection.x);
        runwalk.SetFloat("Vertical", moveDirection.y);
        runwalk.SetFloat("MoveSpeed", moveDirection.sqrMagnitude);

        // chcecks last position used for spells and attacks
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) {
            runwalk.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            runwalk.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

        // Extra check to process where to position SpectralGreatsword
        if (GameObject.Find("Spectral_GreatSword") != null)
        {
            GameObject.Find("Spectral_GreatSword").GetComponent<SpectralGreatsword>().attackDirection = moveDirection;
        }

        //audio for player walking
        playwalkingclip();
    }

    //physics calculations
    void FixedUpdate() {
        Move();

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

    }

    void ProcessInputs() {
        // move inputs
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        //Debug.Log(moveX);
        //Debug.Log(moveY);

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        // changing up how movement works to make wall rigidbodies work -Will
        //this.rb.MovePosition(this.rb.position + (moveDirection * moveSpeed * Time.deltaTime));

        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        GetComponent<PlayerCombat>().setDirection(moveDirection.x, moveDirection.y);



    }

    public void playwalkingclip()
    {
        
        // checking for the audio source to play 
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            if (!walkingSrc.isPlaying)
                walkingSrc.Play();
        }
        else
            walkingSrc.Stop();
    }
}
