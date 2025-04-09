using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class PairsManage : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private int matchId;
    private bool isDragging;
    private Vector3 endPoint;
    private MatchForm objMatch;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null && hit.collider.gameObject == gameObject )
            {
                isDragging = true;
                Vector3 mousrPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousrPosition.z = 0f;
                lineRenderer.SetPosition(0,mousrPosition);
            }
        }
        if (isDragging)
        {
            Vector3 mousrPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousrPosition.z = 0f;
            lineRenderer.SetPosition(1, mousrPosition);
            endPoint = mousrPosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
            if(hit.collider != null && hit.collider.TryGetComponent(out objMatch) && matchId == objMatch.Get_Id())       
            {
                Debug.Log("Correct");
                Gamemenager.Instance.RegisterCorrectMatch();
                this.enabled = false;
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
            lineRenderer.positionCount = 2;
        }

    }
}
