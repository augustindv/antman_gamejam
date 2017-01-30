using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

    public float speed;
    public float slowDown;
    public float jumpForce;
    public float punchForce;
    public GameObject cameraTrigger;
    [HideInInspector]
    public bool facingRight = false;
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool groundedForCrates;
    public Transform cratesList;
    public bool canGetSmall = true;
    public bool startsBig = true;
    [HideInInspector]
    public static bool gameOver = false;
    [HideInInspector]
    public static bool objectGrabbed;

    private Rigidbody2D rb2D;
    private Animator animator;
    private Direction dir = Direction.Still;
    private GameObject objectToGrab;
    private bool isPunching = false;
    private bool isSmall = false;
    private bool goSmall = false;
    private bool justJumped = false;
    private bool canMove = true;
    private float sinceLastJump = 0;
    private float sinceLastSmall = 0;
    private float sinceLastPunch = 0;
    private float sinceLastGrab = 0;
    private float timeRunning = 0;
    // Sound system
    private AudioSource[] audioSources;
    private AudioSource runningSource;
    private AudioSource punchSource;

    // Use this for initialization
    void Start () {
        Flip();

        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        audioSources = GetComponents<AudioSource>();
        runningSource = audioSources[0];
        punchSource = audioSources[1];

        gameOver = false;
        objectGrabbed = false;
    }

    // Update is called once per frame
    void FixedUpdate() {
        animator.SetBool("grounded", grounded);
        animator.SetBool("objectGrabbed", objectGrabbed);

        float horizontal = 0;
        bool jump = false;
        bool grab = false;
        bool small = false;
        bool punch = false;

        canMove = !isPunching && !PauseMenu.isPaused && !gameOver;
        if (canMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            jump = Input.GetButton("Jump");
            grab = Input.GetButton("Fire1");
            punch = Input.GetButton("Punch");
            small = canGetSmall && Input.GetButton("Fire2");
        }


        if (horizontal < -0.15)
        {
            animator.SetBool("run", true);
            dir = Direction.Left;
        } else if (horizontal > 0.15)
        {
            animator.SetBool("run", true);
            dir = Direction.Right;
        } else
        {
            if (!punch && sinceLastPunch > 0.4) timeRunning = 0;
            animator.SetBool("run", false);
            dir = Direction.Still;
        }

        // Sound for running
        if (animator.GetBool("run") && grounded)
        {
            runningSource.mute = false;
            if (isSmall)
            {
                runningSource.pitch = 1.2f;
                animator.speed = 2;
            } else
            {
                runningSource.pitch = 0.85f;
                animator.speed = 1;
            }
        } else
        {
            animator.speed = 1;
            runningSource.mute = true;
        }

        timeRunning += Time.deltaTime;
        rb2D.velocity = new Vector2(runningFunction(timeRunning) / (runningFunction(1)) * horizontal * speed * Time.deltaTime * 60, rb2D.velocity.y);

        if (!jump)
        {
            justJumped = false;
            sinceLastJump = 0;
        }

        if (jump && grounded && !objectGrabbed)
        {
            animator.SetTrigger("jump");
            rb2D.AddForce(Vector2.up * jumpForce/2 * Time.deltaTime * 60, ForceMode2D.Impulse);
            justJumped = true;
            sinceLastJump = 0;
        }

        if (jump && justJumped)
        {
            if (sinceLastJump < 0.1)
            {
                rb2D.AddForce(Vector2.up * jumpForce/15 * Time.deltaTime * 60, ForceMode2D.Impulse);
            }

        }
        sinceLastJump += Time.deltaTime;

        // Grab objects
        if (!groundedForCrates && objectGrabbed)
        {
            grab = true;
        }
        if (sinceLastGrab > 0.3 && grab && !isSmall)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + new Vector3(0, 3, 0), facingRight ? Vector3.right : Vector3.left, 5);
            List<RaycastHit2D> hitsList = hits.ToList<RaycastHit2D>();
            RaycastHit2D hitGrabbable = hitsList.Find(o => o.collider.gameObject.tag == "Grabbable");
            if (hitGrabbable.collider != null)
            {
                GameObject tempObject = hitGrabbable.collider.gameObject;
                if (tempObject.tag == "Grabbable")
                {
                    sinceLastGrab = 0;
                    objectToGrab = tempObject;
                    if (!objectGrabbed && groundedForCrates)
                    {
                        objectToGrab.transform.parent = transform;
                        Destroy(objectToGrab.GetComponent<Rigidbody2D>());
                        objectGrabbed = true;
                        speed -= isSmall ? 25 : 20;
                        rb2D.mass = 20;
                    }
                    else if (grab && objectGrabbed)
                    {
                        objectToGrab.transform.parent = cratesList;
                        objectToGrab.AddComponent<Rigidbody2D>();
                        objectToGrab.GetComponent<Rigidbody2D>().gravityScale = 20;
                        objectToGrab.GetComponent<Rigidbody2D>().mass = 50;
                        objectGrabbed = false;
                        speed += isSmall ? 25 : 20;
                        rb2D.mass = 1;
                    }
                }
            }

            hits = Physics2D.RaycastAll(transform.position + new Vector3(0, 3, 0), facingRight ? Vector3.right : Vector3.left, 15);
            hitsList = hits.ToList<RaycastHit2D>();
            RaycastHit2D[] hitsOtherWay = Physics2D.RaycastAll(transform.position + new Vector3(0, 5, 0), facingRight ? Vector3.left : Vector3.right, 15);
            List<RaycastHit2D> hitsListOtherWay = hitsOtherWay.ToList<RaycastHit2D>();
            hitsList.Union(hitsListOtherWay);
            RaycastHit2D hitLever = hitsList.Find(o => o.collider.gameObject.tag == "Lever");
            if (hitLever.collider != null)
            {
                GameObject tempObject = hitLever.collider.gameObject;
                if (tempObject.tag == "Lever")
                {
                    HingeJoint2D joint = tempObject.GetComponent<HingeJoint2D>();
                    joint.useMotor = true;
                    joint.GetComponent<LeverBehaviour>().activated = true;
                }
            }
        }
        animator.SetBool("push", (dir == Direction.Right && facingRight) || (dir == Direction.Left && !facingRight));
        sinceLastGrab += Time.deltaTime;

        if (horizontal > 0 && !facingRight && !objectGrabbed)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight && !objectGrabbed)
        {
            Flip();
        }

        // Punch
        sinceLastPunch += Time.deltaTime;
        if (punch && !objectGrabbed && sinceLastPunch > 0.3)
        {
            punchSource.Play();
            animator.SetTrigger("punch");
            sinceLastPunch = 0;
            float direction = facingRight ? 1 : -1;
            rb2D.velocity = new Vector2(direction * punchForce * Time.deltaTime * 60, 0);
            isPunching = true;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + (isSmall ? new Vector3(0, 1, 0) : new Vector3(0, 15, 0)), facingRight ? Vector3.right : Vector3.left, 10);
            List<RaycastHit2D> hitsList = hits.ToList<RaycastHit2D>();
            RaycastHit2D hit = hitsList.Find(o => o.collider.gameObject.tag == "Breakable");
            if (hit.collider != null)
            {
                GameObject tempObject = hit.collider.gameObject;
                if (tempObject.tag == "Breakable")
                {
                    tempObject.GetComponent<GlassShatter>().isDestroyed = true;
                }
            }
        }
        if (sinceLastPunch > 0.2)
        {
            isPunching = false;
        }

        // Get smaller and bigger
        sinceLastSmall += Time.deltaTime;
        Vector3 trans = new Vector3(facingRight ? -1 : 1, 1, 1);
        if ((small && sinceLastSmall > 0.2 && !objectGrabbed) || !startsBig)
        {
            startsBig = true;
            sinceLastSmall = 0;
            goSmall = !goSmall;
            if (!goSmall)
            {
                isSmall = false;
                slowDown = 5;
                jumpForce = 85;
                speed -= 10;
                cameraTrigger.GetComponent<CameraTrigger>().smooth = 2;
                cameraTrigger.GetComponent<CameraTrigger>().reset.y = 10;
            }
            else if (goSmall)
            {
                isSmall = true;
                slowDown = 5;
                speed += 10;
                jumpForce = 115;
                cameraTrigger.GetComponent<CameraTrigger>().smooth = 3;
                cameraTrigger.GetComponent<CameraTrigger>().reset.y = 20;
            }
        }
        if (!goSmall)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, trans, 0.2f);
        }
        else if (goSmall)
        {
            Vector3 smallVect = trans / 10.0f;
            transform.localScale = Vector3.Lerp(transform.localScale, smallVect, 0.5f);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    bool canGrabObject(GameObject obj)
    {
        if (facingRight)
        {
            return obj.transform.position.x >= transform.position.x && obj.transform.position.y <= transform.position.y;
        } else
        {
            return obj.transform.position.x <= transform.position.x && obj.transform.position.y <= transform.position.y;
        }
    }

    float runningFunction(float x)
    {
        float a = 1;
        float b = 0.2f;
        float c = 3f;
        float d = 0.1f;
        float e = 0.3f;
        float x2 = x*x;
        //if (x >= 1)
            //return 1;
	    return (x*(a* x + b)) / (x*(c* x + d) + e);

        /*Debug.Log(x);
        if (x < Mathf.Epsilon)
        {
            return 0;
        } else if (x > 0.8)
        {
            return 1;
        } else
        {
            return Mathf.Log(50* x)/2;
        }*/
    }

    public enum Direction {
        Left, Right, Still
    }
}
