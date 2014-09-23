# AutoHasher


Automatic generation of GetHashCode() methods using on the fly run-time code generation.


# 1. Advantages

Using this framework is a real treat. Here is what you get:

* *Productivity!* You are guaranteed to generate a sound hash code. No more forgetting to update your hash code methods when adding or removing fieds from your classes!
* This framework run-time generates code thay is *as fast as hand coded implementations. Sometimes even faster* since we utilize som optimization tricks. 
* You are automatically ensured to following *best practices*- such as calculating inside an 'unchecked' block.
* Automatic null handling.
* Use the same implementation everywhere. Set up your editor to automatically insert the GetHashCode() when creating new classes. 

2. Usage
=========

1. Define a class

```C#
class Foo
{
  internal string field1 = "foo";
  internal string field1 = "foo";
  internal int bar = 42;
  internal List<int> l1 = null;
  internal List<int> l2 = new List<int>() { 0, 0 };

  [DontHash]
  internal string nonHashed = "boo";
}
```
  
2. Implement the GetHashCode() the same way for all classes
    
```C#
public override int GetHashCode()
{
  return AutoHasher.GetHashCode(this);
}
```  

3. Check that it works

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

Notice that the field 'nonHashed' is not part of the hash code, and the collections are only participating with they length.


