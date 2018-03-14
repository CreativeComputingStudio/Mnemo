//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//

using UnityEngine;
using HoloToolkit.Sharing.SyncModel;

namespace HoloToolkit.Sharing.Spawning
{
    /// <summary>
    /// A SyncSpawnedMeshText contains string information needed for another device to spawn an object in the same location
    /// as where it was originally created on this device.
    /// </summary>
    [SyncDataClass]
    public class SyncSpawnedMeshText : SyncSpawnedObject
    {
        /// <summary>
        /// Mesh Text String for the object.
        /// </summary>
        [SyncData] public SyncString MeshTextString;

        public virtual void Initialize(string name, string parentPath, string text)
        {
            Name.Value = name;
            ParentPath.Value = parentPath;
            MeshTextString.Value = text;

            ObjectPath.Value = string.Empty;
            if (!string.IsNullOrEmpty(ParentPath.Value))
            {
                ObjectPath.Value = ParentPath.Value + "/";
            }

            ObjectPath.Value += Name.Value;
        }
    }
}
