using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipMenuButton : MonoBehaviour, IPointerEnterHandler
{
    public GameManager gameManager;

    void Update()
    {
        float delta = Time.unscaledDeltaTime;

        if (Input.GetKey(KeyCode.Escape)) Close();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.OnBtn);
    }

    public void Close()
    {
        Time.timeScale = 1f;
        gameManager.CloseShipMenu();
    }
}
