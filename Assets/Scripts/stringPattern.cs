using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringPattern : MonoBehaviour {

    List<Edge> frameEdges;

    Vector3[] stringVertices;
    int[] stringIndices;

    public float edgeDivisions = 20;

    Mesh mesh2;

    public Transform vertexMarker;

    List<EdgePair> edgePairs;
    int numberOfEdgePairs = 3;

	void Start () {

        frameEdges = GetComponentInParent<createShape>().Edges;
        
        stringVertices = new Vector3[(int)(edgeDivisions * 6)];
        //stringIndices = new int[(int)(edgeDivisions * frameEdges.Count)];

        setStringVertices();

        //drawStringVertices();        

        //setIndexPattern1();
        //setIndexPattern2();BAD

        ///////////////////////////////////////
        createEdgePairs();
        for (int i = 0; i < edgePairs.Count; i++)
        {
            Debug.Log(edgePairs[i].e1.edgeName + " " + edgePairs[i].e2.edgeName);
        }
        setStringVerticesInEdgePairOrder();
        ////////////////////////////////////

        generateMesh();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void setStringVertices()
    {
        int totalNumberOfStringVertices = (int)edgeDivisions * frameEdges.Count;
        
        int p = 0;

        List<int> donePairs = new List<int>();

        for (int edge = 0; edge < frameEdges.Count; edge++)
        {
            Edge e1 = frameEdges[edge];
            Edge e2 = frameEdges[e1.pairedEdge];

            if (!alreadyDoneThePair(e1, donePairs)) //MAYBE SHOULD ACTUALLY HAVE AN EDGEPAIR CLASS
            {
                for (int vert = 0; vert < e1.edgeVertices.Length; vert++)
                {
                    stringVertices[p] = e1.edgeVertices[vert];
                    stringVertices[p + 1] = e2.edgeVertices[vert];
                    p += 2;
                }
            }

        }
    }

    void setStringVerticesInEdgePairOrder()
    {
        for (int i = 0; i < edgePairs.Count; i++)
        {
           // drawStringVertices[i] = 
        }
    }

    void createEdgePairs()
    {
        edgePairs = new List<EdgePair>();
        List<int> donePairs = new List<int>();
        for (int i = 0 ; i < frameEdges.Count; i++)
        {
            Edge e = frameEdges[i];
            if (!alreadyDoneThePair(e, donePairs))
            {
                Edge otherE = frameEdges[e.pairedEdge];
                EdgePair ep = new EdgePair(e, otherE, 0.5f); //we want half say
                edgePairs.Add(ep);

            }
        }
    }

    bool alreadyDoneThePair(Edge newEdge, List<int> donePairs)
    {
        for(int i =0; i < donePairs.Count; i++)
        {
            if (newEdge.edgeName == donePairs[i])
            {
                return true;
            }
            else if(newEdge.pairedEdge == donePairs[i])
            {
                return true;
            }
        }
        donePairs.Add(newEdge.edgeName);
        donePairs.Add(newEdge.pairedEdge);
        return false;
    }

    void setIndexPattern1()
    {
        stringIndices = new int[(int)(edgeDivisions * frameEdges.Count)];
        for (int i = 0; i < stringIndices.Length; i++)
        {
            stringIndices[i] = i;
        }
    }

    void setIndexPattern2()
    {
        if ( (edgeDivisions * frameEdges.Count)%2 != 0)
        {
            Debug.Log("WATCH OUT - CHECK HERE, NOT EVEN NUMBER OF VERTS SO CROSS OVER OR TAKE ONE OFF");
        } 
        stringIndices = new int[(int)(edgeDivisions * frameEdges.Count)/2];

        for(int j = 0; j < frameEdges.Count; j++)
        {
            for (int i = 0; i < edgeDivisions / 2; i++)
            {
                stringIndices[i + j*(int)edgeDivisions/2] = i + j * (int)edgeDivisions;
            }
        }

        for(int i = 0; i < stringIndices.Length; i++)
        {
            Debug.Log(stringIndices[i]);
        }
        
    }

    void generateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh2 = new Mesh();
        mesh2.vertices = stringVertices;
        mesh2.SetIndices(stringIndices, MeshTopology.Lines, 0);

    }

    void drawStringVertices()
    {
        for (int i = 0; i < stringVertices.Length; i++)
        {
            Transform pf = Instantiate(vertexMarker, stringVertices[i], Quaternion.identity);
        }

    }
}
