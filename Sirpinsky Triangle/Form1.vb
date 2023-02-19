Imports System.IO
Imports System.Reflection.Emit
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class frm_Main
    'Private basePoints As New ArrayList
    Private points As New ArrayList
    Private dotSize As Integer = 3
    Private frmsize As Size
    Private CancelToken As New Threading.CancellationToken
    Private PointBuilder As New RandomPoints(points, CancelToken)
    Private waiting As Boolean = False

    Private Sub frm_Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        If points.Count = 0 Then
            points.Add(New Point(Me.Width / 2, 1))
            points.Add(New Point(1, Me.ClientSize.Height - (1 + dotSize)))
            points.Add(New Point(Me.ClientSize.Width - (1 + dotSize), Me.ClientSize.Height - (1 + dotSize)))
        End If
        Me.Invalidate()
    End Sub

    Private Sub frm_Main_Click(sender As Object, e As MouseEventArgs) Handles Me.Click
        'If basePoints.Count < 3 Then
        '    basePoints.Add(e.Location)
        'Else
        '    points.Add(e.Location)
        'End If
        'Me.Invalidate()
    End Sub

    Private Sub draw_Point(location As Point)
        Dim Gfx As Graphics = Me.CreateGraphics

        Dim GPen As Pen

        GPen = New Pen(Drawing.Color.Black, 3)
        GPen.Brush = New SolidBrush(Color.Black)

        Gfx.FillEllipse(GPen.Brush, location.X, location.Y, 3, 3)
    End Sub

    Private Sub frm_Main_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        'For Each pnt In points
        Dim x As Integer
        While x < points.Count
            draw_Point(points(x))
            x += 1
        End While
    End Sub

    Private Sub frm_Main_Resize(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If points.Count > 3 Then Exit Sub
        Me.Width = Me.Height - (Me.Height / 128)

        If points.Count = 0 Then
            points.Add(New Point(Me.Width / 2, 1))
            points.Add(New Point(1, Me.ClientSize.Height - (1 + dotSize)))
            points.Add(New Point(Me.ClientSize.Width - (1 + dotSize), Me.ClientSize.Height - (1 + dotSize)))
        Else
            points.Clear()
            points.Add(New Point(Me.Width / 2, 1))
            points.Add(New Point(1, Me.ClientSize.Height - (1 + dotSize)))
            points.Add(New Point(Me.ClientSize.Width - (1 + dotSize), Me.ClientSize.Height - (1 + dotSize)))
        End If

        Me.Invalidate()
    End Sub

    Private Sub frm_Main_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Char.Parse("q") Then
            PointBuilder.RequestStop()
        ElseIf e.KeyChar = Char.Parse("s") And (PointBuilder.ClassThreadState = system.Threading.ThreadState.Stopped Or PointBuilder.ClassThreadState = system.Threading.ThreadState.Unstarted) Then
            frmsize = Me.Size
            Me.FormBorderStyle = FormBorderStyle.Fixed3D
            Me.MinimizeBox = False
            Me.MaximizeBox = False
            Me.ControlBox = False
            AddHandler PointBuilder.ThreadStateChange, AddressOf threadstatechange
            AddHandler PointBuilder.waiting, AddressOf setwait
            AddHandler PointBuilder.NewPoint, AddressOf NewPoint
            PointBuilder.Begin()
        ElseIf e.keychar = Char.Parse("i") Then
            MsgBox(PointBuilder.count)
        ElseIf e.KeyChar = Char.Parse("d") Then
            PointBuilder.Pause = True
            While (PointBuilder.Pause And Not waiting)

            End While

            Me.Invalidate()
            PointBuilder.Pause = False
        End If
    End Sub

    Private Sub threadstatechange(state As System.Threading.ThreadState)
        MsgBox(state.ToString)
    End Sub

    Private Sub NewPoint(pnt As Point)
        points.Add(pnt)
        'Me.Invalidate()
    End Sub

    Private Sub setwait(wait As Boolean)
        waiting = wait
    End Sub

End Class