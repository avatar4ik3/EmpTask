using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Managment.Common.Extensions;

public static class IdentityResultErrorExtensions
{
    public static void CheckErrors(this IdentityResult result){
        if(result.Succeeded is false){
            throw new BadHttpRequestException(result.Errors.Select(x => x.Description).Aggregate((x1,x2) => $"{x1}\n{x2}"));
        }
    }
}