# AutoHasher

[![Join the chat at https://gitter.im/kbilsted/AutoHasher](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/kbilsted/AutoHasher?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![Build status](https://ci.appveyor.com/api/projects/status/3ip6tk8i3s277474/branch/master?svg=true)](https://ci.appveyor.com/project/kbilsted/autohasher/branch/master)
[![Nuget](https://img.shields.io/nuget/dt/autohasher.svg)](http://nuget.org/packages/autohasher)
[![Nuget](https://img.shields.io/nuget/v/autohasher.svg)](http://nuget.org/packages/autohasher)
[![Nuget](https://img.shields.io/nuget/vpre/autohasher.svg)](http://nuget.org/packages/autohasher)


Automatic generation of GetHashCode() methods using on the fly run-time code generation.


# 1. Advantages

Using this framework is a real treat. Here is what you get:

* *Productivity!* You are guaranteed to generate a sound hash code. No more forgetting to update your hash code methods when adding or removing fields from your classes!
* This framework run-time generates code that is *as fast as hand coded implementations. Sometimes even faster* since we utilize some optimization tricks. 
* You are automatically ensured to follow *best practices*- such as calculating inside an 'unchecked' block.
* Automatic null handling.
* Use the same implementation everywhere. Set up your editor to automatically insert the GetHashCode() when creating new classes. 

# 2. Usage

### 1. Define a class

```C#
using AutoHash;
using AutoHash.Attributes;
class Foo
{
  internal string field1 = "foo";
  internal int bar = 42;
  internal List<int> l1 = null;
  internal List<int> l2 = new List<int>() { 0, 0 };

  [DontHash]
  internal string nonHashed = "boo";
}
```
  
### 2. Implement GetHashCode() the same way for all classes
    
```C#
public override int GetHashCode()
{
  return AutoHasher.GetHashCode(this);
}
```  

### 3. Check that it works

```C#
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
```

Notice that the field 'nonHashed' is not part of the hash code, and the collections are only participating with their length.


