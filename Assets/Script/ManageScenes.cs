using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageScenes : MonoBehaviour
{
    public void OpenScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty.");
            return;
        }

        // Kiểm tra xem Scene có tồn tại trong Build Settings không
        if (!SceneExists(sceneName))
        {
            Debug.LogError("Scene " + sceneName + " does not exist in Build Settings.");
            return;
        }

        // Đảm bảo xóa sạch mọi dữ liệu trước khi Load Scene mới
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}

