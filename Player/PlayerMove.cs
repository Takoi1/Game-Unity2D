using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    private Rigidbody2D body;
    private Animator anim;
    //private bool grounded; // remplace grounded var by is*Ground method
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCoolDown;
    private float horizontalInput;
    public Text WINTEXT;


    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private void Awake()
    {
        //2.1 grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "win")
        {
            WINTEXT.gameObject.SetActive(true);
        }
    }

    //update 

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");


        //1 flip player when moving left and right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;

        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);



        //2.2set animator parametres 

        /* first zexpe 
         
         * arrow keys nit presse => horizental iput =0 , 
         * is not equal to 0 ? =false ,,
         * run =false (player not runnnung)
         */


        /*Seconf logic ::
         * arrow keys  presse
         * => horizental iput !=0 , 
         * is 1not  equal to 0 ? =ttrue ,,
         * run =true (player  runnnung)
         * 
         */
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //wall jump logic 
        if (wallJumpCoolDown > 0.2f)
        {

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;


            if (Input.GetKey(KeyCode.Space))
            {
                Jump();

                if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
                    SoundManager.instance.PlaySound(jumpSound);


            }


        }
        else
            wallJumpCoolDown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {

            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCoolDown = 0;

        }


    }



    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }




    //wall jump
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

}
