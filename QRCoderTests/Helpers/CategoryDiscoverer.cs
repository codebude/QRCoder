using System;
using System.Collections.Generic;
using System.Linq;
#if !NETFRAMEWORK
using Xunit.Abstractions;
#endif
using Xunit.Sdk;

namespace QRCoderTests.Helpers.XUnitExtenstions;

#if NETFRAMEWORK
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CategoryAttribute : Attribute
{
#pragma warning disable IDE0060 // Remove unused parameter
    public CategoryAttribute(string category) { }
#pragma warning restore IDE0060 // Remove unused parameter
}
#else
public class CategoryDiscoverer : ITraitDiscoverer
{
    public const string KEY = "Category";

    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        var ctorArgs = traitAttribute.GetConstructorArguments().ToList();
        yield return new KeyValuePair<string, string>(KEY, ctorArgs[0].ToString());
    }
}

//NOTICE: Take a note that you must provide appropriate namespace here
[TraitDiscoverer("QRCoderTests.XUnitExtenstions.CategoryDiscoverer", "QRCoderTests")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CategoryAttribute : Attribute, ITraitAttribute
{
#pragma warning disable IDE0060 // Remove unused parameter
    public CategoryAttribute(string category) { }
#pragma warning restore IDE0060 // Remove unused parameter
}
#endif
