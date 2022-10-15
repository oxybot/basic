using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Basic.Model
{
    /// <summary>
    /// Tests on the <see cref="BaseModel"/> usage.
    /// </summary>
    [TestClass]
    public class BaseModelTest
    {
        /// <summary>
        /// Gets or sets the current test context.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Asserts that all model classes inherit from <see cref="BaseModel"/>.
        /// </summary>
        [TestMethod]
        public void AllModelClassesInheritFromBaseModel()
        {
            var types = typeof(BaseModel).Assembly.GetTypes()
                .Where(t => t.IsPublic && !t.IsAbstract && !t.IsEnum);

            foreach (var type in types)
            {
                if (type.GetCustomAttribute<OwnedAttribute>() != null)
                {
                    continue;
                }

                if (!type.IsSubclassOf(typeof(BaseModel)))
                {
                    Assert.Fail("{0} should be inheriting from BaseModel", type.Name);
                }
            }
        }

        /// <summary>
        /// Assets that all properties linked to model classes are virtual.
        /// </summary>
        [TestMethod]
        public void AllLinkedModelPropertiesAreVirtual()
        {
            var types = typeof(BaseModel).Assembly.GetTypes()
                .Where(t => t.IsPublic && !t.IsAbstract && !t.IsEnum)
                .Where(t => t.GetCustomAttribute<OwnedAttribute>() == null);

            foreach (var type in types)
            {
                foreach (var property in type.GetProperties())
                {
                    if (property.PropertyType.IsAssignableTo(typeof(BaseModel)))
                    {
                        if (property.SetMethod == null)
                        {
                            // Ignore the simple property without a setter (calculated fields)
                            continue;
                        }

                        var method = property.GetMethod;
                        if (method == null)
                        {
                            continue;
                        }

                        Assert.IsTrue(method.IsVirtual, "{0}.{1} should be virtual", type.Name, property.Name);
                    }
                    else if (property.PropertyType.IsAssignableTo(typeof(IEnumerable<BaseModel>)))
                    {
                        var method = property.GetMethod;
                        if (method == null)
                        {
                            continue;
                        }

                        Assert.IsTrue(method.IsVirtual, "{0}.{1} should be virtual", type.Name, property.Name);
                    }
                    else
                    {
                        TestContext.WriteLine("ignore {0}.{1}", type.Name, property.Name);
                    }
                }
            }
        }
    }
}
