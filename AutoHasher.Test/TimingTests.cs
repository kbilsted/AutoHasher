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
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using AutoHasher.Test.Holders;
  using AutoHasher.Test.IntegrationTests;
  using AutoHasher.Test.Util;

  using NUnit.Framework;

  [TestFixture]
  internal class TimingTests
  {
    const int N = 200000;
    Random rnd;

    [SetUp]
    public void Setup()
    {
      rnd = new Random(122);
    }

    private List<T> CreateCollection<T>(int noItems, Func<T> creatorFunc) where T : Holder
    {
      var res = new List<T>();
      for (int i = 0; i < noItems; i++)
      {
        res.Add(creatorFunc());
      }

      return res;
    }

    int DoHashing<T>(List<T> c) where T : Holder
    {
      unchecked
      {
        int checksum = 0;
        foreach (var obj in c)
        {
          checksum += obj.GetHashCode();
        }
        return checksum;
      }
    }

    int DoFasterHashing<T>(List<T> c) where T : Holder
    {
      unchecked
      {
        int checksum = 0;
        foreach (var obj in c)
        {
          checksum += obj.FasterGetHashCode();
        }
        return checksum;
      }
    }
    
    int DoHandmadeHashing<T>(List<T> c) where T : Holder
    {
      unchecked
      {
        int checksum = 0;
        foreach (var obj in c)
        {
          checksum += obj.ExpectedFromCodegen();
        }
        return checksum;
      }
    }

    /// <summary>
    /// large object to hash
    /// </summary>
    [Test]
    public void RuntimeCodeGen_allValues()
    {
      var collection = CreateCollection(N, () => new AllValues(rnd));

      TimingCode(collection);
    }

    // placed here to prevent JIT inlining
    int checksum = 0;
    int fasterchecksum = 0;
    int checksumHandcoded = 0;


    [Test]
    /// <summary>
    /// small object to hash
    /// </summary>
    public void RuntimeCodeGen_enums()
    {
      var collection = CreateCollection(N, () => new EnumHolder());

      TimingCode(collection);
    }

    void TimingCode<T>(List<T> collection) where T : Holder
    {
      // codegenerate outside timing
      collection.First().GetHashCode();
      collection.First().ExpectedFromCodegen();
      collection.First().FasterGetHashCode();

      for (int i = 0; i < 10; i++)
      {
        var time = Timer.TimeInMillis(() => { checksum = DoHashing(collection); });
        Console.Write("auto.hash ** {0} millis  {1}   ", time, checksum);

        time = Timer.TimeInMillis(() => { fasterchecksum = DoFasterHashing(collection); });
        Console.Write("fast.auto ** {0} millis  {1}   ", time, fasterchecksum);

        time = Timer.TimeInMillis(() => { checksumHandcoded = DoHandmadeHashing(collection); });

        Console.WriteLine("hand.code ** {0} millis {1}   ", time, checksumHandcoded);
        Assert.AreEqual(checksumHandcoded, fasterchecksum);
      }
    }
  }
}
