using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhancementCardUI : MonoBehaviour
{
    public Image icon;

    public TMP_Text titleText;
    public TMP_Text descText;
    Button button;

    private Enhancement_SO data;
    private System.Action<Enhancement_SO> onClick;

    public void Init(Enhancement_SO e, System.Action<Enhancement_SO> click)
    {
        data = e;
        onClick = click;

        icon.sprite = data.icon;
        titleText.text = data.displayName;
        descText.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        onClick?.Invoke(data);
    }
}
