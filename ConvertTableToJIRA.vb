
' https://stackoverflow.com/q/11109832/5411712

Function IsInArray(stringToBeFound As String, arr As Variant) As Boolean
  IsInArray = (UBound(Filter(arr, stringToBeFound)) > -1)
End Function


' initial inspiration : https://gist.github.com/DBremen/0ba67c6ec894ee581d98
Sub ConvertToJiraTable()
    Dim workingRange As Range, currCol As Range, currRow As Range
    Dim rowIndex As Long, colIndex As Long
    Dim output As String, cellVal As String, status As String

    'Dim statusHash As Dictionary
    Dim cb As DataObject

    ' set up clipboard
    Set cb = New DataObject

    ' set up hash for "status"
    'Set statusHash = New Dictionary
    'statusHash("Done") = "(/)"
    'statusHash("Not Done") = "(x)"
    'statusHash("WIP") = "(i)"
    'statusHash("Not Required") = "(!)"

    rowIndex = 1
    'output = "||"

    Dim foundBoldStart As Boolean
    foundBoldStart = False
    ' Declare a 5000 x 3 multi-dimensional array strings
    ' Why 5000? Because it seems like an absurd amount of
    ' individually Bolded strings words to have in a single Test Case cell
    ' Why 3? to keep a set of
    '    {stringToBold, start_index_of_stringToBold, end_index_of_stringToBold}
    Dim stringsToBoldArray(5000, 3) As Variant
    ' TODO: consider a more elegant way to hold Bolded strings through resizing the array
    Dim arrayCounter As Integer
    arrayCounter = 0
    Dim stringToBold As String
    stringToBold = ""
    Dim currChar As String

    boldedCellVal = ""

    Dim boldArray(20) As String
    boldArray(0) = "Click"
    boldArray(1) = "Navigate"
    boldArray(2) = "Open"
    boldArray(3) = "Save"
    boldArray(4) = "Load"
    boldArray(5) = "Delete"
    boldArray(6) = "Hello"
    boldArray(7) = "Set"
    boldArray(8) = "Fill"
    boldArray(9) = "Enter"
    boldArray(10) = "Drag"
    boldArray(11) = "Type"

    Application.ScreenUpdating = False

    Set workingRange = Range("A1").CurrentRegion
    For Each currRow In workingRange.Rows
        colIndex = 1
        For Each currCol In currRow.Columns

            cellStr = currCol.Value

            ' according to https://techsupport.osisoft.com/Troubleshooting/KB/KB01372
            ' There can be weird non ASCII spaces due to copy-pasting from an HTML source
            ' The werid characters mess with the Split() function.
            ' The character is Char(160) or "%C2%A0"
            ' this replacement converts the space character to ASCII
            cellStr = Replace(cellStr, CStr(Chr(160)), CStr(Chr(32)))
            ' then use the space character as the delimiter
            ' because JIRA only formats words separated by space.
            cellWordsArr = Split(cellStr, " ")

            For i = LBound(cellWordsArr) To UBound(cellWordsArr)
                Debug.Print (cellWordsArr(i))

                ' the empty string exists in cellWordsArr
                ' avoid empty string and match the keywords
                If cellWordsArr(i) <> "" _
                    And IsInArray(CStr(cellWordsArr(i)), boldArray) Then
                    cellWordsArr(i) = "*" & cellWordsArr(i) & "*"
                End If

                Debug.Print (cellWordsArr(i))
            Next i

            boldedCellString = Join(cellWordsArr)

            Debug.Print ("meh")

'            ' read each char in a range
'            For i = 1 To Len(Cells(rowIndex, colIndex).Value)
'
'                If Cells(rowIndex, colIndex).Characters(i, 1).Font.FontStyle = "Bold" Then
'                    currChar = Cells(rowIndex, colIndex).Characters(i, 1).Text
'                    'Debug.Print (currChar & "if bold")
'                    If arrayCounter >= 5000 Then
'                        Debug.Print ("*Someone fit 5000 individually Bolded strings into a Cell! Wow!*")
'                        Exit Sub
'                    End If

