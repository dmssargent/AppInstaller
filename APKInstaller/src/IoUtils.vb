Imports System.IO
Imports ICSharpCode.SharpZipLib.Core
Imports ICSharpCode.SharpZipLib.Zip

''' <summary>
''' Provides utilities for performing various common IO tasks
''' </summary>
Public Class IoUtilities : Implements IDisposable
    ''' <summary>
    ''' Unzips a file from a file stream into a folder
    ''' </summary>
    ''' <param name="zipStream">the stream from a zip file</param>
    ''' <param name="outFolder">the path of the destination directory</param>
    Public Shared Sub UnzipFromStream(zipStream As Stream, outFolder As String)
        Dim zipInputStream = New ZipInputStream(zipStream)
        Dim zipEntry = zipInputStream.GetNextEntry()
        Dim buffer(4096) As Byte ' 4K Is optimum
        While (zipEntry IsNot Nothing)
            Dim entryFileName = zipEntry.Name
            ' Convert UNIX paths to the current platform path
            entryFileName = entryFileName.Replace("/", Path.DirectorySeparatorChar)
            Dim fullZipToPath = Path.Combine(outFolder, entryFileName)
            Dim directoryName = Path.GetDirectoryName(fullZipToPath)

            If (directoryName.Length > 0) Then
                Directory.CreateDirectory(directoryName)
            End If
            If Not (directoryName & Path.DirectorySeparatorChar).Equals(fullZipToPath) Then
                Using streamWriter As FileStream = File.Create(fullZipToPath)
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer)
                End Using
            End If


            zipEntry = zipInputStream.GetNextEntry()
        End While
    End Sub
#Region "IDisposable Support"
    Private Shared _instance As New IoUtilities
    Private _isReady As Boolean = False
    ' Private tempFile As File
    Private _sessions As New List(Of String)
    Private _tempFilePath As String

    ''' <summary>
    ''' Prepares the IOUtils to do operations
    ''' </summary>
    Public Shared Sub Prepare()
        If (_instance IsNot Nothing) Then
            _instance._Prepare()
        End If

    End Sub

    Private Sub _Prepare()
        If _isReady Then
            Exit Sub
        End If

        _tempFilePath = Path.GetTempFileName()
        File.Delete(_tempFilePath)
        Directory.CreateDirectory(_tempFilePath)

        _isReady = True
    End Sub

    ''' <summary>
    ''' Creates a new temp session, and returns the path to that temp session
    ''' </summary>
    ''' <param name="name">the name of the temp session</param>
    ''' <returns>the path of the temp session</returns>
    Public Shared Function CreateTempSession(name As String) As String
        Prepare()
        If name Is Nothing Then
            Throw New ArgumentNullException(NameOf(name))
        End If

        Dim session = Path.Combine(_instance._tempFilePath, name)
        If (_instance._sessions.Exists(New Predicate(Of String)(
                                     Function(test As String) As Boolean
                                         Return name.Equals(test)
                                     End Function))) Then
            Return session
        End If

        If (File.Exists(session)) Then
            File.Delete(session)
        End If
        Directory.CreateDirectory(session)
        Return session
    End Function

    ''' <summary>
    ''' Disposes of the current IO Utilities
    ''' </summary>
    Public Shared Sub Cleanup()
        _instance.Dispose()
    End Sub

    ''' <summary>
    ''' Removes a specified temp session
    ''' </summary>
    ''' <param name="name">the name of a previous created temp session</param>
    Public Shared Sub RemoveTempSession(name As String)

        If name Is Nothing Then
            Throw New ArgumentNullException(NameOf(name))
        End If

        Dim session = Path.Combine(_instance._tempFilePath, name)
        If (_instance._sessions.Exists(New Predicate(Of String)(
                                     Function(test As String) As Boolean
                                         Return name.Equals(test)
                                     End Function))) Then
            If (File.Exists(session)) Then
                File.Delete(session)
            End If
        End If

        Throw New ArgumentException("Session """ & name & """ doesn't currently exist")
    End Sub


    Private _disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposedValue Then
            Dim temp = _tempFilePath
            If disposing Then
                _isReady = False
                _instance = Nothing

                _sessions.Clear()
                _sessions = Nothing
                _tempFilePath = Nothing
            End If

            If (File.Exists(temp)) Then
                If (Directory.Exists(temp)) Then
                    Directory.Delete(temp)
                Else
                    File.Delete(temp)
                End If
            End If
        End If
        _disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub


    Public Sub Dispose() Implements IDisposable.Dispose
        GC.SuppressFinalize(Me)
        Dispose(True)
    End Sub
#End Region
End Class
