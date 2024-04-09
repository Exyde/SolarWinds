using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Engine.Scenes
{
    public static class SceneNameCache
    {
        private static readonly Dictionary<int, string> SceneNames = new();

        private static string GetForBuildIndex(int buildIndex)
        {
            if (SceneNames.TryGetValue(buildIndex, value: out var name))
            {
                return name;
            }

            var path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            var sceneName = System.IO.Path.GetFileNameWithoutExtension(path);

            SceneNames[buildIndex] = sceneName;
            return sceneName;
        }

        public static string GetCurrent()
        {
            return GetForBuildIndex(SceneManager.GetActiveScene().buildIndex);
        }
    }
}