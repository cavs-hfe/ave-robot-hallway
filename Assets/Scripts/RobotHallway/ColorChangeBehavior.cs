using UnityEngine;
using System.Collections;

public class ColorChangeBehavior : MonoBehaviour
{

    public static Color Black = Color.black;
    public static Color Red = Color.red;
    public static Color Neutral = new Color(245, 245, 220);
    public static Color Yellow = Color.yellow;
    public static Color Green = new Color(124, 252, 0);

    [Tooltip("Material of body (recommended to use Standard Shader)")]
    public Material bodyMaterial;

    [SerializeField]
    private Color bodyColor;

    void Start()
    {
        //set body color to initial color
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
