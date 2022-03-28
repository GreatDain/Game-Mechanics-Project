using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.UI;
using Cinemachine;

public class Area_Detection : MonoBehaviour
{
    // [SerializeField] private ForwardRendererData rendererData = null;

    public Image listenMode;
    
    // public Material silhouette;

    private float listenModeAlpha;
    private float silhouetteAlpha;

    private float aTime;

    private float aValue;

    public SphereCollider detector;

    float sphere = 0;

    private Volume vol;

    private Vignette vg;

    public CinemachineFreeLook mainCamera;

    private float focalLength;

    // Start is called before the first frame update
    void Start()
    {
    // **** SET INITIAL VALUES FOR ALPHAS ****

        // print(listenMode.color);
        listenMode.color = new Color (0.151f, 0.149f, 0.129f, 0.0f);
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out vg);
        focalLength = mainCamera.m_Lens.FieldOfView;
        // print(listenMode.color);
        // if(TryGetFeature(out var something)){something.SetActive(true);}
        //listenMode.SetActive(false);
        // silhouette.color = new Color(191, 191, 191, 0);
        // print(listenMode.GetComponent<Image>().color.a);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftShift)){
        //     StartCoroutine(FadeIn(0.5f, 3.0f));
        // }

        // if(Input.GetKeyUp(KeyCode.LeftShift)){
        //     StartCoroutine(FadeOut(0f, 3.0f));
        // }

    //**** COROUTINE CALL WHEN INPUT IS HELD OR RELEASED ****
        if (Input.GetKeyDown(KeyCode.Space)){   
            StartCoroutine(FadeInSphere(20f, 0.3f));
            StartCoroutine(FadeIn(0.65f, 0.3f));
        }
        
        if (Input.GetKeyUp(KeyCode.Space)){
            StartCoroutine(FadeInSphere(0f, 0.3f));
            StartCoroutine(FadeOut(0.0f, 0.3f));
        }
    }

    // private bool TryGetFeature(out ScriptableRendererFeature feature){
    //     feature = rendererData.rendererFeatures.Where((f) => f.name == "EnemiesHidden").FirstOrDefault();
    //     return feature != null;
    // }

// **** FADE THE SCREEN OVERLAY AND VIGNETTE INTO THE SCREEN OVER TIME ****
    IEnumerator FadeIn(float aValue, float aTime)
    {   
        listenModeAlpha = listenMode.color.a;
        // silhouetteAlpha = silhouette.color.a;
        for (float t = 0f; t < 1; t+=Time.deltaTime / aTime){
            Color newListenColor = new Color(0.151f, 0.149f, 0.129f, Mathf.Lerp(listenModeAlpha, aValue, t));
            listenMode.color = newListenColor;
            vg.intensity.value = Mathf.Lerp(0, 0.5f, t);
            mainCamera.m_Lens.FieldOfView = Mathf.Lerp(focalLength, 30, t);
            yield return null;
            // if(Input.GetKeyUp(KeyCode.Space)){
            //     break;
            // }
        }
        listenMode.color = new Color (0.151f, 0.149f, 0.129f, 0.65f);
        vg.intensity.value = 0.5f;
    }

// **** FADE THE SCREEN OVERLAY AND VIGNETTE OUT OVER TIME ****
    IEnumerator FadeOut(float aValue, float aTime)
    {   
        listenModeAlpha = listenMode.color.a;
        // silhouetteAlpha = silhouette.color.a;
        for (float t = 0f; t < 1; t+=Time.deltaTime / aTime){
            Color newListenColor = new Color(0.151f, 0.149f, 0.129f, Mathf.Lerp(listenModeAlpha, aValue, t));
            listenMode.color = newListenColor;
            vg.intensity.value = Mathf.Lerp(0.5f, 0f, t);
            mainCamera.m_Lens.FieldOfView = Mathf.Lerp(30, focalLength, t);
            yield return null;
            // if(Input.GetKeyDown(KeyCode.Space)){
            //     break;
            // }
        }
        listenMode.color = new Color (0.151f, 0.149f, 0.129f, 0f);
        vg.intensity.value = 0;
    }

// **** INCREASE ENEMY DETECTION RADIUS TRIGGER OVER TIME ****
    IEnumerator FadeInSphere(float radius, float rTime){
        // TryGetFeature(out var something);
        sphere = detector.radius;
        for (float t = 0f; t < 1; t+=Time.deltaTime/rTime){
            detector.radius = Mathf.Lerp(sphere, radius, t);
            yield return null;
            if(Input.GetKeyUp(KeyCode.Space)){
                break;
            }
        }
    }

// **** DECREASE ENEMY DETECTION RADIUS TRIGGER OVER TIME ****
    IEnumerator FadeOutSphere(float radius, float rTime){
        sphere = detector.radius;
        for (float t = 0f; t < 1; t+=Time.deltaTime/rTime){
            detector.radius = Mathf.Lerp(sphere, radius, t);
            yield return null;
            if(Input.GetKeyDown(KeyCode.Space)){
                break;
            }
        }
    }

// **** IF AN ENEMY IS DETECTED IN TRIGGER RADIUS, SET IT'S LAYER TO BE THE ENEMY SILHOUETTE LAYER SO IT'S VISIBLE THROUGH OBSTACLES ****
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.layer = 3;
            foreach (Transform child in other.gameObject.GetComponentsInChildren<Transform>()){
                child.gameObject.layer = 3;
            }
        }
    }

// **** IF AN ENEMY LEAVES THE TRIGGER RADIUS, SET IT'S LAYER BACK TO DEFAULT SO SILHOUETTE IS NOT VISIBLE ****
    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.layer = 0;
            foreach (Transform child in other.gameObject.GetComponentsInChildren<Transform>()){
                child.gameObject.layer = 0;
            }
        }
    }
}
