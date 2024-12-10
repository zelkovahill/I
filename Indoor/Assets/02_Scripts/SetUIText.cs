using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetUIText : MonoBehaviour
{
    [SerializeField] private TMP_Text texdtField;
    [SerializeField] private string fixedText;

    public void OnSliderValueChanged(float numericValue)
    {
        texdtField.text = $"{fixedText} : {numericValue}";
    }
}
