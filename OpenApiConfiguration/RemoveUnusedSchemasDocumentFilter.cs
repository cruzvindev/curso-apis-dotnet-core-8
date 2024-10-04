using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APICatalogo.OpenApiConfiguration;

public class RemoveUnusedSchemasDocumentFilter : IDocumentFilter
{
    private readonly HashSet<string> _schemasToRemove;

    public RemoveUnusedSchemasDocumentFilter()
    {
        _schemasToRemove = new HashSet<string>()
        {
            "Operation",
            "OperationType"
        };
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var schemaToRemove in _schemasToRemove)
        {
            // Remove o schema se ele existir no dicionário de schemas
            if (swaggerDoc.Components.Schemas.ContainsKey(schemaToRemove))
            {
                swaggerDoc.Components.Schemas.Remove(schemaToRemove);
            }
        }
    }
}