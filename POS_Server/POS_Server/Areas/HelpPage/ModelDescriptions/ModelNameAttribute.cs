using System;

namespace POS_Server.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// Use this attribute to change the Name of the <see cref="ModelDescription"/> generated for a type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public sealed class ModelNameAttribute : Attribute
    {
        public ModelNameAttribute(string Name)
        {
            Name = Name;
        }

        public string Name { get; private set; }
    }
}