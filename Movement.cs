using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    PhotonView photonView;
    public TMP_Text name;

    [SerializeField] Animator animator;

    //[SerializeField] GameObject floor;
    public BoxCollider2D swordCollider;
    public Rigidbody2D rb;
    Vector2 movement;

    bool dying = false;
    bool isJump = false;
    int jumps = 0;
    //List<float> floors = new List<float>();
    bool attacking = false;

    float ypos;
    float xpos;

    // Start is called before the first frame update
    void Start()
    {
        /*
        Vector2 vector2 = new Vector2(10, 0);
        floors.Add(-4f);
        for (int i = 0; i < 100; i++)
        {
            int random = UnityEngine.Random.Range(0, 2);
            Instantiate(floor, vector2, Quaternion.identity);
            floors.Add(vector2.y);
            vector2.x += 10.5f;
            vector2.y += 4f + random;
        }
        */
        photonView = GetComponent<PhotonView>();
        name.text = photonView.Controller.NickName;

        rb = GetComponent<Rigidbody2D>();
        movement = new Vector3();

        ypos = -4f; 
        xpos = 0f;

        //level.text = "Floor " + floors.IndexOf(ypos);
    }

    // Update is called once per frame
    void Update()
    {
        //movement.y = Input.GetAxisRaw("Vertical");

        /*
        if(movement.x == 1)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if(movement.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        */

        if(Input.GetKeyDown(KeyCode.W) && isJump == false && jumps == 1)
        {
            jumps = 0;
            isJump = true;
            animator.SetBool("jump", true);
        }
        name.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
        name.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            //rb.AddForce(movement*10 * Time.deltaTime, ForceMode2D.Force);

            movement.x = Input.GetAxisRaw("Horizontal");


            if (Input.GetKey(KeyCode.S))
            {
                UnityEngine.Debug.Log(jumps);
                animator.SetBool("crouch", true);
                UnityEngine.Debug.Log("Crouch");
            }
            else if (!Input.GetKey(KeyCode.S))
                animator.SetBool("crouch", false);



            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(attack());
                animator.SetTrigger("attack");
            }
            if(attacking == false)
                swordCollider.GetComponent<BoxCollider2D>().enabled = false;


            if (movement.x != 0)
            {
                animator.SetBool("run", true);
                if (movement.x == 1)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    name.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    name.transform.localScale = new Vector3(1f, 1f, 1f);
                }

            }
            else
            {
                animator.SetBool("run", false);
            }

            rb.velocity = new Vector2(movement.x * 10, rb.velocity.y);

            if (isJump && jumps == 0)
            {
                //rb.AddForce(new Vector2(0f, 400f), ForceMode2D.Impulse);
                rb.velocity = new Vector2(rb.velocity.x, 9f);
                isJump = false;
            }

            /*
            if(movement.y == 1f && !isJump)
            {
                rb.AddForce(new Vector2(0f, movement.y*10), ForceMode2D.Impulse);
            }
            */

            if (((rb.position.y < (ypos - 3)) && (rb.position.x > xpos)) || rb.position.y < -5 && !dying)
            {
                //animator.SetTrigger("death");
                StartCoroutine(death());
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {

            /*
            isJump = false;
            if (movement.y == 1f && !isJump)
            {
                rb.AddForce(new Vector2(0f, movement.y * 10), ForceMode2D.Impulse);
            }
            */
            ypos = collision.gameObject.transform.position.y;
            xpos = collision.gameObject.transform.position.x;
            /*
            if(floors.Contains(ypos))
                level.text = "Floor " + floors.IndexOf(ypos);
            */
            animator.SetBool("jump", false);
            jumps = 1;
            isJump = false;
            UnityEngine.Debug.Log("Floor");
        }
        if (collision.gameObject.CompareTag("Edge"))
        {
            jumps = 0;
            animator.SetBool("jump", false);
            animator.SetBool("run", true);
        }
        if(collision.gameObject.CompareTag("Sword"))
        {
            Debug.Log("Sword");
            photonView.RPC("die", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void die()
    {
        //PhotonNetwork.Destroy(this.gameObject);
        Time.timeScale = 0f;
        //NetworkManager.instance.gameOver.SetActive(true);
        PhotonNetwork.Instantiate("GameOver", Vector3.zero, Quaternion.identity);
    }

    private IEnumerator death()
    {
        if (!dying)
        {
            dying = true;
            animator.SetTrigger("death");
        }
        yield return new WaitForSeconds(0.35f);
        respawn();
    }

    private void respawn()
    {
        dying = false;
        rb.position = new Vector2(0, -0.21f);
        ypos = -4f;
    }

    private IEnumerator attack()
    {
        if (!attacking)
        {
            attacking = true;
            swordCollider.GetComponent<BoxCollider2D>().enabled = true;
            animator.SetTrigger("attack");
        }
        yield return new WaitForSeconds(0.70f);
        attackfalse();
    }

    private void attackfalse()
    {
        attacking = false;
    }
}
