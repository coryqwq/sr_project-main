using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    private Rigidbody rb;
    public bool isGrounded = true;

    private Vector3 input = Vector3.zero;

    public float speed = 1f;
    public float speedDefault = 1f;
    public float speedFast = 2f;

    public float jumpForce = 1f;
    public bool doubleJump = false;

    public float fallMultiplier = 2f;
    public float jumpMultiplier = 2f;

    public float dashForce = 2f;
    public float attackForce = 2f;
    public float dashDuration = 1f;
    public float elapsedTime = 0f;

    public bool jump = false;
    public bool dash = false;
    public bool attackSequenceStart = false;
    public int attackPhase = 0;
    public int direction = 1;
    public bool flag0 = false;
    public bool flag1 = false;
    public bool flag2 = false;

    public bool flag3 = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (EnablePlayerMovement())
        {
            //slow movement speed of player in air vs on ground
            PlayerMovement();
        }

        //flip player sprite when moving left/right
        PlayerFlipSprite();

        //player double jump, reset if on ground
        PlayerDoubleJump();

        //player dash, can dash once after elasped time is greater than the dash duration
        //player movement speed increases indefinitely after dash, but resets to default if horizontal direction changes
        PlayerDash();

        //transition to player stanby animation if not moving and on ground
        //transition to player move animation if moving and on ground
        PlayerMovementAnimation();

        //reset attack animation parameters if player is on standby, move, or jump end animations
        ResetAttackParameters();

        //player attack sequence
        PlayerAttack();
    }


    private bool EnablePlayerMovement()
    {
        //stop movement if both inputs are true, //stop movement if attack animation is playing
        if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)) || anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            input.x = 0;
            return false;
        }
        else
        {
            return true;
        }
    }
    private void ResetAttackParameters()
    {
        //reset attack animation parameters if player is on standby, move, or jump end animations
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("ResetAttack"))
        {
            attackPhase = 0;
            anim.ResetTrigger("Attack0");
            anim.SetBool("Attack1", false);
            attackSequenceStart = true;
            flag0 = false;
            flag1 = false;
            flag2 = false;
        }
    }

    private void PlayerMovement()
    {
        //slow movement speed of player in air vs on ground
        if (!isGrounded)
        {
            input.x = Input.GetAxisRaw("Horizontal") * speed * 0.8f;
        }
        else
        {
            input.x = Input.GetAxisRaw("Horizontal") * speed;
        }
    }

    private void PlayerFlipSprite()
    {
        //flip player sprite when moving left/right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            direction = 1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            direction = -1;
        }
    }

    private void PlayerDoubleJump()
    {
        //player double jump, reset if on ground
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !doubleJump && 
            !anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttackStart")  && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            jump = true;
            if (!isGrounded)
            {
                doubleJump = true;
            }
        }
        else if (isGrounded)
        {
            doubleJump = false;
        }
    }

    private void PlayerDash()
    {
        //player dash, can dash once after elasped time is greater than the dash duration
        //player movement speed increases indefinitely after dash, but resets to default if horizontal direction changes
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && elapsedTime > dashDuration)
        {
            dash = true;
            if (input.x != 0)
            {
                speed = speedFast;
            }
        }
    }

    private void PlayerMovementAnimation()
    {
        //transition to player stanby animation if not moving and on ground
        if (Mathf.Abs(input.x) == 0 && isGrounded)
        {
            speed = speedDefault;
            anim.SetBool("Move", false);
            anim.SetBool("Standby", true);
        }
        //transition to player move animation if moving and on ground
        if (Mathf.Abs(input.x) > 0 && isGrounded)
        {
            anim.SetBool("Standby", false);
            anim.SetBool("Move", true);
        }
    }

    private void PlayerAttack()
    {
        //player main attack sequence
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && !anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttack"))
        {
            if (attackPhase == 0)
            {
                anim.SetTrigger("Attack0");
                attackPhase++;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerAttack0") && attackPhase == 1)
            {
                anim.ResetTrigger("Attack0");
                anim.SetBool("Attack1", true);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerAttack1") && attackPhase == 1)
            {
                anim.SetBool("Attack1", false);
                anim.SetBool("DownAttack", true);
            }
        }
        if (isGrounded && anim.GetCurrentAnimatorStateInfo(0).IsName("playerAttack2"))
        {
            anim.SetBool("DownAttack", false);
        }

        //player down attack
        if (!isGrounded && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            anim.SetBool("DownAttack", true);
        }
    }

    private void FixedUpdate()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            rb.useGravity = true;
            //change player fall curve 
            if (rb.velocity.y < 0)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttackStart"))
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * 5 * Time.deltaTime;
                }
                else
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
                }
            }
            //change player jump height on input held down
            if (rb.velocity.y > 0 && (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * jumpMultiplier * Time.deltaTime;
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && attackSequenceStart)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            attackSequenceStart = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerAttack0") && flag0 == false)
        {
            rb.AddForce(new Vector3(Mathf.Abs(1 + rb.velocity.x) * direction, 0, 0) * dashForce, ForceMode.Impulse);
            flag0 = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerAttack1") && flag1 == false)
        {
            rb.AddForce(new Vector3(direction, 0, 0) * attackForce, ForceMode.Impulse);
            flag1 = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerAttack2") && flag2 == false)
        {
            if (isGrounded)
            {
                rb.AddForce(new Vector3(direction, 0, 0) * attackForce * 2, ForceMode.Impulse);
                flag2 = true;
            }
            else
            {
                rb.AddForce(new Vector3(direction, 2, 0) * attackForce, ForceMode.Impulse);
                flag2 = true;
            }
        }

        //player jump physics
        if (jump)
        {
            rb.velocity = Vector3.up * jumpForce;
            jump = false;
        }

        //player horizontal movement physics
        //player dash applies horizontal vector in current direction
        //disable horizontal movement control during dash for the dash duration 
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime > dashDuration && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            rb.velocity = new Vector3(input.x, rb.velocity.y, 0);
        }
        if (dash && !anim.GetCurrentAnimatorStateInfo(0).IsName("playerStandby"))
        {
            elapsedTime = 0;
            rb.velocity += new Vector3(direction, 0, 0) * dashForce;
            rb.velocity += new Vector3(0, 1, 0) * dashForce * 0.25f;

            dash = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //set player is grounded to true, transition to end jump animation
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("Jump", false);
            anim.ResetTrigger("JumpStart");
            anim.SetBool("DownAttack", false);

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //set player is grounded to false, transition to start jump and jump animation
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            anim.SetBool("Standby", false);
            anim.SetBool("Move", false);
            anim.SetTrigger("JumpStart");
            anim.SetBool("Jump", true);
        }
    }
}
