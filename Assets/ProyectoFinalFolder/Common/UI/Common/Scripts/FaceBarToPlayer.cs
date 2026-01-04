using UnityEngine;

public class FaceBarToPlayer : MonoBehaviour
{
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main == null) return;
        rectTransform.rotation = Quaternion.LookRotation(
            Camera.main.transform.forward,
            Camera.main.transform.up
        );
    }
}
