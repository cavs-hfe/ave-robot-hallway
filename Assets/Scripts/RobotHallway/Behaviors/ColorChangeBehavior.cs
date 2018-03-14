using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ColorChangeBehavior : MonoBehaviour
{
    public static Color Black = Color.black;
    public static Color Red = Color.red;
    public static Color Neutral = new Color(245.0f/255.0f, 245.0f/255.0f, 220.0f/255.0f);
    public static Color Yellow = Color.yellow;
    public static Color Green = new Color(0, 252.0f/255.0f, 0);
    public static Color Maroon = new Color(88.0f/255.0f, 0, 0);

    [Tooltip("Material of body (recommended to use Standard Shader)")]
    public Material bodyMaterial;

    [SerializeField]
    private Color bodyColor;

    //allows color to be updated in edit mode
    void Update()
    {
        bodyMaterial.color = bodyColor;
    }

    /// <summary>
    /// Set the color of the assigned material to the color provided.
    /// </summary>
    /// <param name="color">Color to change to</param>
    public void SetBodyColor(Color color)
    {
        this.bodyColor = color;
        bodyMaterial.color = bodyColor;
        //Debug.Log("Set color to " + bodyColor);
    }

    /// <summary>
    /// Get the current color of the assigned material.
    /// </summary>
    /// <returns>Currently assigned color</returns>
    public Color GetBodyColor()
    {
        return this.bodyColor;
    }
}
