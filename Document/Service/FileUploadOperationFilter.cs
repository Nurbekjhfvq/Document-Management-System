using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Parametrlarning har birini tekshirib chiqing
        foreach (var parameter in operation.Parameters)
        {
            if (parameter.Schema.Type == "string" && parameter.Schema.Format == "binary")
            {
                // Agar fayl yuklash bo'lsa, uni 'IFormFile' tipida ekanligini aniqlash
                parameter.Description = "File to be uploaded";
            }
        }
    }
}
