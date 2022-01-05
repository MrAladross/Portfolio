using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 bubbleShooterPixelPosition;

    private List<Vector3> linePositions;
    [SerializeField] private LineRenderer lr;

    [SerializeField]
    private GameObject lineRendererObject;

    private bool isBouncing = false;
    private List<LineRenderer> bounceRenderers;
    void Start()
    {
        bounceRenderers = new List<LineRenderer>();
        bubbleShooterPixelPosition = new Vector3(Screen.width / 2, 0,0);
        linePositions = new List<Vector3>();
        Vector3 startPosition = Camera.main.ScreenToWorldPoint(bubbleShooterPixelPosition);
        startPosition.z = 0;
        linePositions.Add(startPosition);
        linePositions.Add(Vector3.zero);
    }

    void Update()
    {
        isBouncing = false;
        ClearBounceRenderers();
        RotateCannon();

        //100 is arbitrary length to represent infinity
        Vector3 direction = transform.right.normalized * 100f;
        
        //left is 0,1,0
        //right is 0,-1,0
        //up is 1,0,0
        direction.z = direction.x;
        direction.x = -direction.y;
        direction.y = direction.z;
        direction.z = 0;
        
        RenderFirstLine(linePositions[0], direction);
        
    }

    void RenderBounces(Vector3 origin, Vector3 direction)
    {
        //check for infinite bounces
        if (bounceRenderers.Count > 20)
            return;
        
        GameObject nextRenderer = Instantiate(lineRendererObject, origin, quaternion.identity);
        Vector3[] positions = new Vector3[2];
        positions[0] = origin;
        positions[1] = direction;
        bounceRenderers.Add(nextRenderer.GetComponent<LineRenderer>());
        bounceRenderers[bounceRenderers.Count-1].SetPositions(positions);
        
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                positions[1] = hit.point;
                bounceRenderers[bounceRenderers.Count-1].SetPositions(positions);
                
                Vector3 reflection = direction;
                reflection.x = -reflection.x;
                RenderBounces(hit.point,reflection);
            }
        }
    }
    private void RenderFirstLine(Vector3 origin, Vector3 direction)
    {
        GameObject nextRenderer = Instantiate(lineRendererObject, origin, quaternion.identity);
        Vector3[] positions = new Vector3[2];
        positions[0] = origin;
        positions[1] = direction;
        bounceRenderers.Add(nextRenderer.GetComponent<LineRenderer>());
        lr = bounceRenderers[0];
        bounceRenderers[bounceRenderers.Count-1].SetPositions(positions);
        
        
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                lr.SetPosition(1, hit.point);
                
                Vector3 reflection = direction;
                reflection.x = -reflection.x;
                RenderBounces(hit.point,reflection);
                isBouncing = true;
            }
        }
    }
[SerializeField]
    private float minimumAngle = 10f;
    private void RotateCannon()
    {
        mousePosition = Input.mousePosition - bubbleShooterPixelPosition;
        float zRotation = Mathf.Clamp(Mathf.Acos(mousePosition.x / mousePosition.magnitude) * 180 / Mathf.PI - 90,-90+minimumAngle,90-minimumAngle);
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    private void ClearBounceRenderers()
    {
        for (int i = bounceRenderers.Count - 1; i >= 0; --i)
        {
            LineRenderer _renderer = bounceRenderers[i];
            bounceRenderers.Remove(bounceRenderers[i]);
            Destroy(_renderer.gameObject);
        }
    }
}
