using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Duel_Manager : MonoBehaviour
{
    public List<GameObject> Dueling = new List<GameObject>();
    public List<Transform> visible = new List<Transform>();

    public List<CinemachineVirtualCamera> duelCamera = new List<CinemachineVirtualCamera>();
    public GameObject playerTrans;
    public GameObject target;
    private float walkSpeed = 1f;
    public int Limiter = 0;
    public Letterbox letterBox;
    public float transitionTimer = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        visible = FindObjectOfType<FieldOfView>().visibleTargets;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Limiter == 0 && visible.Count >= 0){
            FindObjectOfType<Player_Duel_Behaviour>().enabled = false;
            FindDuelists();
            letterBox.Show(150f, 0.3f);
            Limiter = 1;
            CameraControl();
            // StartCoroutine("CinematicWalkTimer");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            FindObjectOfType<Player_Duel_Behaviour>().enabled = true;
            letterBox.Hide(0.3f);
            Dueling.Clear();
            Limiter = 0;
            CameraControl();
        }

        if (Limiter == 1){
            float step = walkSpeed * Time.deltaTime;
            playerTrans.transform.position = Vector3.MoveTowards(playerTrans.transform.position, Dueling[0].transform.position, step);
            playerTrans.GetComponent<Animator>().SetFloat("Blend", walkSpeed, 0.3f, Time.deltaTime);
            foreach (GameObject obj in Dueling){
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, playerTrans.transform.position, step);
                obj.GetComponent<Animator>().SetFloat("Blend", 0.3f, 0.3f, Time.deltaTime);
            }
            
            // print("Moving");
        }
    }

    void CameraControl(){
        if (Limiter == 1){
            FindObjectOfType<CinemachineFreeLook>().enabled = false;
            duelCamera[0].gameObject.SetActive(true);
        }else if (Limiter == 0){
            FindObjectOfType<CinemachineFreeLook>().enabled = true;
            duelCamera[0].gameObject.SetActive(false);
        }
    }

    // IEnumerator CameraTransition(){
    //     print("Switch");
    //     yield return new WaitForSeconds(2f);
    //     print("Switch");
    // }

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
        // PreDuel();
    }

    // public void PreDuel(){

    // }

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
