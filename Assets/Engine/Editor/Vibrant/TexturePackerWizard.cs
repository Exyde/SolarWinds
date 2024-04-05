using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Vibrant

{
    //from tutorial https://www.youtube.com/watch?v=Z7TysrGi0Z0
    
    [Flags]
    public enum TextureChannel
    {
        R = 1,
        G = 2,
        B = 4,
        A = 8,
    };
    public class TexturePackerWizard : ScriptableWizard
    {
        [Header("Output Texture Settings")]
        [SerializeField] private string textureName = "New Texture";
        [SerializeField] private int dimension = 1024;
        
        [Space(5)]
        [Header("Textures to pack")]
        [SerializeField] private List<TextureWrapper> _textures = new List<TextureWrapper>();
        private Texture2D _packedTexture;

        [Header("Settings")]
        [SerializeField] private bool _verboseLogging = false;
        [SerializeField] private bool _clear = false;
        private string SavePath
        {
            get
            {
                var refTexture = _textures[0].Texture;
                var texPath = AssetDatabase.GetAssetPath(refTexture);
                texPath = texPath.Substring(0, texPath.IndexOf(refTexture.name, StringComparison.Ordinal));
                return texPath;
            }
        }
        
        [MenuItem("Vibrant/Texture Packer")]
        private static void CreateWizard()
        {
            DisplayWizard<TexturePackerWizard>("Texture Packer", "Pack", "Clear");
        }
        private void Awake()
        {
            Clear();
        }
        
        private void OnWizardUpdate()
        {
            errorString = "";
            helpString = "Welcome to the texture packer tool !";
            isValid = true;
            
            if (!IsPowerOfTwo(dimension))
            {
                errorString += "Pick a power of two dimensions !";
                isValid = false;
            }
        }

        private Color[] GetColorArray()
        {
            Color[] colors = new Color[dimension * dimension];
            
            foreach (var wrapper in _textures)
            {
                Texture2D tex = wrapper.Texture;
                Color[] pixels = tex.GetPixels();

                TextureChannel readChannel = wrapper.ReadChannel;
                TextureChannel writeChannel = wrapper.WriteChannel;
                
                for (int index  = 0; index < dimension * dimension; index++)
                {
                    colors[index] += WritePixel(pixels[index], readChannel, writeChannel);
                }
            }
            
            return colors;
        }

        Color WritePixel(Color pixelColor, TextureChannel from, TextureChannel to)
        {
            Color color = new Color(
                TextureChannel.R == to ? from.Value(pixelColor) : 0, 
                TextureChannel.G == to ? from.Value(pixelColor) : 0,
                TextureChannel.B == to ? from.Value(pixelColor) : 0,
                TextureChannel.A == to ? from.Value(pixelColor) : 0 
            );
            return color;
        }
        
        private void OnWizardCreate()
        {
            _packedTexture = new Texture2D(dimension, dimension);
            _packedTexture.SetPixels(GetColorArray());

            byte[] texture = _packedTexture.EncodeToPNG();

            string path = SavePath + textureName + ".png";
            
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter writer = new BinaryWriter(stream);
            foreach (var b in texture)
            {
                writer.Write(b);
            }
            
            stream.Close();
            writer.Close();
            
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();
        }

        private void OnWizardOtherButton()
        {
            Clear();
        }

        void Clear()
        {
            if (!_clear) return;
            _textures.Clear();
            textureName = "New Texture";
            dimension = 0;
        }

        [Serializable]
        class TextureWrapper
        {
            [SerializeField] private Texture2D _texture;
            [Space(5)][SerializeField] private bool _invert;
            
            [Header(("Packing"))]
            [Space(10)] [SerializeField] private TextureChannel _readChannel;
            [SerializeField] private TextureChannel _writeChannel;

            public Texture2D Texture => _texture;
            public TextureChannel ReadChannel => _readChannel;
            public TextureChannel WriteChannel => _writeChannel;
        }

        #region Utility

        bool IsPowerOfTwo(int x) => (x != 0) && ((x & (x - 1)) == 0);
            
        #endregion
        
        #region Logger
        private void Log(string message)
        {
            if (!_verboseLogging) return;
            Debug.Log(message);
        }

        private void LogError(string message)
        {
            Debug.LogError(message);
        }
        #endregion

    }
    
    
    public static class TextureChannelUtility
    {
        public static float Value(this TextureChannel channel, Color color)
        {
            return channel switch
            {
                TextureChannel.R => color.r,
                TextureChannel.G => color.g,
                TextureChannel.B => color.b,
                TextureChannel.A => color.a,
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, "Incorrect Texture Channel Value")
            };
        }
    }
}