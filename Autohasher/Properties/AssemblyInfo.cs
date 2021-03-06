﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Security;

[assembly: AssemblyTitle("Autohashing")]
[assembly: AssemblyDescription("Automatic GetHashCode generator")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Kasper B. Graversen")]
[assembly: AssemblyProduct("Autohashing")]
[assembly: AssemblyCopyright("Copyright © Kasper B. Graversen 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("702dda21-0de4-4a71-a16f-e8703a3760e0")]

[assembly: InternalsVisibleTo("AutoHasher.Test")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("")]















// speed up the code generation code  - from Stackoverflow

#if !NCRUNCH
[assembly: AllowPartiallyTrustedCallers]
#endif

[assembly: SecurityTransparent]
[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
