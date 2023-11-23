using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class RaymarchRendererFeature : ScriptableRendererFeature
{
    private RaymarchRenderPass raymarchRenderPass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(raymarchRenderPass);
    }

    public override void Create()
    {
        raymarchRenderPass = new RaymarchRenderPass();
    }
}

public class RaymarchRenderPass : ScriptableRenderPass
{
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        
    }
}
