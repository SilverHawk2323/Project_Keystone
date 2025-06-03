using UnityEngine;

public class CardBase : MonoBehaviour, I_Touchable
{
    public int cost;
    public UnitBase unit;
    public GameManager gm;
    protected static Camera mainCamera;
    private float distanceFromCamera;

    public LayerMask mask;

    [SerializeField] private ScriptableCardBase card;
    public ScriptableCardBase cardProperty
    {
        get
        {
            return card;
        }
        set
        {
            card = value;
            cost = card.cost;
            unit = card.unit;
        }
    }

    protected void Awake()
    {
        mainCamera = Camera.main;
        cardProperty = card;
        gm = FindFirstObjectByType<GameManager>();
    }

    public void OnTouchBegin(Vector2 touchPosition, float distance)
    {
        if (gm.state == GameState.Pause)
        {
            return;
        }
        Debug.Log("You clicked on a card");
        distanceFromCamera = distance;
    }

    public void OnTouchEnd(Vector2 touchPosition)
    {
        if(gm.state == GameState.Pause)
        {
            return;
        }
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        gameObject.GetComponent<Collider>().enabled = false;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, mask))
        {
            if (hit.collider != null)
            {
                Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, hit.transform.position.z);
                Vector3 newPosition = mainCamera.ScreenToWorldPoint(screenCoordinates);
                gm.playerManager.RemoveSpawnedCard(this);
                if (gm.state == GameState.Battle)
                {
                    UseAbility();
                }
                else if (gm.state == GameState.Deploy)
                {
                    SpawnUnit(hit.point);
                }
                else
                {
                    print("You can't do anything yet");
                }
            }

        }

    }

    public void OnTouchStay(Vector2 touchPosition)
    {
        if (gm.state == GameState.Pause)
        {
            return;
        }
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, distanceFromCamera)); //mainCamera.nearClipPlane));
        //print(worldPoint);
        worldPoint.y = transform.position.y;
        transform.position = worldPoint;

    }

    public void SpawnUnit(Vector3 spawnPosition)
    {
        Destroy(gameObject);
        var unit = Instantiate(card.unit, new Vector3(spawnPosition.x, spawnPosition.y + 0.5f, spawnPosition.z), Quaternion.identity);

    }

    public virtual void UseAbility()
    {
        print("Use Ability");
    }
}
