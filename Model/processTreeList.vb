﻿Imports INFITF
Imports MECMOD
Imports GoumangToolKit
Imports ProductStructureTypeLib

Public Class processTreeList
    'Private note As List(Of String)
    Public treeList As New Dictionary(Of String, processTree)
    ' Public bindingpart As Part = Nothing
    ' Public PointsFatherProduct As Product
    Public fast_List As List(Of String) = Nothing
    Sub New()

    End Sub

    Public Property FastList As List(Of String)
        Get
            If fast_List Is Nothing Then

                fast_List = AutorivetDB.allfast_list()
            End If
            Return fast_List
        End Get
        Set(value As List(Of String))
            fast_List = value
        End Set
    End Property






    Sub New(ByRef MyGeoSet As HybridBody)
        ' bindingpart = Part
        inputShape(MyGeoSet)
        'PointsFatherProduct = Product1
    End Sub

    Public Function count() As Integer

        Return treeList.Count()
    End Function





    'Public Sub inputShape(ByRef MyGeoSet As HybridBody, ByRef Part As Part)
    '    ' bindingpart = Part
    '    inputShape(MyGeoSet)

    'End Sub
    Public Sub inputShape(ByRef MyGeoSet As HybridBody, Optional iffilvis As Boolean = False)
        '仅可用于读取TVA
        ' Dim tmplst As New List(Of processTree)()

        ' FastList = fstlist
        If iffilvis Then
            'to filter the geoset which is hidden

            If Not TVA_Method.ifGeoVis(MyGeoSet) Then
                Exit Sub
            End If

        End If




        If (TVA_Method.ifFastener(MyGeoSet.Name)) Then
            Add(New processTree(MyGeoSet, iffilvis))


        Else
            '  parproduct()
            Dim k As Integer

                For k = 1 To MyGeoSet.HybridBodies.Count
                    '开始递归
                    inputShape(MyGeoSet.HybridBodies.Item(k), iffilvis:=iffilvis)
                Next

            End If


    End Sub

    Public Sub Add(tmp As processTree)
        Dim id As String
        id = tmp.framename + " - " + tmp.fasternername
        If (treeList.Keys.Contains(id)) Then
            treeList(id).merge(tmp)
        Else
            treeList.Add(id, tmp)
        End If
        ' treeList.Add(id, tmp)
        ' note.Add(tmp.framename + " - " + tmp.fasternername)
    End Sub

    Public Sub Add(frname As String, fstname As String, pstype As String, ByRef point As HybridShape)
        ' Dim id As String
        Dim kkd As New processTree(frname, fstname)
        kkd.Add(pstype, point)


        Add(kkd)
    End Sub

    Public Sub output_topart(sourcepartname As String, targetpartname As String, ByRef hybridBody1 As HybridBody, Optional para As Integer = 0)



        For Each ppls As processTree In treeList.Values

            '遍历每个processTree
            ppls.output(sourcepartname, targetpartname, hybridBody1, para)
            '  part1.Update()
            'partDocument1 = documents1.Item(TVAPartfilename)
            'partDocument1.Activate()
        Next



    End Sub

    Public Function output_fst() As processStatic
        Dim tmpStatic As New processStatic


        For Each ppls As processTree In treeList.Values
            Dim count As Integer = 0

            For Each pp In ppls.fastenertree.Values

                count += pp.Count / 2

            Next

            tmpStatic.Add(count, ppls.fasternername)
        Next

        Return tmpStatic
    End Function








    Public Sub del(partname As String)



        For Each ppls As processTree In treeList.Values

            '遍历每个processTree
            ppls.del(partname)
            '  part1.Update()
            'partDocument1 = documents1.Item(TVAPartfilename)
            'partDocument1.Activate()
        Next



    End Sub

End Class
