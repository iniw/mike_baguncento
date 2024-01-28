using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public List<Sprite> backgrounds;

    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = backgrounds[GameManager.Instance.day - 1];
    }
}
