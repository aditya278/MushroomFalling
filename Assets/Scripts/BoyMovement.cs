using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMovement : MonoBehaviour
{
    public int health = 3;
    public int score = 0;
    float speed = 2f;
    public Animator boyAnimator;
    SpriteRenderer playerSprite;

    public AudioClip eatClip;
    public AudioClip hurtClip;
    public AudioSource boyAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        if(moveDirection < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(moveDirection > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        var movement = moveDirection * speed * Time.deltaTime;
        transform.Translate(movement, 0f, 0f);
        if(movement != 0f)
        {
            boyAnimator.SetBool("IsWalking", true);
        }
        else
        {
            boyAnimator.SetBool("IsWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            boyAnimator.SetBool("IsDead", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("BadMushroom"))
        {
            boyAudio.clip = hurtClip;
            boyAudio.Play();
            health--;
        }
        else if (collision.collider.tag.Equals("GoodMushroom"))
        {
            boyAudio.clip = eatClip;
            boyAudio.Play();
            score++;
        }
    }
}
