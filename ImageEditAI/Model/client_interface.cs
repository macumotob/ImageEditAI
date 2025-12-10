namespace ImageEditAI.Model;
public class interface_parameter
{
    public string type { get; set; } = "";
    public string description { get; set;} = "";
}
public class client_interface
{
    public string name { get; set; } ="";
    public string action { get; set; } = "";
    public string url { get; set;} = "";
    public List<interface_parameter> parameters { get; set; } = new List<interface_parameter>();
}