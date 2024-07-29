namespace backend.Models;

public class User {
    public int Id { get; set; }
    public int GreebloHealth { get; set; }
    public int GreebloCacao { get; set; }
    public int PickedBar { get; set; }
    public int Day { get; set; }
    public int Time { get; set; }
    public int[]? BarIds { get; set; }
}