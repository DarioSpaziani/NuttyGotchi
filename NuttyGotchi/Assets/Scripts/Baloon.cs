using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class Baloon : MonoBehaviour {
    [FormerlySerializedAs("Nutty")] public GameObject nutty;

    [FormerlySerializedAs("Icon")] public SpriteRenderer icon;

    public Sprite boring;
    public Sprite hungry;
    public Sprite tired;

    private void MoveToNuttyPosition() {
        Vector3 newPos = nutty.transform.position;
        newPos.x -= 0.3f;
        newPos.y += 1f;
        newPos.z -= 0.1f;

        transform.position = newPos;
    }

    public void Show(NuttyManager.Status status) {
        Sprite statusSprite = null;

        switch (status) {
            case NuttyManager.Status.Boring:
                statusSprite = boring;
                break;
            case NuttyManager.Status.Hungry:
                statusSprite = hungry;
                break;
            case NuttyManager.Status.Tired:
                statusSprite = tired;
                break;
            default:
                //do nothing
                break;
            
               
        }

        if (status == NuttyManager.Status.Boring || status == NuttyManager.Status.Hungry || status == NuttyManager.Status.Tired) {
            icon.sprite = statusSprite;
            MoveToNuttyPosition();
            gameObject.SetActive(true); 
        }
    }

    public void Hide() {
        icon.sprite = null;
        
        gameObject.SetActive(false);
    }
}
