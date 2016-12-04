Imports System.IO

Public Class Form1

    Private Sub btnFixCRC_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        Dim myStream As IO.FileStream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()
        Dim romStorageBuffer As [Byte]()

        openFileDialog1.InitialDirectory = ""
        openFileDialog1.Filter = "ROM files (*.rom)|*.rom|BIN files (*.bin)|*.bin|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = False

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                myStream = openFileDialog1.OpenFile()

                If myStream IsNot Nothing Then
                    Using br As New BinaryReader(myStream)
                        romStorageBuffer = br.ReadBytes(myStream.Length)

                        Dim oldchecksum As [Byte] = romStorageBuffer(33)
                        Dim size As Integer = romStorageBuffer(2) * 512
                        Dim newchecksum As [Byte] = 0
                        Dim expected As Integer = 0


                        TextBox1.ForeColor = Color.Black
                        TextBox1.Text = "0x" + oldchecksum.ToString("X")

                        For i As Integer = 0 To size - 1
                            newchecksum = (CInt(newchecksum) + romStorageBuffer(i)) Mod 256
                        Next


                        If oldchecksum = (CInt(newchecksum) + romStorageBuffer(33)) Mod 256 Then
                            TextBox2.ForeColor = Color.DarkGreen
                            TextBox2.Text = "0x" + oldchecksum.ToString("X")
                        Else
                            expected = (romStorageBuffer(33) - CInt(newchecksum)) Mod 256
                            TextBox2.ForeColor = Color.Red
                            TextBox2.Text = "0x" + expected.ToString("X").Substring(Math.Max(0, expected.ToString("X").Length - 2))
                        End If

                    End Using
                End If



            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                'Check this again, since we need to make sure we didn't throw an exception on open.
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim url As String = "https://anorak.tech"
        Process.Start(url)
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Dim Donation As String = "t1ZKuTQ7kbFTFArXa1uZDU9THpyU72aJn24"
        My.Computer.Clipboard.SetText(Donation)
        MessageBox.Show("ZEC address copied to clipboard: " & Donation)
    End Sub
End Class
