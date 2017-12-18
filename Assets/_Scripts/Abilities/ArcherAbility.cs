using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAbility : Ability
{
    private Vector3 startPosition;
    public LineRenderer lineRender;
    //public float velocity;
    //public float angle;
    public int resolution;
    //public float maxRange = 30f;
    private float gravity;
    public float maxVelocity = 10f;

    public ArcherAbility(LineRenderer render)
    {
        lineRender = render;
        //gravity = Mathf.Abs(Physics2D.gravity.y);
        resolution = 20;
        //velocity = 20;
        gravity = Mathf.Abs(Physics2D.gravity.y);
        //velocity = Mathf.Sqrt(maxRange * gravity / Mathf.Sin(2 * 45f * Mathf.Deg2Rad));

        //Debug.Log("velocity: " + velocity.ToString());

    }



    public override void startAbility()
    {
        active = true;
        startPosition = Vector3.zero;
    }

    public override void update()
    {
        if(Input.GetMouseButton(0))
        {
            if(startPosition == Vector3.zero)
            {
                startPosition = Input.mousePosition;
            }

            drawArc();

        }
        else if(startPosition != Vector3.zero)
        {
            //drawArc();
            fireAbility();
            this.finishAbility();
        }
        else
        {
            //Debug.Log(startPosition.ToString());
        }
    }

    private void drawArc()
    {
        Debug.Log(string.Format("Drawing Angle! start: {0}", startPosition));

        lineRender.SetVertexCount(resolution + 1);
        lineRender.SetPositions(calculateArcArray());


    }

    private Vector3[] calculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        Vector3 currentPosition = Input.mousePosition;
        //float angle = Vector3.Angle(new Vector3(0, 0), new Vector3(1, 1));

        //Vector3 targetDir = new Vector3(0, 0) - new Vector3(1,1);
        //float angle = Vector3.Angle(targetDir, Vector3.forward);

        //Vector3 p1 = new Vector3(0, 0);
        //Vector3 p2 = new Vector3(1, 1);

        Vector3 p1 = Input.mousePosition;
        Vector3 p2 = startPosition;

        float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;

        float velocity = Mathf.Abs(Vector3.Distance(p1, p2))/10f;

        Debug.Log("velocity: " + velocity.ToString());


        Debug.Log("angle: " + angle.ToString());

        //float angle = 45f;
        float radianArray = angle * Mathf.Deg2Rad;
        
        float maxDistance = (Mathf.Pow(velocity, 2) * Mathf.Sin(2 * radianArray)) / gravity;

        Debug.Log("Starting calculation!");
        for(int i =0; i < resolution + 1; ++i)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = calculateArcPoint(t, maxDistance, radianArray, gravity, velocity);
            //Debug.Log(arcArray[i]);
        }

        return arcArray;
    }

    private Vector3 calculateArcPoint(float t, float maxDistance, float radianAngle, float gravity, float velocity)
    {
        float startY = 0;
        float startX = -10;
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * Mathf.Pow(velocity, 2) * Mathf.Pow(Mathf.Cos(radianAngle),2) ));

        return new Vector3(x + startX, y+startY);
    }



    private void fireAbility()
    {
        Debug.Log("Firing!");
    }
}
