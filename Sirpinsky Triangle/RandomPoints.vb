Imports System.Threading
Public Class RandomPoints
    Private token As CancellationToken
    Private points As New ArrayList
    Private PointCount As Integer
    Private PointProgress As Progress(Of Integer)
    Private ClassThread As New Thread(AddressOf BuildTriangle)
    Public Event ThreadStateChange(State As ThreadState)
    Public Event NewPoint(pnt As Point)
    Public Event waiting(wait As Boolean)
    Private wait As Boolean = False

    Public ReadOnly Property ClassThreadState As ThreadState
        Get
            Return ClassThread.ThreadState
        End Get
    End Property
    Public ReadOnly Property count As Integer
        Get
            Return PointCount
        End Get
    End Property

    Public Property Pause As Boolean
        Get
            Return wait
        End Get
        Set(value As Boolean)
            wait = value
        End Set
    End Property

    Sub New(StartPoints As ArrayList, ByRef CancelToken As CancellationToken)
        token = CancelToken
        Me.points = StartPoints
        RaiseEvent ThreadStateChange(ThreadState.Unstarted)
    End Sub

    Private Sub BuildTriangle()
        If points.Count < 3 Then Exit Sub

        PointProgress = New Progress(Of Integer)(AddressOf ReportProgress)

        Dim a As Point
        Dim b As Point
        Thread.Sleep(100)
        While Not token.IsCancellationRequested
            Dim newpt As New Point()

            If points.Count < 5 Then
                a = points(GetRandom(0, 3))
                Do
                    b = points(GetRandom(0, points.Count))
                Loop While a = b
                newpt.X = (a.X + b.X) / 2
                newpt.Y = (a.Y + b.Y) / 2
            Else
                a = points(GetRandom(0, 3))
                Do
                    b = points(GetRandom(3, points.Count))
                Loop While a = b
                newpt.X = (a.X + b.X) / 2
                newpt.Y = (a.Y + b.Y) / 2
            End If
            Thread.Sleep(5)

            points.Add(newpt)

            TryCast(PointProgress, IProgress(Of Integer)).Report(points.Count)
            RaiseEvent NewPoint(newpt)
            While wait
                RaiseEvent waiting(True)
                Thread.Sleep(1000)
            End While
            RaiseEvent waiting(False)
        End While
        Thread.Sleep(1000)
        RaiseEvent ThreadStateChange(ThreadState.Stopped)
    End Sub

    Private Sub ReportProgress(ByVal count As Integer)
        PointCount = count
    End Sub

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Public Sub Begin()
        Try
            ClassThread.Start()

            RaiseEvent ThreadStateChange(ThreadState.Running)
        Catch ex As Exception
            RaiseEvent ThreadStateChange(ThreadState.Unstarted)
        End Try
    End Sub

    Public Sub RequestStop()
        Try
            token = New CancellationToken(True)
            RaiseEvent ThreadStateChange(ThreadState.StopRequested)
        Catch ex As Exception

        End Try
    End Sub

End Class
