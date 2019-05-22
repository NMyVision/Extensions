using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMyVision.Extensions;

namespace ConverterTests
{
    [TestClass]
    public class ConversionExtensionsTest
    {
        // get the conversion tests
        // https://github.com/dotnet/corefx/tree/master/src/System.Runtime.Extensions/tests/System
        [TestMethod]
        public void To_T()
        {


        }

        [TestMethod]
        public void To_T_DefaultValue() { }


        [TestMethod]
        public void To()
        {

           
        }
    }

    [TestClass]
    public class ConvertersTests
    {
        private const string NULL = null;
        private const string EMPTYSTRING = "";

        [TestMethod]
        public void ToStringTest()
        {
            string x = null;

            var t = typeof(String);

            Assert.AreEqual(null, NMyVision.Extensions.CoreExtensions.To(x, t),
                $"To( [null] , { t.Name }) --> null");

            Assert.AreEqual("test", NMyVision.Extensions.CoreExtensions.To("test", t),
                $"To( 'test' , { t.Name }) --> 'test'");

        }

        [TestMethod]
        public void ToBool()
        {
            object o = null;
            Assert.AreEqual(false, o.To<bool>(false), $" [object (null)].To<bool>(false) --> { false }");


            o = true;
            Assert.AreEqual(true, o.To<bool>(), $" [object (true)].To<bool>() --> { true }");

            var dict = new Dictionary<string, Boolean>
            {
                ["1"] = true,
                ["TRUE"] = true,
                ["-1"] = true,
                ["0"] = false,
                ["FALSE"] = false,
                ["checked"] = true,
                ["yes"] = true,
                ["no"] = false
            };

            dict.Each(kv => Assert.AreEqual(kv.Value, kv.Key.To<bool>()));

            Assert.AreEqual(true.ToString(), true.To<string>(), $"'{ true }'.To<string>() --> { true }");
            Assert.AreEqual(false.ToString(), false.To<string>(), $"'{ false }'.To<string>() --> { false }");

            TestInvalidInputs(true);
        }

       

        private enum TestCharacters
        {
            //[KamDescription(Lookup = "M", Description = "Super Mario")]
            Mario = 0,
            //[KamDescription(Lookup = "L")]
            Luigi = 1,
            //[KamDescription(Lookup = "B", Description = "King Koopa")]
            Bowser = 2,
            //[KamDescription(Lookup = "T")]
            Toad = 3,
            //[KamDescription(Lookup = "P")]
            PrincessPeach = 4
        }

        [TestMethod]
        public void ToEnum()
        {
            //if tests work for one numeric type it should work for all...
            AreEqual(TestCharacters.Mario, "Mario");
            AreEqual(TestCharacters.Luigi, "1");
                        
            Assert.ThrowsException<ArgumentException>(() => "c".To<TestCharacters?>());

            Assert.AreEqual(null, "c".To<TestCharacters?>(null), "Failed to convert nullable enum to null");
             

            Assert.AreEqual(TestCharacters.Mario, "Mario".To<TestCharacters?>(), "Failed to convert 'Mario' enum to 'Mario' when nullable enum");
        }

        [TestMethod]
        public void ToGuid()
        {
            var g = Guid.NewGuid();
            AreEqual<Guid>(g);

            TestInvalidInputs(new Guid());
        }

        [TestMethod]
        public void ToNumber()
        {
            //if tests work for one numeric type it should work for all...
            AreEqual(0, "0");
            AreEqual(0.1m, "0.1");
            AreEqual(0.1d, "0.1");
            AreEqual(2015, "2015");
            AreEqual(0.001d, "0.001");

            AreEqual(0.001d, "0.001");

            TestInvalidInputs(1);

            //some integer specific 
            AssertHelper.ShouldThrowException(() => "1.1".To<int>(), "1.1 --> error");
            Assert.AreEqual(2, "1.1".To<int>(2), "'1.1' (default:2) --> 2");

            Assert.AreEqual(null, "x".To<int?>(null), "x.To<int?> --> null");
            Assert.AreEqual("0.01", 0.01d.To<string>(), "0.01.To<string> --> 0.01");


        }

        [TestMethod]
        public void ToDateTime()
        {
            //if tests work for one numeric type it should work for all...

            var d = new DateTime(2015, 9, 15);

            AreEqual(d, "2015/09/15");

            AreEqual(d, "09/15/2015");

            TestInvalidInputs(1);

        }

        private void AreEqual<T>(T result, string input = null)
        {
            if (input == null)
                input = result.ToString();

            Assert.AreEqual(result, input.To<T>(), $"{ input }.To<{typeof(T).Name}> --> { result }");


        }

        private void TestInvalidInputs<T>(T defaultValue)
        {
            var name = typeof(T).Name;

            //null or empty without default value should fail
            AssertHelper.ShouldThrowException(() => EMPTYSTRING.To<T>(), $"A. {{ Empty.String }} <{ name }> should throw an error.");
            AssertHelper.ShouldThrowException(() => NULL.To<T>(), $"B. {{ null }} <{ name }> should throw an error.");
            AssertHelper.ShouldThrowException(() => "invalid".To<T>(), $"C. 'invalid' <{ name }>  should throw an error.");
            AssertHelper.ShouldThrowException(() => EMPTYSTRING.To(typeof(T)), $"D. {{ Empty.String }} ({ name }) should throw an error.");

            //null or empty strings with a default should return the default
            Assert.AreEqual(defaultValue, EMPTYSTRING.To<T>(defaultValue), $"E. '' (default: {defaultValue}) --> { defaultValue }");
            Assert.AreEqual(defaultValue, NULL.To<T>(defaultValue), $"F. null (default: {defaultValue}) --> { defaultValue }");
        }
    }
}
