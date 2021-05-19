using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Gmtl.HandyLib.AspNetCore.Tests
{
    public class HLModelStateTests
    {
        [Fact]
        public void ShouldHaveTwoErrorsForOneField()
        {
            ModelStateDictionary dict = new ModelStateDictionary();
            dict.AddModelError("k1", "error1");
            dict.AddModelError("k1", "error2");

            var errors = dict.GetErrors();

            Assert.True(errors.ContainsKey("k1"));
            Assert.Contains("error1", errors["k1"]);
            Assert.Contains("error2", errors["k1"]);
        }

        [Fact]
        public void ShouldHaveOneErrorForOneField()
        {
            ModelStateDictionary dict = new ModelStateDictionary();
            dict.AddModelError("k1", "error1");

            var errors = dict.GetErrors();

            Assert.True(errors.ContainsKey("k1"));
            Assert.Contains("error1", errors["k1"]);
        }

        [Fact]
        public void ShouldHaveOneErrorForOneFieldAndNoErrorsForOther()
        {
            ModelStateDictionary dict = new ModelStateDictionary();
            dict.AddModelError("k1", "error1");
            dict.AddModelError("k2", "");

            var errors = dict.GetErrors();

            Assert.True(errors.ContainsKey("k1"));
            Assert.Contains("error1", errors["k1"]);
        }
        [Fact]
        public void ShouldHaveNoErrors()
        {
            ModelStateDictionary dict = new ModelStateDictionary();

            var errors = dict.GetErrors();

            Assert.True(errors.Count == 0);
        }
    }
}
