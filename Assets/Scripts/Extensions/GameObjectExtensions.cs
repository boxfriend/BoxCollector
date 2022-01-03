using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boxfriend.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Recursively changes game object and its children to specified layer
        /// </summary>
        /// <param name="layer">Layer to change all objects to</param>
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform) 
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

        
        /// <summary>
        /// Destroys all children of the GameObject not including itself\
        /// </summary>
        public static void DestroyChildren(this GameObject parent)
        {
            var children = new Transform[parent.transform.childCount];
            for (var i = 0; i < parent.transform.childCount; i++)
                children[i] = parent.transform.GetChild(i);
            for (var i = 0; i < children.Length; i++)
                GameObject.Destroy(children[i].gameObject);
        }
    }
}
