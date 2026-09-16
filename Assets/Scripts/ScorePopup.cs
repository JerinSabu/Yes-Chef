using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [Header("Settings")]
    public float floatSpeed = 1.5f;
    public float fadeTime = 2.0f;

    public TextMeshProUGUI textMesh; 
    private float timer = 0f;

    public void Setup(int scoreAmount)
    {
        
        if (scoreAmount >= 0)
        {
            textMesh.text = $"+{scoreAmount}";
            textMesh.color = Color.green;
        }
        else
        {
            textMesh.text = $"{scoreAmount}";
            textMesh.color = Color.red;
        }

        
        Destroy(gameObject, fadeTime);
    }

    private void Update()
    {
        
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        
        timer += Time.deltaTime;
        if (textMesh != null)
        {
            Color c = textMesh.color;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeTime);
            textMesh.color = c;
        }
    }
}