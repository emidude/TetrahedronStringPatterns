using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePair {

    public Edge e1, e2;

    int numberPairedEdgeVertices;
    int subsetNumberVertices;

    public Vector3[] verticese1e2;

    public int[] indicesConnectingVertices;

    public EdgePair(Edge e1, Edge e2, float frac) {
        this.e1 = e1;
        this.e2 = e2;

        numberPairedEdgeVertices = e1.edgeDivisions;

        subsetNumberVertices = (int)(numberPairedEdgeVertices * frac);

        verticese1e2 = new Vector3[numberPairedEdgeVertices * 2];

        //put vertices in array: [v1,v2,v3,...,w1,w2,w3,...] for v=e1 verts w =e2 verts
        for (int i = 0; i < numberPairedEdgeVertices; i++)
        {
            verticese1e2[i] = e1.edgeVertices[i];
        }
        for (int i = numberPairedEdgeVertices; i < numberPairedEdgeVertices*2; i++)
        {
            verticese1e2[i] = e2.edgeVertices[i- numberPairedEdgeVertices];
        }

        indicesConnectingVertices = new int[subsetNumberVertices * 2];

    }
    
    void setIndicesConnectingVerticesForwards()
    {
        for(int i = 0; i < subsetNumberVertices*2; i+=2)
        {
            indicesConnectingVertices[i] = i;
            indicesConnectingVertices[i + 1] = numberPairedEdgeVertices + i;
        }
    }

    public void setIndicesConnectingVertices(int direction, int startPosition) //
    {
        for(int i = 0; i < subsetNumberVertices*2; i += 2)
        {
            indicesConnectingVertices[i] = i;
            indicesConnectingVertices[i + 1] = numberPairedEdgeVertices + startPosition + i * direction;

        }
    }
    
}
