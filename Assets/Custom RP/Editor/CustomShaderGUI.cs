using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class CustomShaderGUI : ShaderGUI
{
    private MaterialEditor _materialEditor;
    private Object[] _materials;
    private MaterialProperty[] _properties;

    private bool _showPreset;
    
    private bool clipping
    {
        set => SetProperty("_Clipping", "_CLIPPING", value);
    }

    private bool premulAlpha
    {
        set => SetProperty("_PremulAlpha", "_PREMULTIPLY_ALPHA", value);
    }

    private BlendMode srcBlend
    {
        set => SetProperty("_SrcBlend", (float) value);
    }

    private BlendMode dstBlend
    {
        set => SetProperty("_DstBlend", (float) value);
    }

    private bool zWrite
    {
        set => SetProperty("_ZWrite", value ? 1f : 0f);
    }

    RenderQueue renderQueue
    {
        set
        {
            foreach (Material material in _materials)
            {
                material.renderQueue = (int) value;
            }
        }
    }
    
    bool HasProperty (string name) =>
        FindProperty(name, _properties, false) != null;
    
    bool HasPremultiplyAlpha => HasProperty("_PremulAlpha");
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);

        _materialEditor = materialEditor;
        _materials = materialEditor.targets;
        _properties = properties;
        
        EditorGUILayout.Space();

        _showPreset = EditorGUILayout.Foldout(_showPreset, "Presets", true);
        if (_showPreset)
        {
            OpaquePreset();
            ClipPreset();
            FadePreset();
            TransparentPreset();   
        }
    }

    bool SetProperty(string name, float value)
    {
        var property = FindProperty(name, _properties);

        if (property != null)
        {
            property.floatValue = value;
            
            return true;
        }

        return false;
    }

    void SetProperty(string name, string keyword, bool value)
    {
        if (SetProperty(name, value ? 1f : 0f))
        {
            SetKeyword(keyword, value);   
        }
    }

    void SetKeyword(string keyword, bool enabled)
    {
        if (enabled)
        {
            foreach (Material material in _materials)
            {
                material.EnableKeyword(keyword);
            }
        }
        else
        {
            foreach (Material material in _materials)
            {
                material.DisableKeyword(keyword);
            }
        }
    }

    bool PresetButton(string name)
    {
        if (GUILayout.Button(name))
        {
            _materialEditor.RegisterPropertyChangeUndo(name);
            return true;
        }

        return false;
    }

    void OpaquePreset()
    {
        if (PresetButton("Opaque"))
        {
            clipping = false;
            premulAlpha = false;
            srcBlend = BlendMode.One;
            dstBlend = BlendMode.Zero;
            zWrite = true;
            renderQueue = RenderQueue.Geometry;
        }
    }

    void ClipPreset()
    {
        if (PresetButton("Clip"))
        {
            clipping = true;
            premulAlpha = false;
            srcBlend = BlendMode.One;
            dstBlend = BlendMode.Zero;
            zWrite = true;
            renderQueue = RenderQueue.AlphaTest;
        }
    }
    
    void FadePreset()
    {
        if (PresetButton("Fade"))
        {
            clipping = false;
            premulAlpha = false;
            srcBlend = BlendMode.SrcAlpha;
            dstBlend = BlendMode.OneMinusSrcAlpha;
            zWrite = false;
            renderQueue = RenderQueue.Transparent;
        }
    }
    
    void TransparentPreset()
    {
        if (HasPremultiplyAlpha && PresetButton("Transparent"))
        {
            clipping = false;
            premulAlpha = true;
            srcBlend = BlendMode.SrcAlpha;
            dstBlend = BlendMode.OneMinusSrcAlpha;
            zWrite = false;
            renderQueue = RenderQueue.Transparent;
        }
    }
}