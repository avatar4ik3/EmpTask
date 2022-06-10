using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models.Dtos.Requests;

public class CreateClaimRequest
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string Value { get; set; }
}