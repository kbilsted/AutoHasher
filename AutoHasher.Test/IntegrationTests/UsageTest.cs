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

using Autohash;
using Autohash.Attributes;

using NUnit.Framework;

namespace AutoHash.Test.IntegrationTests
{
  class Foo
  {
    internal string field1 = "foo";
    internal int bar = 42;
    internal List<int> l1 = null;
    internal List<int> l2 = new List<int>() { 0, 0 };
    [DontHash]
    internal string nonHashed = "boo";
    
    public override int GetHashCode()
    {
      return AutoHasher.GetHashCode(this);
    }
  }


  [TestFixture]
  class UsageTest
  {
    [Test]
    public void TestUsage()
    {
      var foo = new Foo();
      int expected = 0;
      expected = (expected * 397) ^ foo.field1.GetHashCode();
      expected = (expected * 397) ^ foo.bar.GetHashCode();
      expected = (expected * 397) ^ foo.l2.Count.GetHashCode();
 
      int actual = foo.GetHashCode();
      
      Assert.AreEqual(expected, actual);
    }
  }
}
