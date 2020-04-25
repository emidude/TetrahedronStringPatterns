using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringPattern : MonoBehaviour {

    Vector3[] frameVertices;
    int[] frameIndices;

    List<Edge> frameEdges;

    Vector3[] stringVertices;
    int[] stringIndices;

    float edgeDivisions = 20;

    Mesh mesh2;

    public Transform vertexMarker;

    public Transform edgeMarer;

	void Start () {

        frameVertices = GetComponentInParent<createShape>().vertices;
        frameIndices = GetComponentInParent<createShape>().indices;

        frameEdges = GetComponentInParent<createShape>().Edges;
        //for (int i = 0; i < 6; i++)
        //{
        //    frameEdges[i].debugInfo();
        //}

        float edgeFraction = 1 / edgeDivisions;

        stringVertices = new Vector3[(int)(edgeDivisions * 6)];
        stringIndices = new int[(int)(edgeDivisions * 6)];

        for (int i = 0; i < 3; i++)
        {
            drawString(i, edgeFraction);
        }

        //for(int i = 0; i < stringVertices.Length; i++)
        //{
        //    Debug.Log("vert = " + stringVertices[i]);
        //}

        //for (int i = 0; i < stringVertices.Length; i++)
        //{
        //    Debug.Log("ind = " + stringIndices[i]);
        //}

        //for (int i = 0; i < stringVertices.Length; i++)
        //{
        //    Transform pf = Instantiate(vertexMarker, stringVertices[i], Quaternion.identity);
        //}
        for (int i = 0; i <2; i++)
        {
            Transform pf = Instantiate(vertexMarker, stringVertices[i], Quaternion.identity);
        }

        generateMesh();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void drawString(int ind, float edgeFraction)
    {

        Edge e1 = frameEdges[ind];
        Edge e2 = frameEdges[e1.pairedEdge];
        
        for (int i = 0; i < edgeDivisions; i+=2)
        {
            int sectionInArray = (int)(ind * edgeDivisions);
            //can do with split half after
            Vector3 newVertexStart = Vector3.Lerp(e1.startVertex, e1.endVertex, i * edgeFraction + edgeFraction);
            
            Vector3 newVertexEnd = Vector3.Lerp(e2.startVertex, e2.endVertex, i * edgeFraction + edgeFraction);
            
            stringVertices[i + sectionInArray] = newVertexStart;
            
            stringVertices[i + 1 + sectionInArray] = newVertexEnd;

            stringIndices[i + sectionInArray] = i;
            
            stringIndices[i + 1 + sectionInArray] = i + 1;
            
        }
    }

    void generateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh2 = new Mesh();
        mesh2.vertices = stringVertices;
        mesh2.SetIndices(new int[]{0,1,2,3,4,5,6}, MeshTopology.Lines, 0);
    }
}
