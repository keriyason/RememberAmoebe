using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteMeshCollider : MonoBehaviour
{
    private void Awake()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite; //the sprite on this object
        Vector2[] spriteVerts = sprite.vertices; //outline points of the sprite
        ushort[] spriteTris = sprite.triangles; //how those points connect into triangles

        Vector3[] vertices = new Vector3[spriteVerts.Length];
        for (int i = 0; i < spriteVerts.Length; i++)
            vertices[i] = new Vector3(spriteVerts[i].x, spriteVerts[i].y, 0f); //2D points become 3D

        int[] triangles = new int[spriteTris.Length * 2];
        for (int i = 0; i < spriteTris.Length; i += 3)
        {
            triangles[i] = spriteTris[i]; //front side
            triangles[i + 1] = spriteTris[i + 1];
            triangles[i + 2] = spriteTris[i + 2];

            triangles[spriteTris.Length + i] = spriteTris[i + 2]; //back side so you can't fall through
            triangles[spriteTris.Length + i + 1] = spriteTris[i + 1];
            triangles[spriteTris.Length + i + 2] = spriteTris[i];
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null) meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh; //collider now matches the sprite shape
    }
}