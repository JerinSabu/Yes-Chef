using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [Header("Settings")]
    public float floatSpeed = 1.5f;
    public float fadeTime = 2.0f;

    public TextMeshProUGUI textMesh; // Drag the text component here
    private float timer = 0f;

    public void Setup(int scoreAmount)
    {
        // Add a "+" sign for positive numbers
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

        // Automatically destroy this object after the fadeTime
        Destroy(gameObject, fadeTime);
    }

    private void Update()
    {
        // Float upwards
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade out alpha
        timer += Time.deltaTime;
        if (textMesh != null)
        {
            Color c = textMesh.color;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeTime);
            textMesh.color = c;
        }
    }
}