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

using System;

using AutoHash;

using AutoHash.Test.Holders;
using AutoHash.Test.Util;

using NUnit.Framework;

namespace AutoHash.Test
{
  [TestFixture]
  class CompilerPrimitivesTest : CompilerTest
  {
    // must be here to avoid inlining
    int x = 0;


    [Test]
    public void BoolValue()
    {
      var sut = new SimpleValuesHolder();

      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      
      sut.b = true;
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
    }

    [Test]
    public void NoValues()
    {
      var sut = new SimpleValuesHolder();
      int expected = sut.ExpectedFromCodegen();
      
      Assert.AreEqual(expected, sut.GetHash(compiler));
      Assert.AreEqual(10, debugStats.statsPrimitive, "fields compiled");
      Assert.AreEqual(0, debugStats.StatsEnum);
      Assert.AreEqual(0, debugStats.StatsIcollection);
    }


    [Explicit("currently we are not standard")]
    [Test]
    public void AreWeGeneratingAsStandardGetHashcode()
    {
      var sut = new SimpleValuesHolder();
      this.StandardTesting(sut);
    }

    //[Test]
    //public void AreWeGeneratingAsStandardGetHashcodeWithStd()
    //{
    //  compiler.should_inline_hash_for_primitives = false; // use .GetHashCode() on all input
    //  var sut = new SimpleValuesHolder(compiler);
    //  StandardTesting(sut);
    //}

    void StandardTesting(SimpleValuesHolder sut)
    {
      // TODO test alle felterne

      sut.c = 'c';
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      Assert.AreEqual(10, debugStats.statsPrimitive, "fields compiled");


      sut.i = 42433;
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.ui = 2983983;
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.b = true;
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.l = -23832232;
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.ul = 399992;
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.by = 3;
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));
      Console.WriteLine(sut.GetHash(compiler));
    }

    [Test]
    public void ValuesTiming()
    {
      Compiler.Logging = false;

      var N = 1000000;
      var sut = new SimpleValuesHolder();
      AssignValues(sut);

      // pre-compile before timing
      var mth = compiler.CreateHashingMethod<SimpleValuesHolder>();
      mth(sut);


      var time = Timer.TimeInMillis(() =>
                                          {
                                            for (int i = 0; i < N; i++)
                                            {
                                              x = mth(sut);
                                            }
                                          });
      Console.WriteLine("call gethashcode()");
      Console.WriteLine(time + "    " + x);

      time = Timer.TimeInMillis(() =>
                                          {
                                            for (int i = 0; i < N; i++)
                                            {
                                              x = sut.StandardHashCode();
                                            }
                                          });
        Console.WriteLine("std");
        Console.WriteLine(time + "    " + x);
      

      //  compiler = new Compiler(new Dictionary<Type, HashingMethod>());
      //  compiler.should_inline_hash_for_primitives = true;
      //  sut = new SimpleValuesHolder(compiler);
      //  // pre-compile before run
      //sut.GetHash(compiler);

      //time = Timer.TimeInMillis(() =>
      //                                {
      //                                  for (int i = 0; i < N; i++)

      //                                  {
      //                                    x = sut.GetHash(compiler);
      //                                  }
      //                                });
      //Console.WriteLine("inline calls");
      //Console.WriteLine(time + "    " + x);
    }

    void AssignValues(SimpleValuesHolder sut)
    {
      sut.c = 'c';
      sut.i = 42433;
      sut.ui = 2983983;
      sut.b = true;
      sut.l = -23832232;
      sut.ul = 399992;
      sut.by = 3;

    }

    [Explicit("since we at the moment generate different hash'es than the standard.")]
    [Test]
    public void Value()
    {
      var sut = new SimpleValuesHolder();

      sut.c = 'c';
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      Assert.AreEqual(10, debugStats.statsPrimitive, "fields compiled");


      sut.i = 42433;
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.ui = 2983983;
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.b = true;
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.l = -23832232;
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.ul = 399992;
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

      sut.by = 3;
      Assert.AreEqual(sut.ExpectedFromCodegen(), sut.GetHash(compiler));
      Assert.AreEqual(sut.StandardHashCode(), sut.GetHash(compiler));

    }

 
  }
}