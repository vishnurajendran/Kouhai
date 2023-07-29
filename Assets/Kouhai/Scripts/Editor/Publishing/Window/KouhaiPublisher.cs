using System;
using Kouhai.Publishing;
using Kouhai.Scripts.Editor.Publishing.BuildPipeline;
using UnityEditor;
using UnityEngine;

namespace Kouhai.Editor.Publishing
{
    public class KouhaiPublisher : EditorWindow
    {
        private static KouhaiPublisher windowRef;
        private Texture iconLogo;
        private KouhaiPublishingData publishData;
        private string tagText;

        private KouhaiBuildPipeline pipeline;
        
        private Texture KouhaiIcon
        {
            get
            {
                if (iconLogo == null)
                    iconLogo = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kouhai/Images/iCO.png").texture;
                return iconLogo;
            }
        }
        
        [MenuItem("Kouhai/Publish")]
        private static void OpenKouhaiPublisher()
        {
            windowRef = EditorWindow.GetWindow<KouhaiPublisher>();
            windowRef.titleContent = new GUIContent()
            {
                text = "Kouhai Project Publisher"
            };
            windowRef.minSize = windowRef.maxSize = new Vector2(400, 500);
            windowRef.ShowUtility();
        }

        private void OnEnable()
        {
            if (pipeline == null)
                pipeline = new KouhaiBuildPipeline();
            
            if (publishData == null)
                publishData = new KouhaiPublishingData();

            EditorApplication.update -= OnWindowUpdate;
            EditorApplication.update += OnWindowUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnWindowUpdate;
        }

        private void OnWindowUpdate()
        {
            Repaint();
        }
        
        private void OnGUI()
        {
            DrawLogo();
            DrawBasicForm();
            DrawAssetPacking();
            DrawButtons();
        }
    
        private void DrawLogo()
        {
            var aspect = KouhaiIcon.width / KouhaiIcon.height;
            var width = this.position.width - 50;
            var height = width / aspect;
    
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box(KouhaiIcon,GUILayout.Width(width),
                GUILayout.Height(height));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        
        private void DrawBasicForm()
        {
            EditorGUILayout.BeginVertical("groupbox");
            EditorGUILayout.LabelField("Basic Info", EditorStyles.whiteLargeLabel);
            publishData.ProjectName = EditorGUILayout.TextField("Project Name", publishData.ProjectName);
            EditorGUILayout.LabelField("Project Description");
            publishData.ProjectDescription = EditorGUILayout.TextArea(publishData.ProjectDescription, GUILayout.Height(70));
            publishData.Version = EditorGUILayout.TextField("Version", publishData.Version);
            publishData.Developer = EditorGUILayout.TextField("Developer", publishData.Developer);
            tagText = EditorGUILayout.TextField("Tags", tagText);
            if (!string.IsNullOrEmpty(tagText))
                publishData.Tags = tagText.Split(",");
            EditorGUILayout.LabelField("use ',' to seperate tags, ie; \"scifi,mystery\"");
            EditorGUILayout.EndVertical();
        }

        private void DrawAssetPacking()
        {
            EditorGUILayout.Space(7);
            EditorGUILayout.BeginVertical("groupbox");
            EditorGUILayout.LabelField("Build Info", EditorStyles.whiteLargeLabel);
            publishData.DevelopementMode = EditorGUILayout.Toggle("Develpment build", publishData.DevelopementMode);
            if (publishData.DevelopementMode)
            {
                publishData.PackImages = EditorGUILayout.Toggle("Pack Images", publishData.PackImages);
                publishData.PackAudio = EditorGUILayout.Toggle("Pack Audio", publishData.PackAudio);
            }
            else
            {
                publishData.PackImages = publishData.PackAudio = true;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawButtons()
        {
            if (pipeline.IsBuilding)
            {
                GUI.enabled = false;
                return;
            }
            
            var col = GUI.color;
            if (publishData.DevelopementMode)
                GUI.color = Color.cyan;
            else
                GUI.color = Color.green;
            
            if (GUILayout.Button("Build Package", GUILayout.Height(30)))
            {
                Build();
            }
            GUI.color = col;
        }

        private void Build()
        {
            var assetFolder = EditorUtility.SaveFolderPanel("Select build folder", string.Empty, string.Empty);
            if (string.IsNullOrEmpty(assetFolder))
                return;
            
            pipeline.Build(new KouhaiBuildPipeline.PipelineParameters()
            {
                TargetPath = assetFolder,
                PublishingData = publishData
            });
        }
    }
}

