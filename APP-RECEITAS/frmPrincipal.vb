Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class frmPrincipal
    Private Sub frmPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Width = My.Computer.Screen.Bounds.Width
        Me.Height = My.Computer.Screen.Bounds.Height
        Me.Top = 0

        ToolStripStatusLabel1.Text = "RECEITAS"
        ToolStripStatusLabel2.Text = Today
        ToolStripStatusLabel3.Text = TimeOfDay
        ToolStripStatusLabel4.Visible = False
        ToolStripStatusLabel5.Visible = False
        ToolStripStatusLabel6.Visible = False
        tsProgressBar.Visible = False
        ToolStripStatusLabel7.Visible = False
        StatusStrip1.Refresh()

    End Sub

    Private Sub SAIRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SAIRToolStripMenuItem.Click

        Application.Exit()

    End Sub

    Private Sub UNIDADESToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UNIDADESToolStripMenuItem.Click

        frmUnidades.ShowDialog()
        ToolStripStatusLabel4.Visible = False
        ToolStripStatusLabel4.Text = ""

    End Sub

    Private Sub ETAPASToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ETAPASToolStripMenuItem.Click

        frmEtapas.ShowDialog()
        ToolStripStatusLabel4.Visible = False
        ToolStripStatusLabel4.Text = ""

    End Sub

    Private Sub PRODUTOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PRODUTOSToolStripMenuItem.Click

        frmProdutos.ShowDialog()
        ToolStripStatusLabel4.Visible = False
        ToolStripStatusLabel4.Text = ""

    End Sub
End Class
