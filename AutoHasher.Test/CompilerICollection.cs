// Copyright 2014 Kasper B. Graversen
// 
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System.Collections.Generic;

using AutoHash.Test.Holders;
using AutoHash.Test.Util;

using NUnit.Framework;

namespace AutoHash.Test
{
  [TestFixture]
  internal class CompilerICollection : CompilerTest
  {
    private const int numFieldsTouched = 3;

    [Test]
    public void Null()
    {
      var sut = new ICollectionClass();

      int expected = sut.ExpectedFromCodegen();

      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(numFieldsTouched, debugStats.StatsIcollection);
    }

    [Test]
    public void Empty()
    {
      var sut = new ICollectionClass();
      sut.IColl1 = new List<int>();

      int expected = sut.ExpectedFromCodegen();
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(numFieldsTouched, debugStats.StatsIcollection);
    }


    [Test]
    public void Elements()
    {
      var sut = new ICollectionClass();
      sut.IColl1 = new List<int>() { 1, 2, 3 };

      int expected = sut.ExpectedFromCodegen();
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(numFieldsTouched, debugStats.StatsIcollection);
    }


    [Test]
    public void ElementsInBoth()
    {
      var sut = new ICollectionClass();
      sut.IColl1 = new List<int>() { 1, 2, 3 };
      sut.IColl2 = new List<int>() { 1, 2, 3 };

      int expected = sut.ExpectedFromCodegen();
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(numFieldsTouched, debugStats.StatsIcollection);
    }

    [Test]
    public void ArrayIs()
    {
      var sut = new ICollectionClass();
      sut.IColl1 = new List<int>() { 1, 2, 3 };
      sut.IColl2 = new List<int>() { 1, 2, 3 };
      sut.StringArr = new[] { "a", "b" };

      int expected = sut.ExpectedFromCodegen();
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(numFieldsTouched, debugStats.StatsIcollection);
    }
  }
}