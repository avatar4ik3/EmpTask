using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models.Dtos.Requests;

public class CreateEmployeeSubordinateRequest : CreateEmployeeRequest
{
    public int ManagerId { get; set; }
}