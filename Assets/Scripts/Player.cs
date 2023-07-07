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
    private Rigidbody _rb;
    public float JumpVelocity = 5f;
    private bool _isJumping;
    private bool isGrounded;
    private bool onPlatform;
    public GameObject platform;

    private Vector3[] startingPositions = {
        new Vector3(10.0f, 1.0f, 0.0f),
        new Vector3(6.0f, 1.0f, -10.0f),
        new Vector3(6.0f, 1.0f, 10.0f),
        new Vector3(26.0f, 1.0f, 10.0f),
        new Vector3(7.5f, 1.0f, -3.0f),
    };

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("lastLevel") && PlayerPrefs.GetInt("lastLevel") != 0){
            Debug.Log(startingPositions.Length);
            this.transform.position = startingPositions[PlayerPrefs.GetInt("lastLevel")];
            PlayerPrefs.DeleteKey("lastLevel");
        }
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        if (_isJumping && isGrounded){
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
        }

        _isJumping = false;
    }

    void OnCollisionEnter(Collision collision){
        /*if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("platform")){
            onPlatform = true;
        }*/
    }

    void OnCollisionExit(Collision collision){
        /*if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
        else if (collision.gameObject.CompareTag("platform")){
            onPlatform = false;
            this.transform.parent = null;
        }
        else if (collision.gameObject.CompareTag("disappearingplatform"))
        {
            Destroy(collision.gameObject);
        }*/
    }

    void OnTriggerEnter(Collider myCollider){
        PlayerPrefs.SetInt("lastLevel", SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        xVector = Input.GetAxis("Horizontal") * moveSpeed;
        yVector = Input.GetAxis("Vertical") * moveSpeed;

        this.transform.Translate(Vector3.forward * yVector * Time.deltaTime);

        if (onPlatform){
            this.transform.parent = platform.transform;
        }

        this.transform.Translate(Vector3.right * xVector * Time.deltaTime);

        _isJumping |= Input.GetKeyDown(KeyCode.Space);
    }
}
