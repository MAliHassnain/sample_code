Imports System.IO
Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Protected Sub Application_PostAcquireRequestState(ByVal sender As Object, ByVal e As EventArgs)
        Dim app = Context.ApplicationInstance
        Dim acceptEncodings As String = app.Request.Headers.[Get]("Accept-Encoding")

        If Not String.IsNullOrEmpty(acceptEncodings) Then
            Dim baseStream As Stream = app.Response.Filter
            acceptEncodings = acceptEncodings.ToLower()

            If acceptEncodings.Contains("br") OrElse acceptEncodings.Contains("brotli") Then
                app.Response.Filter = New Brotli.BrotliStream(baseStream, Compression.CompressionMode.Compress)
                app.Response.AppendHeader("Content-Encoding", "br")
            ElseIf acceptEncodings.Contains("deflate") Then
                app.Response.Filter = New Compression.DeflateStream(baseStream, Compression.CompressionMode.Compress)
                app.Response.AppendHeader("Content-Encoding", "deflate")
            ElseIf acceptEncodings.Contains("gzip") Then
                app.Response.Filter = New Compression.GZipStream(baseStream, Compression.CompressionMode.Compress)
                app.Response.AppendHeader("Content-Encoding", "gzip")
            End If
        End If
    End Sub
    Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Response.Filter = Nothing
    End Sub

End Class