using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {

    public Vector2 moveValue;
    public float speed;
    private int count;
    private int numPickups = 3;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI velocityText;
    private Vector3 oldpos;
    private LineRenderer lineRenderer;
    private GameObject closestObject;

    private void Start()
    {
        count = 0;
        winText.text = "";
        SetCountText();
        oldpos = transform.position;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    void OnMove ( InputValue value ) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate () {
        Vector3 movement = new Vector3 ( moveValue.x , 0.0f , moveValue.y);
        GetComponent < Rigidbody >().AddForce ( movement * speed * Time.fixedDeltaTime);
        SetStats();
        oldpos = transform.position;
        findClosest();
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "PickUp"){
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void SetCountText(){
        scoreText.text = " Score : " + count.ToString();
        if (count >= numPickups){
            winText.text = " You win ! ";
        }
    }

    private void SetStats(){
        Vector3 pos = transform.position;
        positionText.text = "Position: " + pos.ToString("0.00");
        Vector3 velocity = (pos - oldpos) / Time.deltaTime;
        velocityText.text = "Velocity: " + velocity.magnitude.ToString("0.00");
    }

    void findClosest()
    {
        GameObject[] pickup;
        pickup = GameObject.FindGameObjectsWithTag("PickUp");
        float closest = float.PositiveInfinity; ;
        float distance;


        if (pickup.Length != 0)
        {
            for (int i = 0; i < pickup.Length; i++)
            {
                if (pickup[i].activeSelf)
                {
                    distance = Vector3.Distance(pickup[i].transform.position, transform.position);
                    pickup[i].GetComponent<Renderer>().material.color = Color.white;

                    if (distance < closest)
                    {
                        closestObject = pickup[i];
                        closest = distance;
                    }
                }
            }
            closestObject.GetComponent<Renderer>().material.color = Color.blue;
            distanceText.text = "Closest Pickup: " + closest.ToString("0.00");
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, closestObject.transform.position);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }
    }
}