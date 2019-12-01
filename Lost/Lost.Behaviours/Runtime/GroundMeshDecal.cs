﻿//-----------------------------------------------------------------------
// <copyright file="GroundMeshDecal.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
        #pragma warning disable 0649
        #pragma warning restore 0649

        // Variables used to determine if we need to update the mesh
        private Matrix4x4 oldMatrix;

                // Resetting the y to the position so we can re-project
                worldSpaceVert = worldSpaceVert.SetY(this.transform.position.y);
            // Making sure the mesh is created and set to the sharedMesh
            if (this.meshFilter.sharedMesh == null)

                // This makes sure they're never saved out and don't dirty up the scene every time the scene loads
                this.meshFilter.sharedMesh.hideFlags = HideFlags.HideAndDontSave;
                // No need to update the mesh
                return;

            // If we got here then we need to recalculate all our meshes verts/tris/uvs/normals
            mesh.Clear();

            // 3 ------- 2
            //   |   / |
            //   |  /  |
            //   | /   |
            //   |/    |
            // 0 ------- 1

            Vector3[] vertices = new Vector3[vertCount];

            // Initializing the verts, uvs and normals
            for (int i = 0; i < vertCount; i++)

            // Making sure the mesh is centered about the origin
            for (int i = 0; i < vertCount; i++)

            // Setting up the triangles
            int triIndex = 0;
            // If it moved then update the verticies
            if (this.transform.localToWorldMatrix != oldMatrix || this.oldGroundOffset != this.groundOffset)

            // Checking to see if the quad count changed and we must update the mesh
            if (this.quadCount != this.oldQuadCount)