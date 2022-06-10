using System.Collections.Generic;
using AutoMapper;
using Managment.Common.Models;
using Managment.Common.Models.Dtos.Requests;
using Managment.Common.Models.Dtos.Responses;
using Managment.Employees.Data;
using Nito.AsyncEx.Synchronous;

namespace Managment.Employees.Profiles;

public class AppProfile : Profile
{
    public AppProfile()
    {
        //мапперы внутри реквестов
        //из реквостов в модели
        CreateMap<CreateEmployeeRequest, EmployeeBase>();
        CreateMap<CreateEmployeeSubordinateRequest, EmployeeSubordinate>();
        CreateMap<CreateEmployeeManagerRequest, EmployeeManager>();
;
        //из можелей в ответы
        CreateMap<EmployeeBase,EmployeeBaseResponse>();
        CreateMap<EmployeeSubordinate,EmployeeSubordinateResponse>();
        CreateMap<EmployeeManager,EmployeeManagerResponse>()
            .ForMember(
                d => d.SubordinatesId,
                options => options.MapFrom(s => s.Subordinates.Select(x => x.Id)
                    ));
        
        //мапперы для повышение, понижяние
        //повышение employeeBase
        CreateMap<EmployeeBase,EmployeeSubordinate>();
        //понижение subordinate до employeeBase
        CreateMap<EmployeeSubordinate,EmployeeBase>();
        //повышение subordinate до manager
        CreateMap<EmployeeSubordinate,EmployeeManager>()
            .ForMember(
                d => d.Subordinates,
                options =>options.Ignore()
            );

        //понижение manager до subordinate
        CreateMap<EmployeeManager,EmployeeSubordinate>();


    }

}