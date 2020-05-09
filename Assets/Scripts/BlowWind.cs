using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlowWind : MonoBehaviour
{
    bool alreadyCalculated = false;
    bool mouseOver = false;
    bool toRight = false;

    Vector2 lastMousePos;
    float diffTime = 0;
    public float maxPower;

    SpriteRenderer buttonBG;

    private void Start()
    {
        buttonBG = GetComponent<SpriteRenderer>();
    }

    private void OnMouseUp()
    {
        if (!alreadyCalculated && !mouseOver)
        {
            CalculateAndSend();
            alreadyCalculated = true;            
        }

        if (!Input.GetMouseButton(0))
        {
            alreadyCalculated = false;
            toRight = false;
            diffTime = 0;
            buttonBG.color = new Color(1, 1, 1, 0);
        }
    }

    private void OnMouseDrag()
    {
        if (alreadyCalculated) return;

        diffTime += Time.deltaTime;

        if (Input.GetAxis("Mouse X") > 0f)
        {
            toRight = true;
        }
        else
        {
            if (toRight) OnMouseUp();
            toRight = false;
            lastMousePos = Input.mousePosition;
            diffTime = 0;
        }
    }

    private void OnMouseDown()
    {
        buttonBG.color = new Color(1, 1, 1, 0.5f);
    }

    private void CalculateAndSend()
    {
        Vector2 mousePos = Input.mousePosition;

        float xDelta = Mathf.Abs(lastMousePos.x - mousePos.x);
        float mouseSpeed = xDelta / diffTime / 100;
        SendWind(mouseSpeed);

        diffTime = 0;
    }

    void SendWind(float power)
    {
        if (power > maxPower)
        {
            power = maxPower;
        }
        WindManager.Instance.airFactor += (power * 0.7f) / maxPower;
        if (WindManager.Instance.airFactor > 1)
        {
            WindManager.Instance.airFactor = 1;
        }
    }
    private void OnMouseOver()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
