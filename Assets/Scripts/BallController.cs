using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;
    [SerializeField] float respawnDelay = 0.2f;
    [SerializeField] float detachDelay = 1f;

    Rigidbody2D ballRb2D;
    SpringJoint2D ballSpringJoint;

    private Camera mainCamera;
    private bool isDragging;

    public void Awake()
    {
        mainCamera = Camera.main;
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         if (!Touchscreen.current.press.isPressed)
        {
        return;
        }
        ayn� isi yapar basilmamissa update in devamina bakmaz
         */

        if (Touchscreen.current.press.isPressed && ballRb2D != null)
        {
            isDragging = true;

            // s�r�klerken fizik calismasin yoksa sap�t�yor
            ballRb2D.isKinematic = true;

            // Vector2CContrrol diye bi�ey d�enecekti ama .ReadValue() eklenerek normal vector2 ye cevirdik     
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);

           // Debug.Log("T�klanan yer" + worldPos);

            //Top tiklanan yere gitsin - s�r�kleme
            ballRb2D.position = worldPos;

        }
        else
        {
            if (isDragging)
            {

                isDragging = false;
                StartCoroutine(LaunchBall());
            }
            
        
        }

    }

    private IEnumerator LaunchBall()
    {
        // b�rak�nca dinamik olsun ki gitsin, hareket etsin
        ballRb2D.isKinematic = false;
        ballRb2D = null;    // top hareket ederken ekrana t�klanip y�n� degismesin

        yield return new WaitForSeconds(detachDelay);

        // top bi ileri bi geri hareket edip durmas�n
        ballSpringJoint.enabled = false;
        ballSpringJoint = null;

        Invoke(nameof(SpawnBall), respawnDelay);
    }

    private void SpawnBall()
    {
        //Quaternion.identity default rotasyon
        GameObject ballInstance = Instantiate(ballPrefab,pivot.position, Quaternion.identity);

        ballRb2D = ballInstance.GetComponent<Rigidbody2D>();
        ballSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        ballSpringJoint.connectedBody = pivot;
    }
}
