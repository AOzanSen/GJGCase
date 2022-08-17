using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateMesh : MonoBehaviour
{
    /*
    - Orjinal mesh in vertex ve triangle datas?n? aliyoruz,
    - Bir tane cloneMesh olusturuyoruz ve sadece vertex lerini veriyoruz,
    - Bunu bu objenin MeshFilter ina atiyoruz fakat cloneMesh in triangle datasi olmadigi icin bos gorunuyor,
    - Update icinde cubugumuzun en yakin (0.1F yapildi) oldugu vertexler icin
    bunlarin bagli bulundugu triangle datalarini cloneMesh in triangle datasina da aktariyoruz.
    (Vertexlerin bagli bulundugu triangle datasini almak icin asagidaki metodu buldum ve editledim.)
    - Mesh i generate ediyoruz sadece triangle datasi olanlar gorunur hale geliyor.

    */
    private Vector3[] _baseVertices; // original mesh vertices
    private int[] _baseTriangles;    // origianl mesh triangles
    private int[] _cloneTriangles;   // clone mesh triangles

    private Mesh cloneMesh;        

    [SerializeField]
    private GameObject stickObject; 

    List<List<int>> vertexToTriangleList;

    // Start is called before the first frame update
    void Start()
    {
        _baseVertices = this.GetComponent<MeshFilter>().mesh.vertices; //store base vertices
        _baseTriangles = this.GetComponent<MeshFilter>().mesh.triangles; //store base triangles

        _cloneTriangles = new int[_baseTriangles.Length]; // make empty cloneTrtiangle for generation

        MakeCloneMesh();
        
    }

    public void MakeCloneMesh()
    {
        cloneMesh = new Mesh
        {
            name = "Clone",
        };

        this.GetComponent<MeshFilter>().mesh = cloneMesh;

        cloneMesh.vertices = _baseVertices; //orjinal mesh vertices[]
        cloneMesh.triangles = _cloneTriangles; //empty mesh triangles[]

        NeighborhoodTrianglesToList();
    }

    //https://answers.unity.com/questions/1402839/looking-for-triangles-attached-to-a-vertice.html
    //this method copy from this posts comment and edited, i need to find triangle for specific vertex and search from google about it
    public void NeighborhoodTrianglesToList()
    {
        /// For each mesh vertex (index) store a list of the triangle indices 
        vertexToTriangleList =
        new List<List<int>>(_baseVertices.Length);

        /// Init list
        for (int i = 0; i < _baseVertices.Length; i++)
        {
            vertexToTriangleList.Add(new List<int>());
        }

        /// Store containing triangle indices for vertices
        int stride = 3;
        for (int i = 0; i < _baseTriangles.Length; i += stride)
        {
            vertexToTriangleList[_baseTriangles[i + 0]].Add(i + 0);
            vertexToTriangleList[_baseTriangles[i + 1]].Add(i + 1);
            vertexToTriangleList[_baseTriangles[i + 2]].Add(i + 2);
        }
        
    }

    public void ApplyWax()
    {
        for (int i = 0; i < _baseVertices.Length; i++)
        {
            //We search in all vertices for close to my stick (0.1F) hard coded for scene
            if (Vector3.Distance(transform.TransformPoint(_baseVertices[i]) , stickObject.transform.position) < 0.1F) 
            {
                //We assign triangles of this vertex to cloneTriangles
                for (int j = 0; j < vertexToTriangleList[i].Count; j++)
                {
                    _cloneTriangles[vertexToTriangleList[i][j]] = _baseTriangles[vertexToTriangleList[i][j]];
                }
            }
        }
       
        cloneMesh.triangles = _cloneTriangles;  //update newly assigned triangles
        cloneMesh.RecalculateNormals(); //generate mesh visible
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyWax();
    }
}
