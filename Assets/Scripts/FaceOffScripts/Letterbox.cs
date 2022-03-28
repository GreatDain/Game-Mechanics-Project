using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterbox : MonoBehaviour
{
    private RectTransform topBar, bottomBar;
    private float targetSize;
    private float changeSizeAmount;
    [SerializeField]
    private bool letterboxActive;
    private void Awake() {
        GameObject gameObject = new GameObject("topBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        topBar = gameObject.GetComponent<RectTransform>();
        topBar.anchorMin = new Vector2(0, 1);
        topBar.anchorMax = new Vector2(1, 1);
        topBar.sizeDelta = new Vector2(0, 0);

        gameObject = new GameObject("bottomBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        bottomBar = gameObject.GetComponent<RectTransform>();
        bottomBar.anchorMin = new Vector2(0, 0);
        bottomBar.anchorMax = new Vector2(1, 0);
        bottomBar.sizeDelta = new Vector2(0, 0);
    }

    private void Update(){
        // if (Input.GetKeyDown(KeyCode.Space)){
        //     Show(150f, 0.3f);
        // }

        // if (Input.GetKeyDown(KeyCode.LeftShift)){
        //     Hide(0.3f);
        // }

        if(letterboxActive){
            Vector2 customSizeDelta = topBar.sizeDelta;
            customSizeDelta.y += changeSizeAmount * Time.deltaTime;
            if (changeSizeAmount > 0){
                if(customSizeDelta.y >= targetSize){
                    customSizeDelta.y = targetSize;
                    letterboxActive = false;
                }
            }else{
                if(customSizeDelta.y <= targetSize){
                    customSizeDelta.y = targetSize;
                    letterboxActive = false;
                }
            }
            topBar.sizeDelta = customSizeDelta;
            bottomBar.sizeDelta = customSizeDelta;
        }
    }

    public void Show(float targetSize, float time){
        this.targetSize = targetSize;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        letterboxActive = true;
    }

    public void Hide(float time){
        targetSize = 0f;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        letterboxActive = true;
    }
}
