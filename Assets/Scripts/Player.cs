using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float xVector;
    public float yVector;
    Rigidbody _rb;
    public GameObject platform;
    bool grounded = true;
    private Animator animator;
    private string currentAnimation;

    void ClearLevelPrefs(){
        PlayerPrefs.DeleteKey("exitName");
        PlayerPrefs.DeleteKey("x");
        PlayerPrefs.DeleteKey("y");
        PlayerPrefs.DeleteKey("z");
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("playerName", "Shane");
        PlayerPrefs.SetString("friendName", "Jinglu");
        if (PlayerPrefs.HasKey("exitName")){
            this.transform.position = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"), PlayerPrefs.GetFloat("z"));
            ClearLevelPrefs();
        }
        _rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //animator.SetTrigger("WalkForward");
    }

    /*void OnCollisionEnter(Collision collision){
        
        if (collision.gameObject.tag == "Ground"){
            grounded = true;
        }
    }

    void OnCollisionExit(Collision collision){
        Debug.Log("collision exit");
        if (collision.gameObject.tag == "Ground"){
            grounded = false;
        }
    }

    void OnCollisionStay(Collision collision){
        if (collision.gameObject.tag == "Ground"){
            grounded = true;
        }
    }*/

    private void AnimateWalk(Vector3 direction){
        Debug.Log(direction);
        animator.speed = 0.5f;
        if (direction.x < -250f){
            Debug.Log("walkforward");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkForward");
            currentAnimation = "RenForward";
        }
        else if (direction.x > 250f){
            Debug.Log("walkback");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkBack");
            currentAnimation = "RenBack";
        }
        else if (direction.z > 250f){
            Debug.Log("WalkLeft");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkLeft");
            currentAnimation = "RenLeft";
        }
        else if (direction.z < -250f){
            Debug.Log("WalkRight");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkRight");
            currentAnimation = "RenRight";
        }
        else {
            Debug.Log("stop");
            animator.ResetTrigger("WalkForward");
            animator.ResetTrigger("WalkLeft");
            animator.ResetTrigger("WalkRight");
            animator.ResetTrigger("WalkBack");
            animator.Play(currentAnimation, 0, 1/3.0f);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    void FixedUpdate()
    {
        xVector = Input.GetAxis("Horizontal");
        yVector = Input.GetAxis("Vertical");

        

        //assuming we only using the single camera:
        var camera = UnityEngine.Camera.main;
    
        //camera forward and right vectors:
        var forward = camera.transform.forward;
        var right = camera.transform.right;
    
        //project forward and right vectors on the horizontal plane (y = 0)
        
        
        forward.y = 0f;
        right.y = 0f;
        
        
        forward.Normalize();
        right.Normalize();

        var moveDir = ( yVector * forward + xVector * right ) * moveSpeed;
        

        //this.transform.Translate(Vector3.forward * yVector * Time.deltaTime);
        //this.transform.Translate(Vector3.right * xVector * Time.deltaTime);

        //_rb.MovePosition(transform.position + moveDir * Time.deltaTime);
        _rb.velocity = moveDir * Time.deltaTime;
        AnimateWalk(moveDir);
        
    }
}