'                    If foundBoldStart = False Then
'                        Debug.Print (currChar & "found start")
'                        foundBoldStart = True
'                        ' because we just found a Bold word,
'                        ' we want to remember the start_index and the end_index
'                        start_index_of_stringToBold = i
'                        end_index_of_stringToBold = i
'                        boldedCellVal = boldedCellVal & "*"
'''                    If foundBoldStart = False Then
'''                        foundBoldStart = True
'''                        ' because we just found a Bold word,
'''                        ' we want to remember the start_index and the end_index
'''                        start_index_of_stringToBold = i
'''                        end_index_of_stringToBold = i
'                    End If
'                    If foundBoldStart = True Then
'                        stringToBold = stringToBold & currChar
'                        boldedCellVal = boldedCellVal & currChar
'                        ' because the Bold word was previously found
'                        ' we want to keep track of the end index
'                        end_index_of_stringToBold = i
'                        stringsToBoldArray(arrayCounter, 0) = stringToBold
'                        stringsToBoldArray(arrayCounter, 1) = start_index_of_stringToBold
'                        stringsToBoldArray(arrayCounter, 2) = end_index_of_stringToBold
'                    End If
'                    'Debug.Print (stringToBold)
'                Else
'                  ' The bold word ended before the end of the string
'                  ' lets reset out variables and prepare for the next Bold word
'                  If stringToBold <> "" Then
'''                      Debug.Print ("*" & stringsToBoldArray(arrayCounter, 0) & "*")
'''                      Debug.Print ("start = " & Cells(rowIndex, colIndex).Characters(stringsToBoldArray(arrayCounter, 1), 1).Text)
'''                      Debug.Print ("end   = " & Cells(rowIndex, colIndex).Characters(stringsToBoldArray(arrayCounter, 2), 1).Text)
'''                      Debug.Print ("diff  = " & stringsToBoldArray(arrayCounter, 2) - stringsToBoldArray(arrayCounter, 1))
'                      foundBoldStart = False ' reset to False until next bold
'                      boldedCellVal = boldedCellVal & "*"
'                      arrayCounter = arrayCounter + 1
'                      stringToBold = ""      ' reset to empty string until next bold
'                      start_index_of_stringToBold = 0
'                      end_index_of_stringToBold = 0
'                  End If
'                End If
'                'Debug.Print (currChar)
'            Next i
'''            If stringToBold <> "" Then
'''                ' This if condition is useful when the Bold word
'''                ' ends as the final word in the string
'''                ' e.g: *Click* the button, *quickly!*
'''                Debug.Print ("*" & stringToBold & "*")
'''                Debug.Print ("start = " & Cells(rowIndex, colIndex).Characters(start_index_of_stringToBold, 1).Text)
'''                Debug.Print ("end   = " & Cells(rowIndex, colIndex).Characters(end_index_of_stringToBold, 1).Text)
'''                Debug.Print ("diff  = " & end_index_of_stringToBold - start_index_of_stringToBold)
'''                foundBoldStart = False ' reset to False until next bold
'''                stringToBold = ""      ' reset to empty string until next bold
'''                start_index_of_stringToBold = 0
'''                end_index_of_stringToBold = 0
'''            End If
'
'            'For i = 1 To Len(Range("A1").Value)
'            '  If Range("A1").Characters(i, 1).Font.FontStyle = "Bold" Then
'            '    If foundBoldStart = False Then
'            '        foundBoldStart = True
'            '    End If
'            '    If foundBoldStart = True Then
'            '        stringToBold = stringToBold & Range("A1").Characters(i, 1).Text
'            '    End If
'            '    Debug.Print (stringToBold)
'            '
'            '    'Debug.Print ("The " & Range("A1").Characters(i, 1).Text & " character is bold.")
'            '  End If
'            'Next i


'            'replace empty value with space to generate the cell border
'            cellVal = currCol.Value
'            boldedCellVal = ""


'            For i = 0 To Len(cellVal)
'                For k = 0 To arrayCounter
'                    startIndexOfBold = stringsToBoldArray(k, 1)
'                    If i = startIndexOfBold Then
'                        boldedCellVal = boldedCellVal & "*" & currCol.Characters(i, 1).Text
'
 ''                   Else
 '                   End If
'                Next k
'
'            Next i
'''
'
'            For i = 0 To arrayCounter
'                ' This if condition is useful when the Bold word
'                ' ends as the final word in the string
'                ' e.g: *Click* the button, *quickly!*
'
'
'
'                If stringsToBoldArray(i, 1) = 1 Then
'                    ' this Bold starts at the first character of cellVal
'                    locationOfStartOfCurrBold = InStr(stringsToBoldArray(i, 1), cellVal, stringsToBoldArray(i, 0))
'                    currBold = Left(cellVal, locationOfStartOfCurrBold)
'                    theNext = Right(cellVal, Len(cellVal) - locationOfStartOfCurrBold)
'                    currBold = "*" & currBold & "*"
'                   ' theRest = "*" & theRest
'                    boldedCellVal = boldedCellVal & currBold
'                Else
'
'                    endIndexOfLastBold = stringsToBoldArray(i - 1, 2)
'                    startIndexOfCurrBold = stringsToBoldArray(i, 1)
'
'                    meh = Left(cellVal, startIndexOfCurrBold)
'
'                    locationOfCurrBold = InStr(stringsToBoldArray(i, 1), cellVal, stringsToBoldArray(i, 0))
'                    currBold = Left(cellVal, Len(cellVal) - locationOfCurrBold)
'                    theRest = Mid(cellVal, Len(cellVal) - locationOfCurrBold)
'                    currBold = "*" & currBold
'                    theRest = "*" & theRest
'                    boldedCellVal = currBold & theRest
'                End If
'
'                Debug.Print (currBold)
'                Debug.Print (theRest)
'                Debug.Print ("*" & stringsToBoldArray(i, 0) & "*")
'                Debug.Print ("start = " & Cells(rowIndex, colIndex).Characters(stringsToBoldArray(i, 1), 1).Text)
'                Debug.Print ("end   = " & Cells(rowIndex, colIndex).Characters(stringsToBoldArray(i, 2), 1).Text)
'                Debug.Print ("diff  = " & stringsToBoldArray(arrayCounter, 2) - stringsToBoldArray(i, 1))
'            Next i
'
''            If stringToBold <> "" Then
''                ' This if condition is useful when the Bold word
''                ' ends as the final word in the string
''                ' e.g: *Click* the button, *quickly!*
''                Debug.Print ("*" & stringsToBoldArray(arrayCounter, 0) & "*")
''                Debug.Print ("start = " & Cells(rowIndex, colIndex).Characters(stringsToBoldArray(arrayCounter, 1), 1).Text)
''                Debug.Print ("end   = " & Cells(rowIndex, colIndex).Characters(stringsToBoldArray(arrayCounter, 2), 1).Text)
''                Debug.Print ("diff  = " & stringsToBoldArray(arrayCounter, 2) - stringsToBoldArray(arrayCounter, 1))
''                foundBoldStart = False ' reset to False until next bold
''                stringToBold = ""      ' reset to empty string until next bold
''                start_index_of_stringToBold = 0
''                end_index_of_stringToBold = 0
''            End If


            cellVal = boldedCellString

            If (cellVal = "") Then cellVal = " "
            If rowIndex = 1 Then ' if it's the first row do the "||"
                'MsgBox "rowIndex : " & rowIndex & vbCrLf & "colIndex : " & colIndex
                output = output & "||" & cellVal
            Else
                'If colIndex = 1 Then
                    'status = statusHash(cellVal)
                    'If status = "" Then status = " "
                    'output = output & "|" & rowIndex & "|"
                'ElseIf colIndex > 1 Then
                    output = output & "|" & cellVal
                'End If
            End If
            colIndex = colIndex + 1
        Next currCol
        rowIndex = rowIndex + 1
        '====================================================='
        '============fix the end of each row=================='
        '====================================================='
        If rowIndex = 1 Then ' if it's the first row do the "||"
            'MsgBox "rowIndex : " & rowIndex & vbCrLf & "colIndex : " & colIndex
            output = output & " " & "||"
        Else
            output = output & " " & "|"
        End If
        ' put a new line
        output = output & vbCrLf
        '====================================================='

    Next currRow
    cb.SetText output
    cb.PutInClipboard
    MsgBox ("THE FOLLOWING HAS BEEN PUT INTO YOUR CLIPBOARD :" _
            & vbCrLf & vbCrLf & output)
End Sub
