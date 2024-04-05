#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//Based on this : https://github.com/JakeCarterDPM/unity-search-material-by-shader/blob/main/Assets/Editor/SearchMaterialByShader/SearchMaterialByShader.cs

//Todo : 
// - Options for display materials per page
// - Options for preview Size
// - Target material with a particular Property value Filtering

namespace Vibrant
{
    public class ShaderInspector : EditorWindow
    {
        #region Properties
        private Shader _selectedShader;
        private bool _includeEmbeddedMaterials = false;
        private bool _onlyEmbeddedMaterials = false;

        private bool _filterMaterialByProperty;
        private string _filterPropertyName;

        //Results
        private List<Material> _foundMaterials = new();
        private List<Editor> _materialPreviews = new();

        //Window Management
        private const int MaterialsPerPage = 10;
        private const int ButtonsPerRow = 20;
        private const int HeightSelectedShaderPicker = 25;
        private const int HeightFindMaterialsButton = 25;

        private Vector2 _scrollPos;
        private string _currentPageTxt;
        private int _currentPage = 0;
        private int _startIndex;
        private int _endIndex;
        private int _pageCount;
        private bool _showResults = false;
        
        //Material Preview
        private const int MaterialPreviewSize = 48;
        private const int MaterialLabelHeight = 16;
        private const int MaterialSelectButtonHeight = 24;

        private GUIStyle _materialLabelStyle;

        //Labels
        private const string IncludeEmbeddedLabel = "Include Embedded Materials (Non-editable)";
        private const string IncludeEmbeddedTooltip =
            "For materials that are embedded into files such as a 3D model, which can't be edited directly. Fix for these mats is to \"Extract Materials...\" under the materials tab for that model.";

        private const string OnlyEmbeddedLabel = "Only Embedded Materials";
        private const string OnlyEmbeddedTooltip = "Doesn't include editable materials in results";

        #endregion

        #region Core Behavior
        [MenuItem("Vibrant/Shader Inspector")]
        public static void ShowWindow()
        {
            var window = GetWindow<ShaderInspector>("Shader Inspector");
            Vector2 windowSize = new(500, 800);
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, windowSize.x, windowSize.y);
            window.minSize = new Vector2(300, 300);
            window.Show();
        }
        private void CreateGUI()
        {
            _materialLabelStyle = new GUIStyle
            {
                normal =
                {
                    textColor = new Color(0.38f, 0.78f, 0.49f, 1)
                }
            };

            _selectedShader = Shader.Find("Universal Render Pipeline/Lit");
            MenuChangeCache();
        }
        private void MenuChangeCache()
        {
            if (!_showResults) return;
            
            //Page Navigation
            _startIndex = _currentPage * MaterialsPerPage;
            _endIndex = Mathf.Min(_startIndex + MaterialsPerPage, _foundMaterials.Count);
            _pageCount = Mathf.CeilToInt((float)_foundMaterials.Count / MaterialsPerPage);
            _currentPageTxt = $"Current Page: {_currentPage + 1}/{_pageCount}";

            //Material Previews
            _materialPreviews.Clear();
            _materialPreviews = new Editor[_foundMaterials.Count].ToList();
            for (int i = _startIndex; i < _endIndex; i++)
            {
                Material mat = _foundMaterials[i];
                Editor materialPreviewObjectEditor = Editor.CreateEditor(mat);
                _materialPreviews[i] = materialPreviewObjectEditor;
            }
        }
        
        private void OnGUI()
        {
            //Target Shader Field
            GUILayout.Space(15);
            GUILayout.Label($"Target Shader", EditorStyles.boldLabel);
            _selectedShader = EditorGUILayout.ObjectField(_selectedShader, typeof(Shader), false, GUILayout.Height(HeightSelectedShaderPicker)) as Shader;
            
            DrawOptions();
            DrawFindMaterialButton();

            //Results Panel
            if (!_showResults) return;
            
            DrawSelectionButtons();
            DrawNavigationsButtons();
            DrawMaterialList();
            
        }

        #endregion

        #region Find
        private void FindMaterials()
        {
            FindMaterialsWithShader(_selectedShader.name);
            _showResults = true;
            _currentPage = 0;
            MenuChangeCache();
        }
        
