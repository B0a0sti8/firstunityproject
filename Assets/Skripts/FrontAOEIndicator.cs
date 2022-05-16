using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAOEIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh dreieckMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = dreieckMesh;

        float fov = 90f;
        Vector3 origin = Vector3.zero;
        int rayCount = 2;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        dreieckMesh.vertices = vertices;
        dreieckMesh.uv = uv;
        dreieckMesh.triangles = triangles;


        
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
