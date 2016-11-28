using UnityEngine;
using System.Collections;

public class LightingColorBehavior : MonoBehaviour
{
    public enum ColorPattern
    {
        Eradic,     // Randomly turning on and off
        Sin,		// Implements the sin wave
        Square      // Implements the square wave
    }

    [SerializeField]
    [Tooltip("The time the interpolation will start on")]
    float timeSeed;

    [SerializeField]
    [Tooltip("The pattern to play over the color")]
    ColorPattern patternToPlay = ColorPattern.Sin;

    [SerializeField]
    [Tooltip("The base color that we will interpolate over")]
    Color primaryColor;

    [SerializeField]
    [Tooltip("The color we will interpolate to")]
    Color secondaryColor;

    [SerializeField]
    [Tooltip("How quickly the pattern is playing")]
    float wavelength;

    [SerializeField]
    [Tooltip("Array of GameObjects that represent the underglow lights")]
    GameObject[] lightingObjects;

    public void SetColorPattern(ColorPattern pattern, Color color, Color secondary, float intensity)
    {
        this.primaryColor = color;
        this.secondaryColor = secondary;
        this.patternToPlay = pattern;
        this.wavelength = intensity;
        this.timeSeed = Time.time;
    }

    public void SetColorPattern(ColorPattern pattern, Color color, Color secondary, float intesity, float seed)
    {
        this.primaryColor = color;
        this.secondaryColor = secondary;
        this.patternToPlay = pattern;
        this.wavelength = intesity;
        this.timeSeed = seed;
    }

    private float nextTimeJump = 0;
    private bool lastJumpWasOn = false;

    public Color getPlayedColor()
    {

        Color currentColor = this.primaryColor;

        switch (patternToPlay)
        {

            case ColorPattern.Eradic:

                if (nextTimeJump < Time.time)
                {
                    nextTimeJump = Time.time + (this.wavelength * Random.value);
                    lastJumpWasOn = !lastJumpWasOn;
                }

                currentColor = lastJumpWasOn ? this.primaryColor : secondaryColor;

                break;

            case ColorPattern.Sin:
                float curSinVal = (1 + Mathf.Sin((this.timeSeed + Time.time) * (1 / this.wavelength))) / 2;

                float rDist = secondaryColor.r - primaryColor.r;
                float gDist = secondaryColor.g - primaryColor.g;
                float bDist = secondaryColor.b - primaryColor.b;
                float aDist = secondaryColor.a - primaryColor.a;

                currentColor = new Color(primaryColor.r + (rDist * curSinVal),
                                          primaryColor.g + (gDist * curSinVal),
                                          primaryColor.b + (bDist * curSinVal),
                                          primaryColor.a + (aDist * curSinVal));
                break;

            case ColorPattern.Square:

                if (nextTimeJump < Time.time)
                {
                    nextTimeJump = Time.time + (this.wavelength / 2);
                    lastJumpWasOn = !lastJumpWasOn;
                }

                currentColor = lastJumpWasOn ? this.primaryColor : secondaryColor;
                break;

            default:
                currentColor = this.primaryColor;
                break;
        }

        return currentColor;

    }
}
