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
                        Destroy(this.gameObject);
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
    }

    public void StartRaining()
    {
        Debug.Log("It's raining");
        WaterManager.Instance.AddCloudToList(this);

        stopDragging = true;
        isRaining = true;
    }
    public void StopRaining()
    {
        isRaining = false;
        WaterManager.Instance.DeleteCloudFromList(this);
    }
}
