using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCloud : MonoBehaviour
{
    public float speed;
    public float rainPower = 0.5f;

    private Vector3 mOffset;
    bool isDragged = false;
    bool isRaining = false;
    bool stopDragging = false;

    int timesShaken = 0, timesToBeShaken = 6;
    float timeToCheckShake = 0.5f;
    float oldMouseAxis;
    float timer, rainTimer;
    float rainTimeToDecrease = 1f;
    float rainDecreaser = 0.05f;

    public List<GameObject> cloudsInContact = new List<GameObject>();
    public int colliderCount = 0;
    bool contacted = false;

    private void OnMouseDown()
    {
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        isDragged = true;

        if (isRaining)
        {
            StopRaining();
        }
    }
    private void OnMouseUp()
    {
        isDragged = false;
        stopDragging = false;
        timesShaken = 0;
        timer = 0;
    }

    private void OnMouseDrag()
    {
        if (!stopDragging)
        {
            transform.position = new Vector3(GetMouseWorldPos().x + mOffset.x, transform.position.y);
        }
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void Update()
    {
        if (!isDragged)
        {
            if (!isRaining)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                timer = 0.25f;
            }
            else
            {
                rainTimer += Time.deltaTime;
                if (rainTimer >= rainTimeToDecrease)
                {
                    rainTimer = 0;
                    rainPower -= rainDecreaser;
                    if (rainPower <= 0)
                    {
                        StopRaining();
                        DestroyCloud();
                    }
                }
            }
        }
        else
        {
            float mouseXAxis = Input.GetAxis("Mouse X");
            if (mouseXAxis - oldMouseAxis > 1f)
            {
                Debug.Log("Shaken");
                timesShaken++;
                if (timesShaken >= timesToBeShaken)
                {
                    StartRaining();
                    timesShaken = 0;
                }
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= timeToCheckShake)
                {
                    timesShaken = 0;
                }
            }
            oldMouseAxis = mouseXAxis;
        }

        if (cloudsInContact.Count >= 2)
        {
            contacted = true;

            cloudsInContact.Add(gameObject);
            Debug.Log("Do the multiple clouds thing");
            int aux = Random.Range(0, cloudsInContact.Count);
            Debug.Log(aux);
            for (int i = cloudsInContact.Count - 1; i >= 0; i--)
            {
                if (i != aux)
                {
                    Debug.Log("asd");
                    Destroy(cloudsInContact[i].gameObject);
                }
                else
                {
                    cloudsInContact[i].GetComponent<ShakeCloud>().StartRaining();
                    cloudsInContact[i].GetComponent<Rigidbody2D>().Sleep();
                    cloudsInContact[i].GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }

    public void StartRaining()
    {
        Debug.Log("It's raining");
        WaterManager.Instance.cloudsRaining.Add(this);

        stopDragging = true;
        isRaining = true;
        isDragged = false;
    }
    public void StopRaining()
    {
        isRaining = false;
        WaterManager.Instance.cloudsRaining.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Destroyer")
        {
            DestroyCloud();
        }
        else if (collision.tag == "Cloud")
        {
            colliderCount++;
            cloudsInContact.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cloud" && !contacted)
        {
            colliderCount--;
            cloudsInContact.Remove(collision.gameObject);
        }
    }

    private void DestroyCloud()
    {
        WaterManager.Instance.cloudsAlive.Remove(this);
        Destroy(gameObject);
    }
}
