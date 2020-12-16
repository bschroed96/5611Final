
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    RaycastHit hit;
    List<UnitController> selectedUnits = new List<UnitController>();
    bool isDragging = false;
    Vector3 mousePositon;
    bool door1Open = false;
    bool door2Open = false;
    public GameObject EndUI;


    private void OnGUI()
    {
        if(isDragging)
        {
            var rect = ScreenHelper.GetScreenRect(mousePositon, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
            ScreenHelper.DrawScreenRectBorder(rect, 1, Color.blue);
        }
        
        
    }

    // Update is called once per frame
    void Update () {
		GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        Debug.Log(playerUnits.Length);
        if (playerUnits.Length <= 5) {RTSGameManager.EndGame(EndUI);}
        //Detect if mouse is down
        if(Input.GetMouseButtonDown(0))
        {
            mousePositon = Input.mousePosition;
            //Create a ray from the camera to our space
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Shoot that ray and get the hit data
            if(Physics.Raycast(camRay, out hit))
            {
                //Do something with that data 
                //Debug.Log(hit.transform.tag);
                if (hit.transform.CompareTag("PlayerUnit"))
                {
                    SelectUnit(hit.transform.GetComponent<UnitController>(), Input.GetKey(KeyCode.LeftShift));
                }
                else
                {
                    isDragging = true;
                }
                // When the activator cubes are clicked, the corresponding doors are opened
                if (hit.transform.CompareTag("Activator")) {
                    GameObject act;
                    GameObject wall1;
                    act = GameObject.Find("Activator");
                    if (act != null) {
                        act.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    if (act != null) {
                        wall1 = GameObject.Find("OpenableDoor1");
                        Vector3 pos = wall1.transform.position;
                        if (!door1Open) {
                            pos.x += 15;
                            wall1.transform.position = pos;
                            wall1.GetComponent<Renderer>().material.color = Color.blue;
                            door1Open = true;
                        }
                    }
                }
                if (hit.transform.CompareTag("Activator2")) {
                    GameObject act2;
                    GameObject wall2;
                    act2 = GameObject.Find("Activator2");
                    if (act2 != null) {
                        act2.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    if (act2 != null) {
                        wall2 = GameObject.Find("OpenableDoor2");
                        Vector3 pos = wall2.transform.position;
                        if (!door2Open) {
                            pos.x -= 15;
                            wall2.transform.position = pos;
                            wall2.GetComponent<Renderer>().material.color = Color.blue;
                            door2Open = true;
                        }
                    } 
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                DeselectUnits();
                foreach (var selectableObject in FindObjectsOfType<PlayerUnitController>()) // changed from BoxCollider
                {
                    if (IsWithinSelectionBounds(selectableObject.transform))
                    {
                        SelectUnit(selectableObject.gameObject.GetComponent<UnitController>(), true);
                    }
                }

                isDragging = false;
            }
            
        }
        // on right click, move units
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0) {
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(camRay, out hit)) {
                if (hit.transform.CompareTag("Ground"))
                {
                    foreach (var unit in selectedUnits)
                    {
                        if (unit != null) {
                            unit.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 10;
                            unit.MoveUnit(hit.point);
                        }
                    }
                }
                else if (hit.transform.CompareTag("EnemyUnit")) {
                    foreach (var unit in selectedUnits)
                    {
                        if (unit != null) {
                            unit.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 25;
                            unit.SetNewTarget(hit.transform);
                        }
                    }
                }
                else if (hit.transform.CompareTag("PowerUp")) {
                    foreach (var unit in selectedUnits)
                    {
                        if (unit != null) {
                            unit.MoveUnit(hit.point);
                            var distance = (unit.transform.position - hit.transform.position).magnitude;
                            // Debug.Log(distance);
                            if (distance <= 15) {
                                // unit.gameObject.GetComponent<UnitController>().IncreaseDamage(5);
                                RTSGameManager.UnitGetPowerUp(unit.gameObject.GetComponent<UnitController>(),
                                hit.transform.gameObject);
                            }
                            unit.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 15;
                        }
                    }
                }
            }
        }

    }

    private void SelectUnit(UnitController unit, bool isMultiSelect = false)
    {
        if(!isMultiSelect)
        {
            DeselectUnits();
        }
        // Add a unit controller for selected object to selectedUnits list
        selectedUnits.Add(unit);
        if (unit != null) {
            unit.SetSelected(true);
        }
    }

    private void DeselectUnits()
    {
        for(int i = 0; i < selectedUnits.Count; i++)
        {
            // selectedUnits[i].Find("Highlight").gameObject.SetActive(false);
            if (selectedUnits[i] != null) {
                selectedUnits[i].SetSelected(false);
            }
        }
        selectedUnits.Clear();
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if(!isDragging)
        {
            return false;
        }

        var camera = Camera.main;
        var viewportBounds = ScreenHelper.GetViewportBounds(camera, mousePositon, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }
}
