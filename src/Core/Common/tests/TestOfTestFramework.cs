﻿using System;
using System.Diagnostics;

using nanoFramework.TestFramework;

namespace Hoff.Core.Tests
{
    [TestClass]
    public class FrameworkTests
    {
        [TestMethod]
        public void TestRaisesException()
        {
            Debug.WriteLine("Test will raise exception");
            Assert.ThrowsException(typeof(Exception), this.ThrowMe);
        }

        private void ThrowMe()
        {
            throw new Exception("Test failed and it's a shame");
        }

        [TestMethod]
        public void TestCheckAllEqual()
        {
            Debug.WriteLine("Test will check that all the Equal are actually equal");
            // Arrange
            byte bytea = 42; byte byteb = 42;
            char chara = (char)42; char charb = (char)42;
            sbyte sbytea = 42; sbyte sbyteb = 42;
            int inta = 42; int intb = 42;
            uint uinta = 42; uint uintb = 42;
            long longa = 42; long longb = 42;
            ulong ulonga = 42; ulong ulongb = 42;
            bool boola = true; bool boolb = true;
            short shorta = 42; short shortb = 42;
            ushort ushorta = 42; ushort ushortb = 42;
            float floata = 42; float floatb = 42;
            int[] intArraya = new int[5] { 1, 2, 3, 4, 5 };
            int[] intArrayb = new int[5] { 1, 2, 3, 4, 5 };
            object obja = new(); object objb = obja;
            string stra = "42"; string strb = "42";
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
            byte bytea = 42; byte byteb = 43;
            char chara = (char)42; char charb = (char)43;
            sbyte sbytea = 42; sbyte sbyteb = 43;
            int inta = 42; int intb = 43;
            uint uinta = 42; uint uintb = 43;
            long longa = 42; long longb = 43;
            ulong ulonga = 42; ulong ulongb = 43;
            bool boola = true; bool boolb = false;
            short shorta = 42; short shortb = 43;
            ushort ushorta = 42; ushort ushortb = 43;
            float floata = 42; float floatb = 43;
            int[] intArraya = new int[5] { 1, 2, 3, 4, 5 };
            int[] intArrayb = new int[5] { 1, 2, 3, 4, 6 };
            int[] intArraybis = new int[4] { 1, 2, 3, 4 };
            int[] intArrayter = null;
            object obja = new(); object objb = new();
            string stra = "42"; string strb = "43";

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
        public void TestStringComparison()
        {
            Debug.WriteLine("Test string, Contains, EndsWith, StartWith");

            // Arrange
            string tocontains = "this text contains and end with contains";
            string startcontains = "contains start this text";
            string contains = "contains";
            string doesnotcontains = "this is totally something else";
            string empty = string.Empty;
            string stringnull = null;

            // Assert
            Assert.Contains(contains, tocontains);
            Assert.DoesNotContains(contains, doesnotcontains);
            Assert.DoesNotContains(contains, empty);
            Assert.AreNotSame(contains, stringnull);
            Assert.StartsWith(contains, startcontains);
            Assert.EndsWith(contains, tocontains);
        }
    }
}
