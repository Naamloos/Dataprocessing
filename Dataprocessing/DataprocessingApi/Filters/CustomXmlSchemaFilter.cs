/*
 * 
 * Na een heleboel stress...
 * Source: https://github.com/domaindrivendev/Swashbuckle/issues/1050#issuecomment-542604766
 * 
 */

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataprocessingApi.Filters
{
    public class CustomXmlSchemaFilter : ISchemaFilter
    {
        private const string _SCHEMA_ARRAY_TYPE = "array";
        private const string _SCHEMA_STRING_TYPE = "string";
        private const string _PREFIX_ARRAY = "ArrayOf";

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsValueType)
                return;

            if (schema.Type == _SCHEMA_STRING_TYPE)
                return;

            schema.Xml = new OpenApiXml
            {
                Name = context.Type.Name
            };

            if (schema.Type == _SCHEMA_ARRAY_TYPE && schema.Items.Reference != null)
            {
                schema.Xml = new OpenApiXml
                {
                    Name = $"{_PREFIX_ARRAY}{schema.Items.Reference.Id}",
                    Wrapped = true,
                };
            }

            if (schema.Properties == null)
            {
                return;
            }

            foreach (var property in schema.Properties.Where(x => x.Value.Type == _SCHEMA_ARRAY_TYPE))
            {
                property.Value.Items.Xml = new OpenApiXml
                {
                    Name = property.Value.Items.Type,
                };
                property.Value.Xml = new OpenApiXml
                {
                    Name = property.Key,
                    Wrapped = true
                };
            }
        }
    }
}
