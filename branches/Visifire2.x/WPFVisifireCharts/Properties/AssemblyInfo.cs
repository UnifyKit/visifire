using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WPFVisifireCharts")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Webyog Softworks Pvt. Ltd.")]
[assembly: AssemblyProduct("WPFVisifire.Charts")]
[assembly: AssemblyCopyright("Copyright ©  2008")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AllowPartiallyTrustedCallers()]
[assembly: CLSCompliant(false)]

// Provide unit tests with access to internal members
//#if DEBUG
[assembly: InternalsVisibleTo("WPFVisifireChartsTest, PublicKey=00240000048000009400000006020000002400005253413100040000010001002b67603481f2853a6f79026ee596e8d1a930361e685a57005fe7ab256016e3f717ccbfd964a7526fa6d0667ae72c9ecc59a260add8fbc4d4b20a8b53297ee28427b2d571c4db86af95541c997ac2f9a54c0f5e7957909e83ed8ca6a3fdd7acd2552b6c3e50afa14ca190bacf716ebcfbe022c727f894f01c5b746a8530e556d5")]
//#else
//[assembly: InternalsVisibleTo("WPFVisifireChartsTest, PublicKey=0024000004800000940000000602000000240000525341310004000001000100fda5bc808cff5b2def409a022b8adaf5df81dd90cdc2da3208b6df4abc9c9513dd56422696e94716df03f1481cef3422e9351b6765bdea9ec1ef445b04b0e6c9f52488195e3d812e16014f560041486d40dd44164301852d69229e6331c0f5ddc125fea12fe1397e5dd6dbdeeabf54d5685c847221d5361391675cad0a816b9f")]
//#endif

//// The following GUID is for the ID of the typelib if this project is exposed to COM
//[assembly: Guid("ffa9c75a-8462-4ed9-ba38-882b3d3b86ab")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]


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
[assembly: AssemblyVersion("2.2.1.0")]
[assembly: AssemblyFileVersion("2.2.1.0")]
