Public Enum ErrorCode
    ''' <summary>
    ''' The operation was a success
    ''' </summary>
    Success = 0

    ''' <summary>
    ''' The user aborted the operation
    ''' </summary>
    Abort = 1

    ''' <summary>
    ''' The user ignored the failure
    ''' </summary>
    Ignore = 2

    ''' <summary>
    ''' A type 1 failure occurred, consult the documentation of the function that this resulted from
    ''' for more details
    ''' </summary>
    Failure1 = 3

    ''' <summary>
    ''' A type 2 failure occurred, consult the documentation of the function that this resulted from
    ''' for more details
    ''' </summary>
    Failure2 = 4

    ''' <summary>
    ''' A type 2 failure occurred, consult the documentation of the function that this resulted from
    ''' for more details
    ''' </summary>
    Failure3 = 5

    ''' <summary>
    ''' The operation timed out
    ''' </summary>
    FailureTimeout = 6
End Enum
