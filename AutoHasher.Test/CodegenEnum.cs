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

namespace AutoHasher.Test
{
  using Autohash;

  using AutoHasher.Test.Holders;
  using AutoHasher.Test.Util;

  using NUnit.Framework;

  [TestFixture]
  class CompilerEnumTest : CompilerTest
  {
    [Test]
    public void Empty()
    {
      Compiler.Logging = true;
      var sut = new EnumHolder();

      int expected = sut.ExpectedFromCodegen();
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(1, debugStats.StatsEnum);
      Assert.AreEqual(0, debugStats.statsPrimitive);
      Assert.AreEqual(0, debugStats.StatsIcollection);
    }


    [Test]
    public void Value()
    {
      var sut = new EnumHolder();
      sut.Enum1 = EnumHolder.MyEnum.b;

      int expected = sut.ExpectedFromCodegen();
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(0, debugStats.statsPrimitive);
      Assert.AreEqual(1, debugStats.StatsEnum);
    }


    [Test]
    public void ValuesInBoth()
    {
      var sut = new EnumHolder();
      sut.Enum1 = EnumHolder.MyEnum.b;
      sut.Enum2 = EnumHolder.MyEnum.a;

      int expected = sut.ExpectedFromCodegen();
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(1, debugStats.StatsEnum);
      Assert.AreEqual(0, debugStats.statsPrimitive);
    }

  }
}