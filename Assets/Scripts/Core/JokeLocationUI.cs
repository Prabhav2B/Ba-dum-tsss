using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JokeLocationUI : MonoBehaviour
{
    [SerializeField] Image locationImage;
    [SerializeField] TextMeshProUGUI locationName;

    private void Start()
    {
        LocationBlank();
    }
    public void LocationUpdate(LocationScriptableObject locationInfo)
    {
        locationImage.color = Color.white;
        locationImage.sprite = locationInfo.LocationImage;
        locationName.text = locationInfo.name;
    }
    public void LocationBlank()
    {
        locationImage.color= Color.clear;
        locationName.text = string.Empty;
    }
}
