﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;
using static CodeWars.Unit16;

namespace CodeWars_Test
{
    [TestClass]
    public class UnitTest16
    {
        [TestMethod]
        public void Adam_is_unique()
        {
            Adam adam = Adam.GetInstance();
            Adam anotherAdam = Adam.GetInstance();

            Assert.IsTrue(adam is Adam);
            Assert.AreEqual(adam, anotherAdam);
        }

        [TestMethod]
        public void Adam_is_unique_and_only_GetInstance_can_return_adam()
        {
            // GetInstance() is the only static method on Adam
            Assert.AreEqual(1, typeof(Adam).GetMethods().Where(x => x.IsStatic).Count());

            // Adam does not have public or internal constructors
            Assert.IsFalse(typeof(Adam).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              .Any(x => x.IsPublic || x.IsAssembly));
        }

        [TestMethod]
        public void Adam_is_unique_and_cannot_be_overriden()
        {
            Assert.IsTrue(typeof(Adam).IsSealed);
        }
    
        [TestMethod]
        public void Adam_is_a_human()
        {
            Assert.IsTrue(Adam.GetInstance() is Human); 
        }

        [TestMethod]
        public void Adam_is_a_male()
        { 
            Assert.IsTrue(Adam.GetInstance() is Male);
        }

        [TestMethod]
        public void Eve_is_unique_and_created_from_a_rib_of_adam()
        {
            Adam adam = Adam.GetInstance();
            Eve eve = Eve.GetInstance(adam);
            Eve anotherEve = Eve.GetInstance(adam);
        
            Assert.IsTrue(eve is Eve);
            Assert.AreEqual(eve, anotherEve);

            // GetInstance() is the only static method on Eve
            Assert.AreEqual(1, typeof(Eve).GetMethods().Where(x => x.IsStatic).Count());

            // Eve has no public or internal constructor
            Assert.IsFalse(typeof(Eve).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              .Any(x => x.IsPublic || x.IsAssembly));
        
            // Eve cannot be overridden
            Assert.IsTrue(typeof(Eve).IsSealed);
        }
    
        [TestMethod]
        public void Eve_can_only_be_create_of_a_rib_of_adam()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Eve.GetInstance(null));
        }

        [TestMethod]
        public void Eve_is_a_human()
        {
            Assert.IsTrue(Eve.GetInstance(Adam.GetInstance()) is Human);
        }

        [TestMethod]
        public void Eve_is_a_female()
        {
            Assert.IsTrue(Eve.GetInstance(Adam.GetInstance()) is Female);
        }
 
        [TestMethod]
        public void Reproduction_always_result_in_a_male_or_female()
        {
            Assert.IsTrue(typeof(Human).IsAbstract);
        }
    
        [TestMethod]
        public void Humans_can_reproduce_when_there_is_a_name_a_mother_and_a_father()
        {
            var adam = Adam.GetInstance();
            var eve = Eve.GetInstance(adam);
            var seth = new Male("Seth", eve, adam);
            var azura = new Female("Azura", eve, adam);
            var enos = new Male("Enos", azura, seth);

            Assert.AreEqual("Eve", eve.Name);
            Assert.AreEqual("Adam", adam.Name);
            Assert.AreEqual("Seth", seth.Name);
            Assert.AreEqual("Azura", azura.Name);
            Assert.AreEqual("Enos", ((Human)enos).Name);
            Assert.AreEqual(seth, ((Human)enos).Father);
            Assert.AreEqual(azura, ((Human)enos).Mother);
        }
    
        [TestMethod]
        public void Father_and_mother_are_essential_for_reproduction()
        {
            // There is just 1 way to reproduce 
            Assert.AreEqual(1, typeof(Male).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              .Where(x => x.IsPublic || x.IsAssembly).Count());
            Assert.AreEqual(1, typeof(Female).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
              Where(x => x.IsPublic || x.IsAssembly).Count());
        
            var adam = Adam.GetInstance();
            var eve = Eve.GetInstance(adam);
            Assert.ThrowsException<ArgumentNullException>(() => new Male("Seth", null, null));
            Assert.ThrowsException<ArgumentNullException>(()=> new Male("Abel", eve, null));
            Assert.ThrowsException<ArgumentNullException>(() => new Male("Seth", null, adam));
            Assert.ThrowsException<ArgumentNullException>(() => new Female("Azura", null, null));
            Assert.ThrowsException<ArgumentNullException>(() => new Female("Awan", eve, null));
            Assert.ThrowsException<ArgumentNullException>(() => new Female("Dina", null, adam));
            Assert.ThrowsException<ArgumentNullException>(() => new Female("Eve", null, null));
            Assert.ThrowsException<ArgumentNullException>(() => new Male("Adam", null, null));
        }
    }
}
