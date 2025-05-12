using UnityEngine;

public interface I_Touchable 
{
    public void OnTouchBegin(Vector2 touchPosition);
    public void OnTouchStay(Vector2 touchPosition);
    public void OnTouchEnd(Vector2 touchPosition);
}
