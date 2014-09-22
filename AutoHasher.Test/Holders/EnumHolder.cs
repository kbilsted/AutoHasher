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

namespace AutoHash.Test.Holders
{
  class EnumHolder : Holder
  {
    internal MyEnum Enum1;
    internal MyEnum? Enum2;

    public EnumHolder() : base()
    {
      this.Enum1 = MyEnum.a;
    }

    public override int FasterGetHashCode()
    {
     return Cache<EnumHolder>.Get()(this);
    }

    public override int ExpectedFromCodegen()
    {
      unchecked
      {
        var hashCode = 0;
        hashCode = (hashCode * 397) ^ Enum1.GetHashCode();
        hashCode = (hashCode * 397) ^ Enum2.GetHashCode();
        return hashCode;
      }
    }

    internal enum MyEnum
    {
      a = 234235, b = 34535343
    }
  }
}