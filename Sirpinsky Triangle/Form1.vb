Public Class frm_Main
    Private basePoints As New ArrayList
    Private points As New ArrayList
    Private dotSize As Integer = 3

    Private Sub frm_Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        If basePoints.Count = 0 Then
            basePoints.Add(New Point(Me.Width / 2, 1))
            basePoints.Add(New Point(1, Me.ClientSize.Height - (1 + dotSize)))
            basePoints.Add(New Point(Me.ClientSize.Width - (1 + dotSize), Me.ClientSize.Height - (1 + dotSize)))
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

        'Gfx.DrawEllipse(GPen, location.X, location.Y, 1, 1)
        Gfx.FillEllipse(GPen.Brush, location.X, location.Y, 3, 3)
    End Sub

    Private Sub frm_Main_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        For Each pnt In basePoints
            draw_Point(pnt)
        Next
        For Each pnt In points
            draw_Point(pnt)
        Next
    End Sub

    Private Sub frm_Main_Resize(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        Me.Width = Me.Height - (Me.Height / 128)

        If basePoints.Count = 0 Then
            basePoints.Add(New Point(Me.Width / 2, 1))
            basePoints.Add(New Point(1, Me.ClientSize.Height - (1 + dotSize)))
            basePoints.Add(New Point(Me.ClientSize.Width - (1 + dotSize), Me.ClientSize.Height - (1 + dotSize)))
        Else
            basePoints.Clear()
            basePoints.Add(New Point(Me.Width / 2, 1))
            basePoints.Add(New Point(1, Me.ClientSize.Height - (1 + dotSize)))
            basePoints.Add(New Point(Me.ClientSize.Width - (1 + dotSize), Me.ClientSize.Height - (1 + dotSize)))
        End If

        Me.Invalidate()
    End Sub
End Class
