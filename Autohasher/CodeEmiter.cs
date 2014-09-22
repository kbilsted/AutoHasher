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
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoHash
{
  /// <summary>
  /// Emit code for each field of a type.
  /// </summary>
  class CodeEmiter
  {
    DebugStats debugStats;
    bool optimizeMultOnFirstAssign = true;

    /// <summary>
    /// Ctor
    /// </summary>
    public CodeEmiter(DebugStats debugStats)
    {
      this.debugStats = debugStats;
    }

    /// <summary>
    /// Given a type, generate expressions and add it to the <paramref name="exps"/>
    /// </summary>
    /// <param name="t"></param>
    /// <param name="hashCode"></param>
    /// <param name="castedObj"></param>
    /// <param name="field"></param>
    /// <param name="exps"></param>
    public void GenerateForField(
      ParameterExpression hashCode,
      ParameterExpression castedObj,
      FieldInfo field,
      List<Expression> exps)
    {
      var txt = Generate(hashCode, castedObj, field, exps);

      if (Compiler.Logging)
      {
        System.Diagnostics.Debug.WriteLine("{0}: {1}:{2}", txt, field.FieldType, field.Name);
      }
    }

    string Generate(
      ParameterExpression hashCode,
      ParameterExpression castedObj,
      FieldInfo field,
      List<Expression> exps)
    {
      Type t = field.FieldType;

      if (typeof(ICollection).IsAssignableFrom(t))
      {
        HandleICollection(hashCode, castedObj, field, exps);
        return "Field";
      }

      if (t == typeof(Nullable)
          || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)))
      {
        HandleNullables(t, hashCode, castedObj, field, exps);
        return "Field?";
      }

      if (t.IsEnum) // non-nullable
      {
        HandleEnumsAndPrimitives(t, hashCode, castedObj, field, exps);
        return "Field-enum";
      }

      if (t.IsPrimitive) // non-nullable
      {
        HandleEnumsAndPrimitives(t, hashCode, castedObj, field, exps);
        return "Field";
      }

      if (typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string))
      {
        HandleIEnumerables(field);
        return "IGNORING Field";
      }

      {
        HandleComplex(t, hashCode, castedObj, field, exps);
        return "Complex Field";
      }
    }

    void HandleComplex(Type t, ParameterExpression hashCode, ParameterExpression castedObj, FieldInfo field, List<Expression> exps)
    {
      debugStats.StatsComplex++;

      // hashCode = (hashCode * 397) ^ x.field == null ? 0 : x.field.GetHashCode()
      exps.Add(
        Expression.IfThenElse(
          Expression.ReferenceEqual(Expression.Constant(null), Expression.Field(castedObj, field)),
          MultXor(hashCode, Expression.Constant(0)),
          CallNativeGetHashCodeAndMultXor(t, hashCode, castedObj, field)));
    }

    void HandleIEnumerables(FieldInfo field)
    {
      // skip
      debugStats.StatsIgnored++;
    }

    void HandleNullables(Type t, ParameterExpression hashCode, ParameterExpression castedObj, FieldInfo field, List<Expression> exps)
    {
      debugStats.StatsNullable++;

      exps.Add(CallNativeGetHashCodeAndMultXor(t, hashCode, castedObj, field));
    }

    void HandleEnumsAndPrimitives(Type t, ParameterExpression hashCode, ParameterExpression castedObj, FieldInfo field, List<Expression> exps)
    {
      // Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
      if (t.IsPrimitive)
        debugStats.statsPrimitive++;

      if (t.IsEnum)
      {
        debugStats.StatsEnum++;

        if (Enum.GetUnderlyingType(t) != typeof(int))
          throw new NotImplementedException("Only int enums at the moment");
        
        exps.Add(MultXor(hashCode, Expression.Convert( Expression.Field(castedObj, field), typeof(int))));
        return;
      }

      exps.Add(CallNativeGetHashCodeAndMultXor(t, hashCode, castedObj, field));
    }

    void HandleICollection(ParameterExpression hashCode, ParameterExpression castedObj, FieldInfo field, List<Expression> exps)
    {
      debugStats.StatsIcollection++;
      // generated code

      // object valueOfField = x.field
      // if(valueOfField == null)
      //   goto end;
      //
      // hashCode = (hashCode * 397) ^ valueOfField.Count
      // label end;
      var innerExprs = new List<Expression>();
      var refIsNull = Expression.Label("RefIsNull");

      var valueOfField = Expression.Variable(typeof(ICollection), "tmpfield");
      innerExprs.Add(Expression.Assign(valueOfField, Expression.Field(castedObj, field)));
      innerExprs.Add(
        Expression.IfThen(
          Expression.ReferenceEqual(Expression.Constant(null), valueOfField),
          Expression.Goto(refIsNull)));

      // not-null value hash the count of the collection
      // TODO like char - we should mult the count with 65xxx
      var count = Expression.MakeMemberAccess(valueOfField, typeof(ICollection).GetProperty("Count"));
      innerExprs.Add(MultXor(hashCode, count));

      // end of the inner block
      innerExprs.Add(Expression.Label(refIsNull));
      exps.Add(Expression.Block(new[] { valueOfField }, innerExprs));
    }

    BinaryExpression CallNativeGetHashCodeAndMultXor(Type t, ParameterExpression hashCode, ParameterExpression castedObj, FieldInfo field)
    {
      var fieldValue = Expression.Field(castedObj, field);
      var methodInfo = IsNotNull(t.GetMethod("GetHashCode"));

      return MultXor(hashCode, Expression.Call(fieldValue, methodInfo));
    }

    BinaryExpression MultXor(ParameterExpression hashCode, Expression expr)
    {
      // optimizing the first assignment to hashcode is not faster
      // a bit odd, but it must be the jitter. Comment left in order to remember
      // not to retry this strategy.
      if (optimizeMultOnFirstAssign)
      {
        var res = Expression.Assign(hashCode, expr);
        optimizeMultOnFirstAssign = false;
        return res;
      }

      var mult397 = Expression.Multiply(hashCode, Expression.Constant(397));
      var lhs = Expression.ExclusiveOr(mult397, expr);
      var assign = Expression.Assign(hashCode, lhs);

      return assign;
    }


    MethodInfo IsNotNull(MethodInfo lookup)
    {
      if (lookup == null)
        throw new Exception("Cannot find hashhelpermethod");
      return lookup;
    }
  }
}