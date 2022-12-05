using AutoSaver;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityAutoSaver
{
    public class AutoSave
    {
        private static Task autoSaveTask;

        [InitializeOnLoadMethod]
        private static void AutoSaveInitiator()
        {
            if (AutoSaveProperties.Properties.StartOnEditorLoad)
            {
                autoSaveTask = AutoSaveLoop();
                Debug.Log(autoSaveTask.Status);
            }
        }

        [MenuItem("AutoSaver/Start")]
        private static void StartAutoSaver()
        {
            if(autoSaveTask != null && autoSaveTask.Status != (TaskStatus.Canceled | TaskStatus.Faulted | TaskStatus.RanToCompletion)) {
                EditorUtility.DisplayDialog("Auto saver is runing", "Auto saver is running, you can rest easy :)","Ok");
            }
        }

        private static bool CanSave
        {
            get
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                    return false;

                if (EditorApplication.isCompiling)
                    return false;

                if (BuildPipeline.isBuildingPlayer)
                    return false;

                if (EditorApplication.isUpdating)
                    return false;

                return EditorSceneManager.GetActiveScene().isDirty;
            }
        }

        private static async Task AutoSaveLoop()
        {
            while (true)
            {
                await Task.Delay((int)(AutoSaveProperties.Properties.TimeBetweenAutoSaves * 1000));
                if (CanSave)
                    DoSave();
            }
        }

        private static void DoSave()
        {
            EditorSceneManager.SaveOpenScenes();
            
            if(AutoSaveProperties.Properties.Log)
                Debug.Log("Auto Saved!");
        }
    }
}
