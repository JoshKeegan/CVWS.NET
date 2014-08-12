/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Serialization Binder for anything inside the libCVWS assembly
 * Checks the assembly name of anything being deserialized for previous names of this assembly
 * By Josh Keegan 12/08/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.SerializationHelpers
{
    sealed class AssemblySerializationBinder : SerializationBinder
    {
        private readonly String[] PREVIOUS_ASSEMBLY_NAMES = { "SharedHelpers" };
        private readonly String[] PREVIOUS_ROOT_NAMESPACE_NAMES = { "SharedHelpers" };


        public override Type BindToType(string assemblyFullName, string fullTypeName)
        {
            //Split Assembly Details into (name, Version, Culture, PublicKeyToken), e.g. SharedHelpers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            string[] parts = assemblyFullName.Split(", ".ToCharArray());
            string assemblyName = parts[0];

            //If the assembly of this object being deserialized is within this assembly, but under an older name then use the current class in this Assembly
            if(PREVIOUS_ASSEMBLY_NAMES.Contains(assemblyName))
            {
                assemblyFullName = Assembly.GetExecutingAssembly().FullName; //Contains version number etc...
            }

            //Check the type is in the correct namespace
            string[] typeParts = fullTypeName.Split('.');
            if(PREVIOUS_ROOT_NAMESPACE_NAMES.Contains(typeParts[0]))
            {
                //Update to the current namespace
                Type t = typeof(AssemblySerializationBinder);
                string strNamespace = t.Namespace;
                string strRootNamespace = strNamespace.Split('.')[0];

                fullTypeName = strRootNamespace + fullTypeName.Substring(typeParts[0].Length);
            }

            //return the type from the strings
            return Type.GetType(String.Format("{0}, {1}", fullTypeName, assemblyFullName));
        }
    }
}
