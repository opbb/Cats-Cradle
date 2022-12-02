using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullCatGrab : MonoBehaviour
{

    [SerializeField] SpriteRenderer skullRenderer;
    [SerializeField] GameObject catNoCollar;
    [SerializeField] GameObject catCollar;
    [SerializeField] CatCharacterMovement cat;
    [SerializeField] Sprite skull;
    [SerializeField] Sprite skullCat;
    [SerializeField] Sprite skullCatCollar;

    public void CatOnSkull() {
        catNoCollar.SetActive(false);
        catCollar.SetActive(false);
        if(cat.hasCollar) {
            skullRenderer.sprite = skullCatCollar;
        } else {
            skullRenderer.sprite = skullCat;
        }
    }

    public void CatOffSkull() {
        skullRenderer.sprite = skull;
        catNoCollar.SetActive(true);
        catCollar.SetActive(true);
    }
}
