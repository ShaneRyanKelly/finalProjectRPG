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
        if (PlayerPrefs.HasKey("exitName")){
            this.transform.position = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"), PlayerPrefs.GetFloat("z"));
            ClearLevelPrefs();
        }
        _rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision){

    }

    void OnCollisionExit(Collision collision){
        
    }

    // Update is called once per frame
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

    }
}
