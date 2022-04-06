using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class Duel_Manager : MonoBehaviour
{
    public List<GameObject> Dueling = new List<GameObject>();
    public List<Transform> visible = new List<Transform>();
    public List<Transform> duelpositions = new List<Transform>();

    public List<CinemachineVirtualCamera> duelCamera = new List<CinemachineVirtualCamera>();
    public GameObject playerTrans;
    public GameObject target;
    private float walkSpeed = 1f;
    public int Limiter = 0;
    private int transition = 0;
    public Letterbox letterBox;
    public float transitionTimer = 2f;
    private bool pause;

    public GameObject blackSquare;
    
    // Start is called before the first frame update
    void Start()
    {
        visible = FindObjectOfType<FieldOfView>().visibleTargets;
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            StartCoroutine(FadeToBlack());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            StartCoroutine(FadeBack());
        }

        if (pause == false && Input.GetKeyDown(KeyCode.P)){
            Time.timeScale = 0;
            pause = true;
        }else if (pause == true && Input.GetKeyDown(KeyCode.P)){
            Time.timeScale = 1;
            pause = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Limiter == 0 && visible.Count >= 0){
            FindObjectOfType<Player_Duel_Behaviour>().enabled = false;
            FindDuelists();
            letterBox.Show(150f, 0.3f);
            Limiter = 1;
            CameraSwitch();
            // StartCoroutine("CinematicWalkTimer");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            FindObjectOfType<Player_Duel_Behaviour>().enabled = true;
            letterBox.Hide(0.3f);
            Dueling.Clear();
            Limiter = 0;
            CameraSwitch();
        }

        if (Limiter == 1){
            float step = walkSpeed * Time.deltaTime;
            playerTrans.transform.LookAt(Dueling[0].transform);
            // playerTrans.transform.position = Vector3.MoveTowards(playerTrans.transform.position, Dueling[0].transform.position, step);
            playerTrans.GetComponent<Animator>().SetFloat("Blend", walkSpeed, 0.3f, Time.deltaTime);
            foreach (GameObject obj in Dueling){
                // obj.transform.position = Vector3.MoveTowards(obj.transform.position, playerTrans.transform.position, step);
                obj.GetComponent<Animator>().SetFloat("Blend", 0.3f, 0.3f, Time.deltaTime);
                obj.GetComponent<Animator>().SetBool("Draw", false);
                obj.GetComponent<Animator>().SetBool("Attack", false);
            }

            if (transition <= 1){
                playerTrans.transform.position = Vector3.MoveTowards(playerTrans.transform.position, Dueling[0].transform.position, step);
                foreach (GameObject obj in Dueling){
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, playerTrans.transform.position, step);
                }
            }     

            if (transition == 2){
                print(Dueling[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
                if (Dueling[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.4f && Input.GetKeyDown(KeyCode.G)){
                    playerTrans.GetComponent<Animator>().SetBool("Idle->Atk1", true);
                    Dueling[0].GetComponent<Animator>().SetBool("Death", true);
                }
            } 
            // print("Moving");
        }
        
    }

    void CameraSwitch(){
        if (Limiter == 1){
            FindObjectOfType<CinemachineFreeLook>().enabled = false;
            FindObjectOfType<CinemachineFreeLook>().m_XAxis.m_InputAxisName = null; 
            FindObjectOfType<CinemachineFreeLook>().m_YAxis.m_InputAxisName = null;
            duelCamera[0].gameObject.SetActive(true);
            StartCoroutine(CameraTransition());
        }else if (Limiter == 0){
            FindObjectOfType<CinemachineFreeLook>().enabled = true;
            FindObjectOfType<CinemachineFreeLook>().m_XAxis.m_InputAxisName = "Mouse X";
            FindObjectOfType<CinemachineFreeLook>().m_YAxis.m_InputAxisName = "Mouse Y";
            transition = 0;
            foreach (CinemachineVirtualCamera virt in duelCamera){
                virt.gameObject.SetActive(false);
            }
            // duelCamera[0].gameObject.SetActive(false);
        }
    }

    IEnumerator CameraTransition(){
        if (transition == 0){
            yield return new WaitForSeconds(3f);
            StartCoroutine(FadeToBlack());
            yield return new WaitForSeconds(0.5f);
            // yield return new WaitForSeconds(1f);
            duelCamera[0].gameObject.SetActive(false);
            for (int x = 0; x < Dueling.Count; x++){
                Dueling[x].transform.position = duelpositions[x].position;
            }
            transition = 1;
            duelCamera[1].gameObject.SetActive(true);
            // StartCoroutine(CameraTransition());
            print("Switch 1");
            // StartCoroutine(FadeBack());
        }else if (transition == 1){
            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeToBlack());
            yield return new WaitForSeconds(0.5f);
            duelCamera[1].gameObject.SetActive(false);
            for (int x = 0; x < Dueling.Count; x++){
                Dueling[x].transform.position = duelpositions[x+3].position;
                Dueling[x].GetComponent<Animator>().SetBool("Draw", true);
            }
            playerTrans.GetComponent<Animator>().SetBool("Walk->Idle", true);
            transition = 2;
            duelCamera[2].gameObject.SetActive(true);
            // StartCoroutine(CameraTransition());
            print("Switch 2");
            // StartCoroutine(FadeBack());
        }else if (transition == 2){
            yield return new WaitForSeconds(Random.Range(2f, 6f));
            Dueling[0].GetComponent<Animator>().SetBool("Attack", true);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeBack());
        // StartCoroutine(CameraTransition());
    }

    public void FindDuelists(){
        if (visible.Count < 3 && visible.Count > 0 && Dueling.Count < 3){
            for (int i = 0; i < visible.Count; i++){
                Dueling.Add(visible[i].gameObject);
            }
        }
        else if (visible.Count == 3 && Dueling.Count < 3){
            for (int i = 0; i < 3; i++){
                Dueling.Add(visible[i].gameObject);
            }
        }
        
        if (Dueling.Count < 3 && Dueling.Count > 0){
            this.transform.position = Dueling[0].transform.position;
            SphereCollider col = this.gameObject.AddComponent<SphereCollider>();
            col.isTrigger = true;
            for (float x = 0; x < 1; x += Time.deltaTime/0.5f){
                col.radius = Mathf.Lerp(0, 20, x);
            }
        }

        playerTrans.transform.LookAt(Dueling[0].transform);
    }

    IEnumerator FadeToBlack(float fadeSpeed = 3.5f){
        Color objectColour = blackSquare.GetComponent<Image>().color;
        float fadeAmount;
        while(blackSquare.GetComponent<Image>().color.a < 1){
            fadeAmount = objectColour.a + (fadeSpeed * Time.deltaTime);

            objectColour = new Color(objectColour.r, objectColour.g, objectColour.b, fadeAmount);
            blackSquare.GetComponent<Image>().color = objectColour;
            yield return null;
        }
        // yield return new WaitForSeconds(0.5f);
        // StartCoroutine(FadeBack());
    }
    
    IEnumerator FadeBack(float fadeSpeed = 3.5f){
        Color objectColour = blackSquare.GetComponent<Image>().color;
        float fadeAmount;

        while(blackSquare.GetComponent<Image>().color.a > 0){
            fadeAmount = objectColour.a - (fadeSpeed * Time.deltaTime);

            objectColour = new Color(objectColour.r, objectColour.g, objectColour.b, fadeAmount);
            blackSquare.GetComponent<Image>().color = objectColour;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CameraTransition());
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy" && Dueling.Count < 3){
            for (int i = 0; i < Dueling.Count; i++){
                print("Dueling Count: " + Dueling.Count);
                if (Dueling.Count == 1){
                    if (other.gameObject.name != Dueling[i].name && Dueling.Count < 3){
                        Dueling.Add(other.gameObject);
                    }
                }else if(Dueling.Count == 2){
                    if ((other.gameObject.name != Dueling[i].name) && (other.gameObject.name != Dueling[i+1].name) && Dueling.Count < 3){
                        Dueling.Add(other.gameObject);
                    }
                }
                else if(Dueling.Count >= 3){
                    if(GetComponent<SphereCollider>() == true){
                        Destroy(GetComponent<SphereCollider>());
                    }
                    break;
                }
            }
        }
    }


}
