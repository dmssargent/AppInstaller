﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System
Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Globalization
Imports System.Resources
Imports System.Runtime.CompilerServices

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     DebuggerNonUserCode(),  _
     CompilerGenerated(),  _
     HideModuleName()>  _
    Friend Module Resources
        
        Private resourceMan As ResourceManager
        
        Private resourceCulture As CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <EditorBrowsable(EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As ResourceManager
            Get
                If ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As ResourceManager = New ResourceManager("APKInstaller.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <EditorBrowsable(EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Byte[].
        '''</summary>
        Friend ReadOnly Property aapt_23_0_3_win() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("aapt_23_0_3_win", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        '''</summary>
        Friend ReadOnly Property android_app() As Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("android_app", resourceCulture)
                Return CType(obj,Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property FIRSTTech_IconVert_RGB() As Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("FIRSTTech_IconVert_RGB", resourceCulture)
                Return CType(obj,Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Byte[].
        '''</summary>
        Friend ReadOnly Property platform_tools_r24_windows() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("platform_tools_r24_windows", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
    End Module
End Namespace
