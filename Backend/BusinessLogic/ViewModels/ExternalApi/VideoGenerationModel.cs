namespace BusinessLogic.ViewModels.ExternalApi;

public class VideoGenerationModel
{
    public string key { get; set; }
    public string model_id { get; set; }
    public string prompt { get; set; }
    public string negative_prompt { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public int num_frames { get; set; }
    public int fps { get; set; }
    public int num_inference_steps { get; set; }
    public int guidance_scale { get; set; }
    public int upscale_height { get; set; }
    public int upscale_width { get; set; }
    public float upscale_strength { get; set; }
    public int upscale_guidance_scale { get; set; }
    public int upscale_num_inference_steps { get; set; }
    public string output_type { get; set; }
}
