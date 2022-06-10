using Managment.Common.Models.Dtos.Requests;

namespace Managment.Common.Models.Dtos.Requests;
public class CreateEmployeeManagerRequest : CreateEmployeeRequest
{
    public IEnumerable<int> SubordinatesIds { get; set; }
}