using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Player player = null;
    public bool isPlayerButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isPlayerButton) player.LightSprite();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.OnBtn);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPlayerButton) player.ReturnSprite();
    }
}
