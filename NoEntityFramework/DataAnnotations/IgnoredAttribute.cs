using System;

namespace NoEntityFramework.DataAnnotations
{
    /// <summary>
    ///     Data mapping will be ignored when this attribute is applied
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoredAttribute : Attribute
    {

    }
}