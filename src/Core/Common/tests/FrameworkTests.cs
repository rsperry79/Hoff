using System;
using System.Diagnostics;

using nanoFramework.TestFramework;

namespace Hoff.Core.Common.Tests
{
    [TestClass]
    public class FrameworkTests
    {
        #region Public Methods

        [TestMethod]
        public void TestCheckAllEqual()
        {

            Debug.WriteLine("Test will check that all the Equal are actually equal");
            // Arrange
            const byte bytea = 42; const byte byteb = 42;
            const char chara = (char)42; const char charb = (char)42;
            const sbyte sbytea = 42; const sbyte sbyteb = 42;
            const int inta = 42; const int intb = 42;
            const uint uinta = 42; const uint uintb = 42;

            const long longa = 42; const long longb = 42;
            const ulong ulonga = 42; const ulong ulongb = 42;
            const bool boola = true; const bool boolb = true;
            const short shorta = 42; const short shortb = 42;
            const ushort ushorta = 42; const ushort ushortb = 42;
            const float floata = 42; const float floatb = 42;
            int[] intArraya = new int[5] { 1, 2, 3, 4, 5 };
            int[] intArrayb = new int[5] { 1, 2, 3, 4, 5 };
            object obja = new(); object objb = obja;
            const string stra = "42"; const string strb = "42";
            byte[] arrayempty = new byte[0];

            // Assert
            Assert.IsTrue(boola);
            Assert.AreEqual(bytea, byteb);
            Assert.AreEqual(chara, charb);
            Assert.AreEqual(sbytea, sbyteb);
            Assert.AreEqual(inta, intb);
            Assert.AreEqual(uinta, uintb);
            Assert.AreEqual(longa, longb);
            Assert.AreEqual(ulonga, ulongb);
            Assert.AreEqual(boola, boolb);
            Assert.AreEqual(shorta, shortb);
            Assert.AreEqual(ushorta, ushortb);
            Assert.AreEqual(floata, floatb);

            Assert.AreEqual(intArraya.Length, intArrayb.Length);
            Assert.AreNotSame(intArraya, intArrayb);
            for (int i = 0; i < intArraya.Length; i++)
            {
                Assert.AreEqual(intArraya[i], intArrayb[i]);
            }

            Assert.AreEqual(stra, strb);
            Assert.AreEqual(obja, objb);
            Assert.IsTrue(arrayempty != null);
        }

        [TestMethod]
        public void TestCheckAllNotEqual()
        {
            Debug.WriteLine("Test will check that all the NotEqual are actually equal");

            // Arrange
            const byte bytea = 42; const byte byteb = 43;
            const char chara = (char)42; const char charb = (char)43;
            const sbyte sbytea = 42; const sbyte sbyteb = 43;
            const int inta = 42; const int intb = 43;
            const uint uinta = 42; const uint uintb = 43;
            const long longa = 42; const long longb = 43;
            const ulong ulonga = 42; const ulong ulongb = 43;
            const bool boola = true; const bool boolb = false;
            const short shorta = 42; const short shortb = 43;
            const ushort ushorta = 42; const ushort ushortb = 43;
            const float floata = 42; const float floatb = 43;
            int[] intArraya = new int[5] { 1, 2, 3, 4, 5 };
            int[] intArrayb = new int[5] { 1, 2, 3, 4, 6 };
            int[] intArraybis = new int[4] { 1, 2, 3, 4 };
            int[] intArrayter = null;
            object obja = new(); object objb = new();
            const string stra = "42"; const string strb = "43";

            // Assert
            Assert.IsFalse(boolb);
            Assert.AreNotEqual(bytea, byteb);
            Assert.AreNotEqual(chara, charb);
            Assert.AreNotEqual(sbytea, sbyteb);
            Assert.AreNotEqual(inta, intb);
            Assert.AreNotEqual(uinta, uintb);
            Assert.AreNotEqual(longa, longb);
            Assert.AreNotEqual(ulonga, ulongb);
            Assert.AreNotEqual(boola, boolb);
            Assert.AreNotEqual(shorta, shortb);
            Assert.AreNotEqual(ushorta, ushortb);
            Assert.AreNotEqual(floata, floatb);
            Assert.AreNotEqual(intArraya, intArrayb);
            Assert.AreNotEqual(intArraya, intArraybis);
            Assert.AreNotEqual(intArraya, intArrayter);
            Assert.AreNotEqual(stra, strb);
            Assert.AreNotSame(obja, objb);
            Assert.IsNotNull(intArraya);
        }

        [TestMethod]
        public void TestNullEmpty()
        {
            Debug.WriteLine("Test null, not null, types");

            // Arrange
            object objnull = null;
            object objnotnull = new();
            Type typea = typeof(int);
            Type typeb = typeof(int);
            Type typec = typeof(long);

            // Assert
            Assert.IsNull(objnull);
            Assert.IsNotNull(objnotnull);
            Assert.IsType(typea, typeb);
            Assert.AreNotSame(typea, typec);
        }

        [TestMethod]
        public void TestRaisesException()
        {
            Debug.WriteLine("Test will raise exception");
            Assert.ThrowsException(typeof(Exception), this.ThrowMe);
        }

        [TestMethod]
        public void TestStringComparison()
        {
            Debug.WriteLine("Test string, Contains, EndsWith, StartWith");

            // Arrange
            const string tocontains = "this text contains and end with contains";
            const string startcontains = "contains start this text";
            const string contains = "contains";
            const string doesnotcontains = "this is totally something else";
            string empty = string.Empty;
            const string stringnull = null;

            // Assert
            Assert.Contains(contains, tocontains);
            Assert.DoesNotContains(contains, doesnotcontains);
            Assert.DoesNotContains(contains, empty);
            Assert.AreNotSame(contains, stringnull);
            Assert.StartsWith(contains, startcontains);
            Assert.EndsWith(contains, tocontains);
        }

        #endregion Public Methods

        #region Private Methods

        private void ThrowMe()
        {
            throw new Exception("Test failed and it's a shame");
        }

        #endregion Private Methods
    }
}
