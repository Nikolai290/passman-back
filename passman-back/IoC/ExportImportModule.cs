using Microsoft.Extensions.DependencyInjection;
using passman_back.Business.Interfaces.Exporters;
using passman_back.Business.Interfaces.Importers;
using passman_back.Infrastructure.Business.Exporters;
using passman_back.Infrastructure.Business.Importers;

namespace passman_back.IoC {
    public static class ExportImportModule {
        public static void ConfigureExportImport(this IServiceCollection services) {
            // Export module
            services.AddScoped<IFlatExporter, FlatExporter>();
            services.AddScoped<IHierarchyPassmanExporter, HierarchyPassmanExporter>();

            // Import module
            services.AddScoped<IFlatImporter, FlatImporter>();
            services.AddScoped<IHierarchyPassmanImporter, HierarchyPassmanImporter>();
            services.AddScoped<IHierarchyBitwardernImporter, HierarchyBitwardernImporter>();
        }
    }
}