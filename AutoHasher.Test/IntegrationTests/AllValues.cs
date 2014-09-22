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

using AutoHash.Test.Holders;

namespace AutoHash.Test.IntegrationTests
{
  using System;
  using System.Collections.Generic;

  using Autohash;

  class AllValues : Holder
  {
    public int i1, i2, i3, i4, i5, i6, i7;
    protected int? i8, i9;
    
    protected string s1;
    protected string s2;
    protected string s3;

    protected float f1;
    protected float? f2, f3;

    private IEnumerable<int> ie1, ie2;
 
    internal AllValues(Random rnd) : base()
    {
      this.i1 = rnd.Next();
      this.i2 = rnd.Next();
      this.i3 = rnd.Next();
      this.i4 = rnd.Next();
      this.i5 = rnd.Next();
      this.i6 = rnd.Next();
      this.i7 = rnd.Next();
      this.i8 = null;
      this.i9 = rnd.Next();
      this.s1 = null;
      this.s2 = "" + rnd.Next() + ":" + rnd.Next();
      this.s3 = "" + rnd.Next() + ":" + rnd.Next() + ":" + rnd.Next() + ":" + rnd.Next() + ":" + rnd.Next() + ":" + rnd.Next();
      this.f1 = (float) rnd.NextDouble();
      this.f2 = null;
      this.f3 = (float) rnd.NextDouble();
      this.ie1 = null;
      this.ie2 = new[] {1, 23};
    }

    public override int FasterGetHashCode()
    {
      return Cache<AllValues>.Get()(this);
    }

    public override int ExpectedFromCodegen()
    {
      unchecked
      {
        var hashCode = this.i1;
        hashCode = (hashCode*397) ^ this.i2;
        hashCode = (hashCode*397) ^ this.i3;
        hashCode = (hashCode*397) ^ this.i4;
        hashCode = (hashCode*397) ^ this.i5;
        hashCode = (hashCode*397) ^ this.i6;
        hashCode = (hashCode*397) ^ this.i7;
        hashCode = (hashCode * 397) ^ this.i8.GetHashCode();
        hashCode = (hashCode * 397) ^ this.i9.GetHashCode();
        hashCode = (hashCode * 397) ^ (this.s1 != null ? this.s1.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (this.s2 != null ? this.s2.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (this.s3 != null ? this.s3.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ this.f1.GetHashCode();
        hashCode = (hashCode * 397) ^ (this.f2 != null ? this.f2.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (this.f3 != null ? this.f3.GetHashCode() : 0);

        return hashCode;
      }
    }
  }
}