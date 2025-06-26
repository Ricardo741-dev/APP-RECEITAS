' 1 - declarando os namespaces usados

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class frmEtapas


    ' 2- definindo as variáveis globais do formulário

    Dim str As String = "Server = DESKTOP-3B4F0HI\SQLEXPRESS03;Database=RECEITAS;Trusted_Connection=True;"
    Dim sqlDa As New SqlDataAdapter
    Dim dtb As DataTable
    Dim sqlCon As New SqlConnection(str)
    Dim sqlCmd As New SqlCommand

    Private Sub frmEtapas_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CarregarDados()

        CarregarTextBox()

        ' ativa o modo de seleção de linha 

        dgvEtapas.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        'ajusta o tamanho das células

        dgvEtapas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

    End Sub

    ' 4- Código do procedimento CarregaDados() que seleciona os dados da tabela tblEtapas e exibe no DataGridView:
    Private Sub CarregarDados()

        Try

            With sqlCmd

                .CommandType = CommandType.Text
                .CommandText = "SELECT * from tblETAPA"
                .Connection = sqlCon

            End With

            With sqlDa

                .SelectCommand = sqlCmd
                dtb = New DataTable
                .Fill(dtb)
                dgvEtapas.DataSource = dtb

            End With

        Catch ex As Exception

            MsgBox(ex.Message)

        Finally

            sqlCon.Close()

        End Try

    End Sub

    ' 5- Código do procedimento CarregarTextBox() que atualiza as caixas de texto do formulário com os dados selecionados no DataGridView:
    Private Sub CarregarTextBox()

        Try

            Me.txtCodigo.Text = Me.dgvEtapas.CurrentRow.Cells("Codigo_Etapa").Value
            Me.txtDescricao.Text = Me.dgvEtapas.CurrentRow.Cells("Descricao_Etapa").Value

        Catch ex As Exception

            MsgBox("Erro : " & ex.Message)
        End Try

    End Sub

    ' 8 - Código do evento Click do botão de comando Novo que limpa as caixas de texto, faz a validação e chama a rotina InserirDados() para atualizar a tabela tblETAPA com um novo registro:
    Private Sub btnCadastrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCadastrar.Click

        If btnCadastrar.Text = "Novo" Then
            limpaCampos()
            txtCodigo.Text = dgvEtapas.RowCount
            If txtCodigo.Text = 0 Then
                txtCodigo.Text = 1
            End If
            txtDescricao.Focus()
            btnCadastrar.Text = "Cadastrar"
        Else
            If btnCadastrar.Text = "Cadastrar" Then
                If ValidaDados() Then
                    InserirDados()
                    btnCadastrar.Text = "Novo"

                Else
                    MsgBox("Informe a descrição e a abreviação para cadastramento...")
                End If
            End If
        End If

    End Sub

    ' 9 - Código da rotina InserirDados() que inclui um novo registro na tabela Contatos:

    Private Sub InserirDados()

        Try
            sqlCon.Open()

            With sqlCmd

                .CommandType = CommandType.Text
                .CommandText = ""
                .CommandText = "INSERT INTO tblETAPA (Codigo_Etapa, Descricao_Etapa) 
                                              VALUES (@Codigo_Etapa, @Descricao_Etapa)" &
                                              "SELECT * FROM tblETAPA WHERE (Codigo_Etapa = SCOPE_IDENTITY())"
                .Connection = sqlCon

                .Parameters.Add(New SqlParameter("@Codigo_Etapa", SqlDbType.Int)).Value = txtCodigo.Text
                .Parameters.Add(New SqlParameter("@Descricao_Etapa", SqlDbType.VarChar)).Value = txtDescricao.Text

            End With

            sqlCmd.ExecuteNonQuery()
            CarregarDados()
            MsgBox("Registro Inserido com Sucesso.", MsgBoxStyle.Information)

        Catch ex As Exception

            MsgBox(ex.Message)

        Finally

            sqlCmd.Parameters.Clear()
            sqlCon.Close()

        End Try

    End Sub

    ' 10 - Código do evento Click do botão de comando Alterar que faz a validação e chama a rotina AlterarDados():
    Private Sub btnAlterar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlterar.Click

        If ValidaDados() Then

            AlterarDados()

        Else

            MsgBox("Informe a descrição e a abreviação para alteração de dados...")

        End If

    End Sub
    ' 11 - Código da rotina AlterarDados() que altera os dados de um registro na tabela tblETAPA

    Private Sub AlterarDados()

        Try

            sqlCon.Open()

            With sqlCmd

                .CommandType = CommandType.Text
                .CommandText = ""
                .CommandText = "UPDATE tblETAPA
                                   SET [Descricao_Etapa] = '" & txtDescricao.Text & "'
                                 WHERE ([Codigo_Etapa] = " & txtCodigo.Text & ");" &
                               "SELECT * FROM tblETAPA WHERE (Codigo_Etapa = " & txtCodigo.Text & ")"

                .Connection = sqlCon

            End With

            sqlCmd.ExecuteNonQuery()
            CarregarDados()
            MsgBox("Registro Alterado com Sucesso.", MsgBoxStyle.Information)

        Catch ex As Exception

            MsgBox(ex.Message)

        Finally

            sqlCmd.Parameters.Clear()
            sqlCon.Close()

        End Try

    End Sub

    ' 12 - Código do evento Click do botão de comando Deletar que chama a rotina ExcluirDados():
    Private Sub btnDeletar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeletar.Click

        If MessageBox.Show("Confirma a Exclusão do registro : " & txtCodigo.Text & " ? ", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = vbYes Then

            ExcluirDados()

        End If

    End Sub
    ' 13- Código da rotina ExcluirDados() que remove um registro da tabela Contatos:
    Private Sub ExcluirDados()
        Try
            sqlCon.Open()

            With sqlCmd

                .CommandText = "Delete from tblETAPA where id = " & txtCodigo.Text
                .Connection = sqlCon

            End With

            sqlCmd.ExecuteNonQuery()
            CarregarDados()
            MsgBox("Registro deletado com Sucesso.", MsgBoxStyle.Information)

        Catch ex As Exception

            MsgBox(ex.Message)

        Finally

            sqlCmd.Parameters.Clear()
            sqlCon.Close()

        End Try

    End Sub

    ' 14 - Código do evento Click do botão de comando Localizar que chama a rotina LocalizarDados():

    Private Sub btnLocalizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLocalizar.Click

        LocalizarDados()

    End Sub

    ' 15 - Código da rotina LocalizarDados():
    Private Sub LocalizarDados()

        Dim msg, titulo, valor, nome As String

        msg = "Informe a descrição a procurar."
        titulo = "Procurar Registro" ' define o titulo.
        valor = "A"                  ' define o valor padrao

        ' Exibe mensagem posicionada 

        nome = InputBox(msg, titulo, valor, 800, 400)

        If nome <> String.Empty Then

            Try

                Dim sqlCmdSqlBuscaRegistro As String = "SELECT * from tblETAPA where Descricao_Etapa like '%" & nome & "%'"

                With sqlCmd

                    .CommandType = CommandType.Text
                    .CommandText = sqlCmdSqlBuscaRegistro
                    .Connection = sqlCon

                End With

                With sqlDa

                    .SelectCommand = sqlCmd
                    dtb = New DataTable
                    .Fill(dtb)
                    dgvEtapas.DataSource = dtb

                End With

            Catch ex As Exception

                MsgBox("Não foram encontrados registros com este parâmetro.")

            End Try

        End If

    End Sub

    ' 16 - Código do evento Click do botão de comando Carregar Dados que chama a rotina CarregarDados():

    Private Sub btnCarregaDados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCarregaDados.Click

        CarregarDados()

    End Sub

    ' 17 - Código da rotina ValidaDados
    Private Function ValidaDados() As Boolean

        If txtDescricao.Text <> String.Empty Then
            Return True
        Else
            Return False
        End If

    End Function


    ' 18 - Código da rotina LimpaCampos()
    Private Sub limpaCampos()

        txtDescricao.Text = ""

    End Sub

    ' 19 - Código dos eventos Click dos botões de navegação:

    Private Sub btnPrimeiro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimeiro.Click

        ' Vai para o primeiro registro...

        Me.BindingContext(dtb).Position = 0

        CarregarTextBox()

    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click

        ' Volta um registro...

        Me.BindingContext(dtb).Position -= 1

        CarregarTextBox()

    End Sub

    Private Sub btnProximo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProximo.Click

        ' Avança um registro...

        Me.BindingContext(dtb).Position += 1

        CarregarTextBox()

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click

        ' Vai para o último registro...

        Me.BindingContext(dtb).Position = dtb.Rows.Count

        CarregarTextBox()

    End Sub

    ' 21- Código do eventos CellEndEdit e Click do controle DataGridView:
    Private Sub dgvEtapas_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEtapas.CellEndEdit

        txtDescricao.Text = dgvEtapas.CurrentCell.Value
        AlterarDados()
        CarregarTextBox()

    End Sub

    Private Sub dgvEtapas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvEtapas.Click

        CarregarTextBox()

    End Sub


End Class

