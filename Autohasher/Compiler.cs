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

namespace Autohash
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Reflection;

  using Autohash.Attributes;

  internal delegate int HashingMethod(object source);

  class Compiler
  {
    public static bool Logging = false;

    internal DebugStats stats = new DebugStats();

    public HashingMethod CreateHashingMethod<T>()
    {
      return CreateHashingMethod(typeof(T));
    }

    public HashingMethod CreateHashingMethod(Type sourceType)
    {
      // argument must be of type object
      // Step 2: Cast argument to correct type.
      var objArg = Expression.Parameter(typeof(object));
      var castedObj = Expression.Variable(sourceType, "casted");
      var hashCode = Expression.Variable(typeof(int), "hashCode");
      var vars = new List<ParameterExpression> { castedObj, hashCode };
      var exps = new List<Expression>
                 {
                   // casted = (Foo) source -- downcast to the actual type
                   Expression.Assign(castedObj, Expression.Convert(objArg, sourceType)),
                 };

      // Step 3: Field code.
      var fields = HarvestFields(sourceType);
      var emitor = new CodeEmiter(stats);
      foreach (var field in fields)
      {
        emitor.GenerateForField(hashCode, castedObj, field, exps);
      }

      // return hashCode
      exps.Add(hashCode);

      var methodBody = Expression.Block(vars, exps);

      return Expression.Lambda<HashingMethod>(methodBody, objArg).Compile();
    }

    
    FieldInfo[] HarvestFields(Type sourceType)
    {
      var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
      var allFields = sourceType.GetFields(flags);
      return allFields
        .Where(x => !x.GetCustomAttributes(typeof(DontHashAttribute), false).Any())
        .ToArray();
    }
  }
}

