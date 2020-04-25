using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {

    public int edgeDivisions;

    public Vector3 startVertex, endVertex;

    public int edgeName, pairedEdge;

    public Vector3[] edgeVertices;

    public void debugInfo() {
        Debug.Log("edge name: " + edgeName + " paired edge:" + pairedEdge);
        Debug.Log("vertices: ");
        Debug.Log("start: " + startVertex);
        Debug.Log("end: " + endVertex);
    }
	
	public void setIntermediateVertices(int edgeDivisions)
    {
        edgeVertices = new Vector3[edgeDivisions];
        float edgeFraction = (float)1 / edgeDivisions;

        for(int i = 0; i < edgeDivisions; i++)
        {
            edgeVertices[i] = Vector3.Lerp(startVertex, endVertex, i * edgeFraction + edgeFraction);
        }
    }
}