        private void FindMaterialsWithShader(string shaderName)
        {
            _foundMaterials.Clear();
            string[] allMaterialPaths = AssetDatabase.FindAssets("t:Material");

            foreach (string path in allMaterialPaths)
            {
                //Mats within Assets only.
                string assetPath = AssetDatabase.GUIDToAssetPath(path);
                if (!assetPath.StartsWith("Assets/"))
                    continue;

                //Various checks.
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                if (mat == null || mat.shader == null || string.IsNullOrEmpty(mat.shader.name))
                    continue;
                if (!mat.shader.name.Equals(shaderName, StringComparison.OrdinalIgnoreCase))
                    continue;
                
                //Material Filtering

                if (!FilterMaterial()) continue;
                
                bool FilterMaterial()
                {
                    if (!_filterMaterialByProperty) return true;

                    if (mat.HasProperty(_filterPropertyName))
                    {
                        return mat.GetFloat(_filterPropertyName) == 1;
                    }

                    return false;

                }

                if (IsMaterialEmbedded(assetPath))
                {
                    if (_includeEmbeddedMaterials)
                    {
                        _foundMaterials.Add(mat);
                    }
                }
                else
                {
                    if (!_onlyEmbeddedMaterials)
                    {
                        _foundMaterials.Add(mat);
                    }
                }
            }
            _foundMaterials.Sort((a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
        }

        #endregion

        #region Selection
        private void SelectMaterial(Material mat)
        {
            Selection.activeObject = mat;
            EditorGUIUtility.PingObject(mat);
        }
        private void SelectAllMaterials() => Selection.objects = _foundMaterials.ToArray();

        private void SelectAllMaterialsOnPage()
        {
            List<Material> materialsOnPage = _foundMaterials.GetRange(_startIndex, _endIndex - _startIndex);
            Selection.objects = materialsOnPage.ToArray();
        }
        #endregion
        
        #region Draw
        private void DrawOptions()
        {
            //Options
            GUILayout.Space(5);
            GUILayout.Label($"Options", EditorStyles.boldLabel);
            
            DrawEmbeddedOptions();
            DrawSelectionByPropertyOptions();

            void DrawSelectionByPropertyOptions()
            {
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            
                //Include
                _filterMaterialByProperty = GUILayout.Toggle(
                    _filterMaterialByProperty,
                    new GUIContent("Filter material by shader property", ""),
                    GUILayout.ExpandWidth(true));

                //Only Embedded
                if (_filterMaterialByProperty)
                {
                    _filterPropertyName = GUILayout.TextField(_filterPropertyName);
                }
                else
                {
                    _filterPropertyName = "empty";
                }
                GUILayout.EndHorizontal();
            }
            
            void DrawEmbeddedOptions()
            {
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            
                //Include
                _includeEmbeddedMaterials = GUILayout.Toggle(
                    _includeEmbeddedMaterials,
                    new GUIContent(IncludeEmbeddedLabel, IncludeEmbeddedTooltip),
                    GUILayout.ExpandWidth(true));

                //Only Embedded
                if (_includeEmbeddedMaterials)
                {
                    _onlyEmbeddedMaterials = GUILayout.Toggle(
                        _onlyEmbeddedMaterials,
                        new GUIContent(OnlyEmbeddedLabel, OnlyEmbeddedTooltip),
                        GUILayout.ExpandWidth(true));
                }
                else
                {
                    _onlyEmbeddedMaterials = false;
                }
                GUILayout.EndHorizontal();
            }
        }
        private void DrawFindMaterialButton()
        {
            GUILayout.Space(5);
            if (GUILayout.Button("Find Materials", GUILayout.Height(HeightFindMaterialsButton)))
            {
                FindMaterials();
            }
        }
        private void DrawSelectionButtons()
        {
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Select Page"))
            {
                SelectAllMaterialsOnPage();
            }
            if (GUILayout.Button("Select All Results"))
            {
                SelectAllMaterials();
            }
            
            GUILayout.EndHorizontal();
        }

        private void DrawNavigationsButtons()
        {
            GUILayout.Space(10f);
            GUILayout.Label(_currentPageTxt, GUILayout.Height(20));
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            for (int i = 0; i < _pageCount; i++)
            {
                if (i % ButtonsPerRow == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                }
                GUI.enabled = i != _currentPage;
                if (GUILayout.Button((i + 1).ToString(), GUILayout.ExpandWidth(false), GUILayout.Height(20)))
                {
                    _currentPage = i;
                    MenuChangeCache();
                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            GUILayout.Space(10f);
        }

        private void DrawMaterialList()
        {
            GUILayout.Label($"Found Materials: { _foundMaterials.Count }", EditorStyles.boldLabel);
            
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(position.width), GUILayout.ExpandHeight(true));
            
            for (int i = _startIndex; i < _endIndex; i++)
            {
                Material mat = _foundMaterials[i];
                
                if (mat == null)
                {
                    MenuChangeCache();
                    continue;
                }
                
                EditorGUILayout.BeginHorizontal();
                _materialPreviews[i].OnPreviewGUI(GUILayoutUtility.GetRect(MaterialPreviewSize, MaterialPreviewSize), new GUIStyle());
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(mat), _materialLabelStyle, GUILayout.Height(MaterialLabelHeight));
                
                if (GUILayout.Button(mat.name, GUILayout.Height(MaterialSelectButtonHeight)))
                {
                    SelectMaterial(mat);
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(10f);
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        #endregion

        #region Utilities
        private bool IsMaterialEmbedded(string assetPath)
        {
            Type assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
            return assetType != typeof(Material);
        }
        #endregion
    }
}
#endif