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

    public List<Edge> SubEdges = new List<Edge>();

    public int numberOfSubEdgesForEachEdge = 2;

    GameObject cube1, cube2;

    

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
        //displayMarkerObjects();
        

        //rotate cube so cab see from top dwon
        transform.eulerAngles = new Vector3(-132.213f, 27.851f, -38.45599f);

    }


	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(new Vector3(0f, 0f, 10f) * Time.deltaTime);

            //quaternions
            //https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html

            //https://docs.unity3d.com/ScriptReference/Quaternion.html

            float angle = 50f * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
            //cube1.transform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
            //cube2.transform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);


        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(0f, 0f, 10f) * Time.deltaTime);

            //quaternions
            //https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html

            //https://docs.unity3d.com/ScriptReference/Quaternion.html

            float angle = 50f * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(angle, -Vector3.up);
            

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0f, 0f, 10f) * Time.deltaTime);

            //quaternions
            //https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html

            //https://docs.unity3d.com/ScriptReference/Quaternion.html

            float angle = 50f * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(angle, Vector3.left);
            


        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0f, 0f, 10f) * Time.deltaTime);

            //quaternions
            //https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html

            //https://docs.unity3d.com/ScriptReference/Quaternion.html

            float angle = 50f * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(angle, Vector3.right);
            

        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0f, 0f, 10f) * Time.deltaTime);

            //quaternions
            //https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html

            //https://docs.unity3d.com/ScriptReference/Quaternion.html

            float angle = 50f * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
            
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(new Vector3(0f, 0f, 10f) * Time.deltaTime);

            //quaternions
            //https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html

            //https://docs.unity3d.com/ScriptReference/Quaternion.html

            float angle = 50f * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(angle, Vector3.back);
        }

        
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
        2,3,1,3,
        0,3
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

    //void setSubEdges(int numberOfSubEdgesForEachEdge)
    //{
    //    float lerp_inc = 1 / numberOfSubEdgesForEachEdge;

    //    for (int i )
    //}

    void positionCubes()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 vertexPosFrom = Edges[i].edgeVertices[5];
            int edgePairIndex = Edges[i].pairedEdge;
            Vector3 vertexPosTo = Edges[edgePairIndex].edgeVertices[5];
            
            Color colour = new Color(0, 0, 0);
            colour[i] = 1;

            cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //Get the Renderer component from the new cube
            var cubeRenderer = cube1.GetComponent<Renderer>();
            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", colour);
            cube1.transform.position = vertexPosFrom;
            cube1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            cube1.transform.SetParent(transform);

            cube2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //Get the Renderer component from the new cube
            var cubeRenderer2 = cube2.GetComponent<Renderer>();
            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer2.material.SetColor("_Color", colour);
            cube2.transform.position = vertexPosTo;
            cube2.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            cube2.transform.SetParent(transform);

        }

    }

    void displayMarkerObjects()
    {
        Transform o1 = Instantiate(one, vertices[0], Quaternion.identity);
        Transform o2 = Instantiate(two, vertices[1], Quaternion.identity);
        Transform o3 = Instantiate(three, vertices[2], Quaternion.identity);
        Transform o4 = Instantiate(four, vertices[3], Quaternion.identity);

        o1.transform.SetParent(transform, true);
        o2.transform.SetParent(transform, false);
        o3.transform.SetParent(transform, false);
        o4.transform.SetParent(transform, false);

        //setSubEdges(2);
        positionCubes();
    }

}
