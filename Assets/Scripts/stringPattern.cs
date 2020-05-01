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

        //setStringVertices();

        //drawStringVertices();        

        //setIndexPattern1();
        //setIndexPattern2();BAD

        ///////////////////////////////////////
        float fractionOfVerts = 1f;
        createEdgePairs(fractionOfVerts);

        //for (int i = 0; i < edgePairs.Count; i++)
        //{
        //    Debug.Log(edgePairs[i].e1.edgeName + " " + edgePairs[i].e2.edgeName);
        //    Debug.Log(edgePairs[i].verticese1e2.Length);
        //    for (int j = 0; j < edgePairs[i].verticese1e2.Length; j++)
        //    {
        //        Debug.Log("vertex:" + j + ": " + edgePairs[i].verticese1e2[j]);
        //    }
            
        //}
        setStringVerticesInEdgePairOrder();
        ////////////////////////////////////
        //set indices in the edge pairs according to fucntion:
        setIndicesInEdgePairs(1,0); //direction, start position 

        for(int i = 0; i < edgePairs.Count; i++)
        {
            Debug.Log("edge pair: " + i);
            Debug.Log("indices connecting vertices length = " + edgePairs[i].indicesConnectingVertices.Length);
            for(int j = 0; j < edgePairs[i].indicesConnectingVertices.Length; j++)
            {
                Debug.Log("index " + j + " = " + edgePairs[i].indicesConnectingVertices[j]);
            }
        }

        //put in string:
        setIndicesInEdgePairOrder();

        generateMesh();

        drawStringVertices(0,120);
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
            EdgePair currentEdgePair = edgePairs[i];
            int numVertsPerEdgePair = currentEdgePair.verticese1e2.Length;
            int positionInFullArray = i * numVertsPerEdgePair;
            for (int j = positionInFullArray; j< numVertsPerEdgePair + positionInFullArray; j++)
            {
                stringVertices[j] = currentEdgePair.verticese1e2[j - positionInFullArray];
            }
           //  
        }
    }

    void setIndicesInEdgePairOrder()
    {
        int numVertsPerEdgePair = edgePairs[0].verticese1e2.Length;
        Debug.Log("numVertsPerEdgePair" + numVertsPerEdgePair);
        int numIndicesPerEdgePair = edgePairs[0].indicesConnectingVertices.Length;
        Debug.Log("numIndicesPerEdgePair" + numIndicesPerEdgePair);

        stringIndices = new int[numIndicesPerEdgePair * edgePairs.Count ];

        for (int i = 0; i < edgePairs.Count; i++)
        {
            EdgePair currentEdgePair = edgePairs[i];
            //int numIndicesPerEdgePair = currentEdgePair.indicesConnectingVertices.Length;

            int positionInFullArray = i * numIndicesPerEdgePair;
            
            for (int j = 0; j < numIndicesPerEdgePair; j++)
            {
                stringIndices[j + positionInFullArray] = currentEdgePair.indicesConnectingVertices[j] + numVertsPerEdgePair*i;
                Debug.Log("positionInFullArray=" + positionInFullArray + "+ current edge pair indices count= " + j + ": gives big index " + stringIndices[j + positionInFullArray]);
            }
        }
    }

    void createEdgePairs(float fractionOfVerts)
    {
        edgePairs = new List<EdgePair>();
        List<int> donePairs = new List<int>();
        for (int i = 0 ; i < frameEdges.Count; i++)
        {
            Edge e = frameEdges[i];
            Debug.Log("created edge divisons =" + e.edgeDivisions);
            if (!alreadyDoneThePair(e, donePairs))
            {
                Edge otherE = frameEdges[e.pairedEdge];
                EdgePair ep = new EdgePair(e, otherE, fractionOfVerts); //we want half say
                Debug.Log("created edgeP divisons =" + ep.e1.edgeDivisions); 
                edgePairs.Add(ep);

            }
        }
    }

    void setIndicesInEdgePairs(int direction, int startPosition)
    {
        for(int i = 0; i < edgePairs.Count; i++)
        {
            edgePairs[i].setIndicesConnectingVertices(direction, startPosition);
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

    //hmmmm gonna be tricky unless reordered first to vertex combos
    void setIndexPatternCustom(int direction, int startPosition, float frac)
    {
        //public void setIndicesConnectingVertices(int direction, int startPosition) //
        //{
        //    for (int i = 0; i < subsetNumberVertices * 2; i += 2)
        //    {
        //        indicesConnectingVertices[i] = i;
        //        indicesConnectingVertices[i + 1] = numberPairedEdgeVertices + startPosition + i * direction;

        //    }
        //}
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

    void drawStringVertices(int from, int to )
    {
        //for (int i = 0; i < stringVertices.Length; i++)
        for (int i = from; i < to; i++)
        {
            Transform pf = Instantiate(vertexMarker, stringVertices[i], Quaternion.identity);
        }

    }

    
}
