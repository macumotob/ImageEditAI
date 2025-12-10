namespace ImageEditAI.Model;

public class snap
{
    public string model { get; set; } = "";
    public float model_prob { get; set; }
    public string @object { get; set; } = "";
    public float object_prob { get; set; }
    public List<int> box { get; set; } = new List<int>();
    public bool no_object { get; set; }
    public string output { get; set; } = "";
}