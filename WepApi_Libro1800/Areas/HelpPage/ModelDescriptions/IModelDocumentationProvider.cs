using System;
using System.Reflection;

namespace WepApi_Libro1800.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModelDocumentationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        string GetDocumentation(MemberInfo member);

        /// <summary>
        /// 
        /// </summary>
        string GetDocumentation(Type type);
    }
}