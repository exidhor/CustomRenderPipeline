using UnityEngine.Rendering;
using UnityEngine;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField] private bool _useDynamicBatching = true;
    [SerializeField] private bool _useGPUInstancing = true;
    [SerializeField] private bool _useSRPBatching = true;

    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(_useDynamicBatching, _useGPUInstancing, _useSRPBatching);
    }
}
