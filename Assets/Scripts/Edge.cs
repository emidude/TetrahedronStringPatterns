using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {

    public Vector3 startVertex, endVertex;

    public int edgeName, pairedEdge;

    public void debugInfo() {
        Debug.Log("edge name: " + edgeName + " paired edge:" + pairedEdge);
        Debug.Log(" ");
        Debug.Log("vertices: ");
        Debug.Log("start: " + startVertex);
        Debug.Log("end: " + endVertex);
        Debug.Log(" ");
        Debug.Log(" ");


    }
	// Use this for initialization
	
}
