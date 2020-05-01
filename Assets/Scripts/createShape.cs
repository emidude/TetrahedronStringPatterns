using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createShape : MonoBehaviour {

    public Transform stringPattern;

    public Vector3[] vertices;
    public int[] indices;

    float root2 = Mathf.Sqrt(2);
    private Mesh mesh;

    public List<Edge> Edges = new List<Edge>();

    public Transform one, two, three, four;

    public int edgeDivisions = 20;


    // Use this for initialization
    void Start () {

        //set vertices
        vertices = new Vector3[4];
        setVertexCoords1();

        //generate mesh
        generateMesh();
        
        //set color
        GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);

        //set string pattern
        Instantiate(stringPattern, transform);

        //set edges
        setEdges();

        //find out which vertices are which so can pair opposite sides
        //Instantiate(one, vertices[0], Quaternion.identity);
        //Instantiate(two, vertices[1], Quaternion.identity);
        //Instantiate(three, vertices[2], Quaternion.identity);
        //Instantiate(four, vertices[3], Quaternion.identity);



    }


	
	// Update is called once per frame
	void Update () {
		
	}

    void setVertexCoords1()
    {
        vertices[0] = new Vector3(-1f, 0f, -1 / root2);
        vertices[1] = new Vector3(1f, 0f, -1 / root2);
        vertices[2] = new Vector3(0, -1f, 1 / root2);
        vertices[3] = new Vector3(0, 1f, 1 / root2);
    }

    void setVertexCoords2()
    {
        vertices[0] = new Vector3(1f, 1f, 1f);
        vertices[1] = new Vector3(1f,-1f,-1f);
        vertices[2] = new Vector3(-1f, 1f,-1f);
        vertices[3] = new Vector3(-1,-1, 1);
    }

    void generateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.vertices = vertices;
        //indices = new int[]{
        //0,1,1,2,2,0,
        //1,2,2,3,3,1,
        //3,1,1,0,0,3,
        //0,3,3,2,2,0 };
        indices = new int[]{
        0,1,1,2,2,0,
        2,3,3,1,
        0,3,
        };
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
    }

    void setEdges()
    {
        int numberOfEdges = 6;
        int numberOfIndices = numberOfEdges *2 ; //where indices are pairs, from vertex1 to vertex2 

        //int[] pairs = getPairs(numberOfIndices);
        int[] pairs = setPairsByHand();

        for (int i = 0; i < 6; i++)
        {
            Edge newEdge = new Edge();

            newEdge.edgeName = i;

            newEdge.pairedEdge = pairs[i];

            int startIndex = indices[i * 2];
            int endIndex = indices[i * 2 + 1];

            newEdge.startVertex = vertices[startIndex];
            newEdge.endVertex = vertices[endIndex];

            newEdge.setIntermediateVertices(edgeDivisions);

            Edges.Add(newEdge);   
        }
    }

    int[] getPairs(int numberOfIndices) { //would have to change this significantly for other shapes

        int[] pairs = new int[6];
        
        for (int i = 0; i < numberOfIndices; i += 2)
        {
            for(int j = 0; j < numberOfIndices; j+=2)
            {
                if (!(indices[i] == indices[j] ||
                    indices[i] == indices[j+1] ||
                    indices[i+1] == indices[j] ||
                    indices[i+1] == indices[j + 1]))
                {
                    pairs[i/2] = j;
                }
            }
            
        }
        return pairs;
    }

    int[] setPairsByHand()
    {
        //match up pairs of indices so that no 2 share a number
        //indices = new int[]{
        //0,1,1,2,2,0,
        //2,3,3,1,
        //0,3,
        //};
        return new int[] {3,5,4,0,2,1};

    }


}
