using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    public GameObject ps;

    public bool isGrounded = true;

    public Vector3 input = Vector3.zero;

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
    public bool flag4 = false;
    public bool flag5 = false;

    public PhysicMaterial friction;
    public PhysicMaterial frictionless;

    public bool jumpAnim = false;

    public bool enableInput = false;

    public GameObject mainCamera;
    CameraFollowPlayer cameraFollowPlayerScript;
    RippleEffect rippleEffectScript;

    public float cameraShakeMagnitude = 0.02f;
    public float cameraShakeDuration = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cameraFollowPlayerScript = mainCamera.GetComponent<CameraFollowPlayer>();
        rippleEffectScript = mainCamera.GetComponent<RippleEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enableInput)
        {
            if (EnablePlayerMovement())
            {
                //slow movement speed of player in air vs on ground
                PlayerMovement();
            }
            if (input.x == 0)
            {
                GetComponent<CapsuleCollider>().material = friction;
            }
            else
            {
                GetComponent<CapsuleCollider>().material = frictionless;
            }
            //transition to player stanby animation if not moving and on ground
            //transition to player move animation if moving and on ground
            PlayerMovementAnimation();

            //flip player sprite when moving left/right
            PlayerFlipSprite();

            //player double jump, reset if on ground
            PlayerDoubleJump();

            //player dash, can dash once after elasped time is greater than the dash duration
            //player movement speed increases indefinitely after dash, but resets to default if horizontal direction changes
            PlayerDash();

            //reset attack animation parameters if player is on standby, move, or jump end animations
            ResetAttackParameters();

            //player attack sequence
            PlayerAttack();

            //player special attack
            PlayerLimit();

            //player wake
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Return))
            {
                if (flag4 == false)
                {
                    anim.SetTrigger("Alive");
                    flag4 = true;
                }
                else
                {
                    anim.SetTrigger("Wake");
                }
            }
        }
    }


    private bool EnablePlayerMovement()
    {
        //stop movement if dead animation is playing, if both inputs are true, stop movement if attack animation is playing
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead")
            || anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")
            || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
            || (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)))
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
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("playerLimit"))
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            direction = 1;
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("playerLimit"))
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            }
            direction = -1;
        }
    }

    private void PlayerDoubleJump()
    {
        //player double jump, reset if on ground
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !doubleJump
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttackStart")
            && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")
            && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
        {
            jumpAnim = true;
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
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            && elapsedTime > dashDuration
            && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead")
            && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            dash = true;
            if (rb.velocity.x != 0)
            {
                speed = speedFast;
            }
        }
    }

    private void PlayerMovementAnimation()
    {
        //transition to player stanby animation if not moving and on ground
        if (Mathf.Abs(rb.velocity.x) < 0.1f && isGrounded)
        {
            speed = speedDefault;
            ps.GetComponent<ParticleSystem>().startLifetime = 0f;

            anim.SetBool("Move", false);
            anim.SetBool("Standby", true);
        }
        //transition to player move animation if moving and on ground
        if (Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded)
        {
            ps.GetComponent<ParticleSystem>().startLifetime = Mathf.Abs(rb.velocity.x / 20);

            anim.SetBool("Standby", false);
            anim.SetBool("Move", true);
        }
    }

    private void PlayerAttack()
    {
        //player main attack sequence
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttack")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("playerLimit"))
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
        if (isGrounded && anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttackStart"))
        {
            anim.SetBool("DownAttack", false);
            flag5 = false;

        }

        //player down attack
        if (!isGrounded && (doubleJump || !dash) && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            anim.SetBool("DownAttack", true);
        }
    }

    private void PlayerLimit()
    {
        if (PlayerPrefs.GetInt("PlayerMP") == 100 && (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.Mouse1)))
        {
            anim.SetTrigger("Limit");
            FindObjectOfType<RippleEffect>().refractionStrength = 0.75f;
            FindObjectOfType<RippleEffect>().reflectionStrength = 0.75f;
            FindObjectOfType<RippleEffect>().waveSpeed = 2f;

            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

            PlayerPrefs.SetInt("PlayerMP", 0);
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
                    rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * 4 * Time.deltaTime;
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

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && attackSequenceStart && !isGrounded)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            attackSequenceStart = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && attackSequenceStart && isGrounded)
        {
            rb.velocity = Vector3.zero;
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            attackSequenceStart = false;
        }

        //add force when player attacks
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerAttack0") && flag0 == false)
        {
            rb.AddForce(new Vector3(Mathf.Abs(1 + rb.velocity.x) * direction, 0, 0) * attackForce, ForceMode.Impulse);
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
                rb.AddForce(new Vector3(direction, 0, 0) * attackForce * 1.8f, ForceMode.Impulse);
                flag2 = true;
            }
            else
            {
                rb.AddForce(new Vector3(direction * 0.75f, 1, 0) * attackForce * 2.2f, ForceMode.Impulse);
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
        if (dash)
        {
            if (!isGrounded)
            {
                rb.velocity += new Vector3(direction, 1, 0) * dashForce;
            }
            else
            {
                rb.velocity += new Vector3(direction, 0, 0) * dashForce;
            }
            FindObjectOfType<RippleEffect>().refractionStrength = 0.123f;
            FindObjectOfType<RippleEffect>().reflectionStrength = 0.539f;
            FindObjectOfType<RippleEffect>().waveSpeed = 1f;
            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            elapsedTime = 0;
            dash = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttack"))
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void StartCameraShake()
    {
        StartCoroutine(cameraFollowPlayerScript.Shake(cameraShakeDuration, cameraShakeMagnitude));
    }

    private void OnCollisionStay(Collision collision)
    {
        //set player is grounded to true, transition to end jump animation
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("playerDownAttack") && !flag5)
            {
                StartCameraShake();
                flag5 = true;
            }

            isGrounded = true;
            anim.SetBool("Jump", false);
            anim.ResetTrigger("JumpStart");

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //set player is grounded to true, transition to end jump animation
        if (collision.gameObject.CompareTag("Ground"))
        {

            jumpAnim = false;
            anim.SetBool("DownAttack", false);

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //set player is grounded to false
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            ps.GetComponent<ParticleSystem>().startLifetime = 0;

            if (jumpAnim)
            {
                anim.SetBool("Standby", false);
                anim.SetBool("Move", false);
                anim.SetTrigger("JumpStart");
                anim.SetBool("Jump", true);
            }

        }
    }



}
