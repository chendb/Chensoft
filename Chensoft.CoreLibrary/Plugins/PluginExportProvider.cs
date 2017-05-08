using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Chensoft.Plugins
{
    public class PluginExportProvider : ExportProvider
    {
        public ApplicationContext Context { get; set; }

        public PluginExportProvider(ApplicationContext applicationContext)
        {
            this.Context = applicationContext;
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            IEnumerable<Export> returnValue = Enumerable.Empty<Export>();
            List<Export> exports = new List<Export>();

            foreach (var dir in this.Context.Features)
            {
                exports.AddRange(GetExports(dir, definition.ContractName));
            }
            if (exports.Count == 0) return returnValue;
            return exports;
           
        }

        private IEnumerable<Export> GetExports(PluginFeature dir, string contractName)
        {
            List<Export> exports = new List<Export>();
            foreach (var builtin in dir.GetBuiltins())
            {
                if (builtin.Contract == contractName)
                {
                    Type t = PluginUtils.GetType(builtin, this.Context);
                    object instance = t.GetConstructor(Type.EmptyTypes).Invoke(null);
                    try
                    {
                        this.Context.ComposeParts(instance);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    
                    ExportDefinition exportDefintion = new ExportDefinition(contractName, new Dictionary<string, object>());
                    Export toAdd = new Export(exportDefintion, () => instance);
                    exports.Add(toAdd);
                }
            }

            return exports;
        }
    }
}
