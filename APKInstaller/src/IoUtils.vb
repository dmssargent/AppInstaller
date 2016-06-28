Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip

''' <summary>
''' Provides utilities for performing various common IO tasks
''' </summary>
Public Class IOUtilities : Implements IDisposable
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
                Using _streamWriter As FileStream = File.Create(fullZipToPath)
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipInputStream, _streamWriter, buffer)
                End Using
            End If


            zipEntry = zipInputStream.GetNextEntry()
        End While
    End Sub
#Region "IDisposable Support"
    Private Shared instance As New IOUtilities
    Private isReady As Boolean = False
    ' Private tempFile As File
    Private sessions As New List(Of String)
    Private tempFilePath As String

    ''' <summary>
    ''' Prepares the IOUtils to do operations
    ''' </summary>
    Public Shared Sub Prepare()
        If (instance IsNot Nothing) Then
            instance._Prepare()
        End If

    End Sub

    Private Sub _Prepare()
        If isReady Then
            Exit Sub
        End If

        tempFilePath = Path.GetTempFileName()
        File.Delete(tempFilePath)
        Directory.CreateDirectory(tempFilePath)

        isReady = True
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

        Dim session = Path.Combine(instance.tempFilePath, name)
        If (instance.sessions.Exists(New Predicate(Of String)(
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
        instance.Dispose()
    End Sub

    ''' <summary>
    ''' Removes a specified temp session
    ''' </summary>
    ''' <param name="name">the name of a previous created temp session</param>
    Public Shared Sub RemoveTempSession(name As String)

        If name Is Nothing Then
            Throw New ArgumentNullException(NameOf(name))
        End If

        Dim session = Path.Combine(instance.tempFilePath, name)
        If (instance.sessions.Exists(New Predicate(Of String)(
                                     Function(test As String) As Boolean
                                         Return name.Equals(test)
                                     End Function))) Then
            If (File.Exists(session)) Then
                File.Delete(session)
            End If
        End If

        Throw New ArgumentException("Session """ & name & """ doesn't currently exist")
    End Sub


    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            Dim temp = tempFilePath
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                isReady = False
                instance = Nothing

                'tempFile = Nothing
                sessions.Clear()
                sessions = Nothing
                tempFilePath = Nothing
            End If

            If (File.Exists(temp)) Then
                If (Directory.Exists(temp)) Then
                    Directory.Delete(temp)
                Else
                    File.Delete(temp)
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
    End Sub
#End Region
End Class
