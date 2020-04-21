using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideThis : MonoBehaviour
{
    private void OnMouseExit()
    {
        gameObject.SetActive(false);
    }
}
