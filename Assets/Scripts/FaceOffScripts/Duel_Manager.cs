using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duel_Manager : MonoBehaviour
{
    public List<GameObject> Dueling = new List<GameObject>();
    public List<Transform> visible = new List<Transform>();
    public Transform playerTrans;

    public float walkSpeed = 1f;
    private int Limiter = 0;
    public Letterbox letterBox;
    
    // Start is called before the first frame update
    void Start()
    {
        visible = FindObjectOfType<FieldOfView>().visibleTargets;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Limiter == 0){
            // FindObjectOfType<Player_Duel_Behaviour>().enabled = false;
            FindDuelists();
            letterBox.Show(150f, 0.3f);
            playerTrans.LookAt(Dueling[0].transform);
            float step = walkSpeed * Time.deltaTime;
            playerTrans.position = Vector3.MoveTowards(playerTrans.position, Dueling[0].transform.position, step);
            Limiter = 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Limiter == 1){
            // FindObjectOfType<Player_Duel_Behaviour>().enabled = true;
            letterBox.Hide(0.3f);
            Dueling.Clear();
            Limiter = 0;
        }
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
