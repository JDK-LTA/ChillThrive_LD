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

    int timesShaken = 0, timesToBeShaken = 6;
    float timeToCheckShake = 0.5f;
    float oldMouseAxis;
    float timer;
    //float timer = 0;
    private void OnMouseDown()
    {
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        isDragged = true;
    }
    private void OnMouseUp()
    {
        isDragged = false;
        timesShaken = 0;
        timer = 0;
    }

    private void OnMouseDrag()
    {
        transform.position = new Vector3(GetMouseWorldPos().x + mOffset.x, transform.position.y);
        //timer = 0;
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
                
            }
        }
        else
        {
            float mouseXAxis = Input.GetAxis("Mouse X");
            if (/*Mathf.Sign(mouseXAxis) != Mathf.Sign(oldMouseAxis) &&*/ mouseXAxis - oldMouseAxis > 1f)
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
        WaterManager.Instance.AddCloudToList(this, rainPower);
    }
}
