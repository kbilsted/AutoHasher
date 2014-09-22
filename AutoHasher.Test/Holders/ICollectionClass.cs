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

namespace AutoHasher.Test.Holders
{
  using System.Collections.Generic;

  using Autohash;

  class ICollectionClass : Holder
  {
    public List<int> IColl1, IColl2;

    public string[] StringArr;


    public override int FasterGetHashCode()
    {
      return Cache<ICollectionClass>.Get()(this);
    }

    public override int ExpectedFromCodegen()
    {
      int hashcode = 0;
      if (this.IColl1 != null) hashcode = (hashcode * 397) ^ this.IColl1.Count;
      if (this.IColl2 != null) hashcode = (hashcode * 397) ^ this.IColl2.Count;
      if (this.StringArr != null) hashcode = (hashcode * 397) ^ this.StringArr.Length;

      return hashcode;
    }
  }
}