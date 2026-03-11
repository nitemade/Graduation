using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancementPanelUI : MonoBehaviour
{
    public List<EnhancementCardUI> cards;

    public void Show(List<Enhancement_SO> list)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < cards.Count; i++)
        {
            if (i < list.Count)
            {
                cards[i].gameObject.SetActive(true);


                cards[i].Init(
                    list[i],
                    OnSelectEnhancement
                );
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
        Time.timeScale = 0f;
    }

    void OnSelectEnhancement(Enhancement_SO e)
    {
        EnhancementManager.Instance.ApplyEnhancement(e);

        Close();
    }

    public void Close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
