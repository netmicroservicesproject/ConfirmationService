using System.ComponentModel.DataAnnotations;

public class ConfirmationEntity {
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EventName { get; set; } = null!;
    public string Information { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}