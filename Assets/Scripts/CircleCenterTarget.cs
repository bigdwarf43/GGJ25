using UnityEngine;

public class CircleCenterTarget : MonoBehaviour
{

    public float circleRadius = 3;

    public LineRenderer circleRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DrawCircle(200, circleRadius);
    }

    void DrawCircle(int steps, float radius)
    {

        circleRenderer.positionCount = steps;


        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;


            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPos = new Vector3(x, y, 0);

            circleRenderer.SetPosition(currentStep, currentPos);
        }

    }
}
