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
    public float vectorTolerance;

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

    private bool NoVector(float moveVector){
        if (moveVector == 0){
            return true;
        }
        return false;
    }

    private bool Idle(float xVector, float yVector){
        if (Mathf.Round(xVector) == 0 && Mathf.Round(yVector) == 0){
            return true;
        }
        return false;
    }

    private bool isNegative(float givenVector){
        if (givenVector < 0){
            return true;
        }
        return false;
    }

    private void AnimateWalk(Vector3 direction){
        Debug.Log(direction);
        animator.speed = 0.5f;
        if (NoVector(direction.z) && direction.x < -vectorTolerance){
            Debug.Log("walkforward");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkForward");
            currentAnimation = "RenForward";
        }
        else if (NoVector(direction.z) && direction.x > vectorTolerance){
            Debug.Log("walkback");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkBack");
            currentAnimation = "RenBack";
        }
        else if (NoVector(direction.x) && direction.z > vectorTolerance){
            Debug.Log("WalkLeft");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkLeft");
            currentAnimation = "RenLeft";
        }
        else if (NoVector(direction.x) && direction.z < -vectorTolerance){
            Debug.Log("WalkRight");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("WalkRight");
            currentAnimation = "RenRight";
        }
        else if (direction.x < vectorTolerance && direction.z > vectorTolerance){
            Debug.Log("tiltleftforward");
            animator.ResetTrigger("Stop");
            animator.SetTrigger("FrontTiltLeft");
            currentAnimation = "RenFrontTiltLeft";
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

    private void AnimateWalk(float xVector, float yVector){
        Debug.Log(xVector + ", " + yVector);
        animator.speed = 0.5f;
        if (!Idle(xVector, yVector)){
            if (isNegative(yVector) && isNegative(xVector)){
                Debug.Log("tiltleftforward");
                animator.SetTrigger("FrontTiltLeft");
                currentAnimation = "RenFrontTiltLeft";
            }
            else if (!NoVector(xVector) && !isNegative(xVector) && isNegative(yVector)){
                Debug.Log("tiltrightforward");
                animator.SetTrigger("FrontTiltRight");
                currentAnimation = "RenFrontTiltRight";
            }
            else if (!NoVector(xVector) && !NoVector(yVector) && !isNegative(xVector) && !isNegative(yVector)){
                Debug.Log("tiltrightback");
                animator.SetTrigger("BackTiltRight");
                currentAnimation = "RenBackTiltRight";
            }
            else if (!NoVector(xVector) && !NoVector(yVector) && isNegative(xVector) && !isNegative(yVector)){
                Debug.Log("tiltleftback");
                animator.SetTrigger("BackTiltLeft");
                currentAnimation = "RenBackTiltLeft";
            }
            else if (NoVector(xVector) && isNegative(yVector)){
                Debug.Log("walkforward");
                animator.SetTrigger("WalkForward");
                currentAnimation = "RenForward";
            }
            else if (NoVector(xVector) && !isNegative(yVector)){
                Debug.Log("walkback");
                animator.SetTrigger("WalkBack");
                currentAnimation = "RenBack";
            }
            else if (NoVector(yVector) && isNegative(xVector)){
                Debug.Log("WalkLeft");
                animator.SetTrigger("WalkLeft");
                currentAnimation = "RenLeft";
            }
            else if (NoVector(yVector) && !isNegative(xVector)){
                Debug.Log("WalkRight");
                animator.SetTrigger("WalkRight");
                currentAnimation = "RenRight";
            }
        }
        else {
            Debug.Log("stop");
            animator.ResetTrigger("WalkForward");
            animator.ResetTrigger("WalkLeft");
            animator.ResetTrigger("WalkRight");
            animator.ResetTrigger("WalkBack");
            animator.ResetTrigger("FrontTiltLeft");
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
        _rb.velocity = Vector3.ClampMagnitude(moveDir * Time.deltaTime, 10.0f);
        Debug.Log(_rb.velocity);
        //AnimateWalk(moveDir);
        AnimateWalk(xVector, yVector);
        
    }
}
