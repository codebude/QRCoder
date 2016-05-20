using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace QRCoderTests.XUnitExtenstions
{
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
        public CategoryAttribute(string category) { }
    }
}
