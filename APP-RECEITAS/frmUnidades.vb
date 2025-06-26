' 1 - declarando os namespaces usados

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class frmUnidades

    ' 2- definindo as variáveis globais do formulário

    Dim str As String = "Server = DESKTOP-3B4F0HI\SQLEXPRESS03;Database=RECEITAS;Trusted_Connection=True;"
    Dim sqlDa As New SqlDataAdapter
    Dim dtb As DataTable
    Dim sqlCon As New SqlConnection(str)
    Dim sqlCmd As New SqlCommand

    ' 3 - Código do evento load do formulário frmUnidades:
    Private Sub frmUnidades_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CarregarDados()

        CarregarTextBox()

        ' ativa o modo de seleção de linha 

        dgvUnidades.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        'ajusta o tamanho das células

        dgvUnidades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

    End Sub

    ' 4- Código do procedimento CarregaDados() que seleciona os dados da tabela tblUnidades e exibe no DataGridView:
    Private Sub CarregarDados()

        Try

            With sqlCmd

                .CommandType = CommandType.Text
                .CommandText = "SELECT * from tblUnidade"
                .Connection = sqlCon

            End With

            With sqlDa

                .SelectCommand = sqlCmd
                dtb = New DataTable
                .Fill(dtb)
                dgvUnidades.DataSource = dtb

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

            Me.txtCodigo.Text = Me.dgvUnidades.CurrentRow.Cells("Codigo_Unidade").Value
            Me.txtDescricao.Text = Me.dgvUnidades.CurrentRow.Cells("Descricao_Unidade").Value
            Me.txtAbreviacao.Text = Me.dgvUnidades.CurrentRow.Cells("Abrv_Descricao_Unidade").Value

        Catch ex As Exception

            MsgBox("Erro : " & ex.Message)

        End Try

    End Sub

    ' 8 - Código do evento Click do botão de comando Novo que limpa as caixas de texto, faz a validação e chama a rotina InserirDados() para atualizar a tabela Contatos com um novo registro:
    Private Sub btnCadastrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCadastrar.Click

        If btnCadastrar.Text = "Novo" Then
            limpaCampos()
            txtCodigo.Text = dgvUnidades.RowCount
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
                .CommandText = "INSERT INTO tblUNIDADE (Codigo_Unidade, Descricao_Unidade, Abrv_Descricao_Unidade ) 
                                              VALUES (@Codigo_Unidade, @Descricao_Unidade, @Abrv_Descricao_Unidade)" &
                                              "SELECT * FROM tblUNIDADE WHERE (Codigo_Unidade = SCOPE_IDENTITY())"
                .Connection = sqlCon

                .Parameters.Add(New SqlParameter("@Codigo_Unidade", SqlDbType.Int)).Value = txtCodigo.Text
                .Parameters.Add(New SqlParameter("@Descricao_Unidade", SqlDbType.VarChar)).Value = txtDescricao.Text
                .Parameters.Add(New SqlParameter("@Abrv_Descricao_Unidade", SqlDbType.VarChar)).Value = txtAbreviacao.Text

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
    ' 11 - Código da rotina AlterarDados() que altera os dados de um registro na tabela Contatos:

    Private Sub AlterarDados()

        Try

            sqlCon.Open()

            With sqlCmd

                .CommandType = CommandType.Text
                .CommandText = ""
                .CommandText = "UPDATE tblUNIDADE 
                                   SET [Descricao_Unidade] = '" & txtDescricao.Text & "',
                                       [Abrv_Descricao_Unidade] = '" & txtAbreviacao.Text & "'  
                                 WHERE ([Codigo_Unidade] = " & txtCodigo.Text & ");" &
                               "SELECT * FROM tblUNIDADE WHERE (Codigo_Unidade = " & txtCodigo.Text & ")"

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

                .CommandText = "Delete from tblUNIDADE where id = " & txtCodigo.Text
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

                Dim sqlCmdSqlBuscaRegistro As String = "SELECT * from tblUNIDADE where Descricao_Unidade like '%" & nome & "%'"

                With sqlCmd

                    .CommandType = CommandType.Text
                    .CommandText = sqlCmdSqlBuscaRegistro
                    .Connection = sqlCon

                End With

                With sqlDa

                    .SelectCommand = sqlCmd
                    dtb = New DataTable
                    .Fill(dtb)
                    dgvUnidades.DataSource = dtb

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

        If txtDescricao.Text <> String.Empty And txtAbreviacao.Text <> String.Empty Then
            Return True
        Else
            Return False
        End If

    End Function


    ' 18 - Código da rotina LimpaCampos()
    Private Sub limpaCampos()

        txtDescricao.Text = ""
        txtAbreviacao.Text = ""

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
    Private Sub dgvUnidades_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvUnidades.CellEndEdit

        txtDescricao.Text = dgvUnidades.CurrentCell.Value
        AlterarDados()
        CarregarTextBox()

    End Sub

    Private Sub dgvUnidades_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvUnidades.Click

        CarregarTextBox()

    End Sub


End Class

