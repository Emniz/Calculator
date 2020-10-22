using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcLibrary;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ResultTestMethod()
        {
            string result = Calc.DoOperation("23+4,5");
            Assert.AreEqual("27,5", result);
        }

        [TestMethod]
        public void ResultTestMethod2()
        {
            string result = Calc.DoOperation("23*4");
            Assert.AreEqual("92", result);
        }

        [TestMethod]
        public void ResultTestMethod3()
        {
            string result = Calc.DoOperation("e^0");
            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void ResultTestMethod4()
        {
            string result = Calc.DoOperation("36/4,5");
            Assert.AreEqual("8", result);
        }
    }
}
