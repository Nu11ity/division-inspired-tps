using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoWidget : MonoBehaviour
{
    public TMP_Text ammoText;

    public void Refresh(int ammoCount)
    {
        ammoText.text = ammoCount.ToString();
    }
}
