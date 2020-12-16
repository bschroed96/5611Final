using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    bool forward = true;
    bool right = false;
    bool left = false;
    bool back = false;
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        // Allow for panning using cursor position outside/against window
        // Forward 
        if (Input.mousePosition.y >= Screen.height - panBorderThickness && forward)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - panBorderThickness && back)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - panBorderThickness && left)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - panBorderThickness && right)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        // Back
        if (Input.mousePosition.y <= panBorderThickness && forward)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness && back)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness && left)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness && right)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Right
        if (Input.mousePosition.x >= Screen.width - panBorderThickness && forward)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness && back)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness && left)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness && right)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
                
        // Left
        if (Input.mousePosition.x <= panBorderThickness && forward)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness && back)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness && left)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness && right)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("q"))
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("e"))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp("d")) {
            back = false;
            forward = false;
            left = false;
            right = true;
            float yRotation = Camera.main.transform.eulerAngles.y;
            yRotation = 90f;
            transform.eulerAngles = new Vector3( transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }
        if (Input.GetKeyUp("a")) {
            back = false;
            forward = false;
            left = true;
            right = false;
            float yRotation = Camera.main.transform.eulerAngles.y;
            yRotation = -90f;
            transform.eulerAngles = new Vector3( transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }
        if (Input.GetKeyUp("w")) {
            back = false;
            forward = true;
            left = false;
            right = false;
            float yRotation = Camera.main.transform.eulerAngles.y;
            yRotation = 0f;
            transform.eulerAngles = new Vector3( transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }        
        if (Input.GetKeyUp("s")) {
            back = true;
            forward = false;
            left = false;
            right = false;
            float yRotation = Camera.main.transform.eulerAngles.y;
            yRotation = 180f;
            transform.eulerAngles = new Vector3( transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }        
        
        
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
        
        transform.position = pos;
    }
}
