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

using Autohash;
namespace AutoHasher.Test.Holders
{

  class SimpleValuesHolder : Holder
  {
    internal char c;
    internal int i;
    internal uint ui;
    internal bool b;
    internal long l;
    internal ulong ul;
    internal double d;
    internal float f;
    internal short s;
    internal byte by;

    public override int FasterGetHashCode()
    {
      return Cache<SimpleValuesHolder>.Get()(this);
    }


    public override int ExpectedFromCodegen()
    {
      int hashcode = 0;
      hashcode = (hashcode * 397) ^ (65537 * (int)this.c);
      hashcode = (hashcode * 397) ^ (int)this.i;
      hashcode = (hashcode * 397) ^ (int)this.ui;
      hashcode = (hashcode * 397) ^ (this.b ? 1 : 0);
      hashcode = (hashcode * 397) ^ (int)this.l;
      hashcode = (hashcode * 397) ^ (int)this.ul;
      hashcode = (hashcode * 397) ^ (int)this.d;
      hashcode = (hashcode * 397) ^ (int)this.f;
      hashcode = (hashcode * 397) ^ (int)this.s;
      hashcode = (hashcode * 397) ^ (int)this.@by;

      return hashcode;
    }

    public int StandardHashCode()
    {
      int hashcode = 0;
      hashcode = (hashcode * 397) ^ this.c.GetHashCode();
      hashcode = (hashcode * 397) ^ this.i.GetHashCode();
      hashcode = (hashcode * 397) ^ this.ui.GetHashCode();
      hashcode = (hashcode * 397) ^ this.b.GetHashCode();
      hashcode = (hashcode * 397) ^ this.l.GetHashCode();
      hashcode = (hashcode * 397) ^ this.ul.GetHashCode();
      hashcode = (hashcode * 397) ^ this.d.GetHashCode();
      hashcode = (hashcode * 397) ^ this.f.GetHashCode();
      hashcode = (hashcode * 397) ^ this.s.GetHashCode();
      hashcode = (hashcode * 397) ^ this.@by.GetHashCode();

      return hashcode;
    }


    internal enum MyEnum
    {
      a = 234235,
      b = 34535343
    }
  }
}