using UnityEngine;

public class ScreenCastToMouse : MonoBehaviour
{
    [Tooltip("The physics layers to try to hit with this raycast")]
    [SerializeField] protected LayerMask hitLayer;

    [Tooltip("The maximum distance this raycast can travel")]
    [SerializeField] private float maxDistance;

    //Hold a reference to our camera
    private Camera mainCamera;


    //protected = like private, but child scripts can see it
    //virtual = lets a child script override this function with its own version
    protected virtual void Start()
    {
        mainCamera = FindFirstObjectByType<Camera>();
    }

    public RaycastHit TryToHit()
    {
        //a struct cannot be "null", se we have initialise an empty struct instead
        RaycastHit hit = new RaycastHit();

        //get the currently used camera
        Camera camera = mainCamera.GetComponent<Camera>();

        //Use half the camera width and height to determine the screen centre, and cast a ray from there
        Ray ray = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, camera.pixelHeight) * 0.5f);

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayer))
        {
            return hit;
        }

        //if we hit nothing, record the furthest point we *could* have hit
        hit.point = ray.origin + ray.direction * maxDistance;

        //then we can return the otherwise empty hit
        return hit;
    }
}
