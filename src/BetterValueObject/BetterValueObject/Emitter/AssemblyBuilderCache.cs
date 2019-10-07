namespace BetterValueObject.Emitter
{
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Reflection.Emit;

    internal class AssemblyBuilderCache
    {
        private ConcurrentDictionary<string, AssemblyBuilder> assemblyBuilders;

        public AssemblyBuilderCache()
            => assemblyBuilders = new ConcurrentDictionary<string, AssemblyBuilder>();

        public AssemblyBuilder GetOrCreate(string assemblyName)
            => assemblyBuilders.GetOrAdd(assemblyName, (name) =>
                AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(name), 
                    AssemblyBuilderAccess.Run));
    }
}
