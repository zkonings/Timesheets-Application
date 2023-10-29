Imports System.Data.OleDb
Imports System.Data.SQLite
Imports System.Data.SqlClient
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Newtonsoft.Json
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Net.NetworkInformation
Imports Microsoft.Extensions.Configuration

Public Class Form1



    'uses a configuration file to keep sensitive info in JSON file. Hidden on GitHub using .gitignore
    Private apiKey As String
    Private baseApiUrl As String
    Public Sub NewConfigure()
        ' This is the form's constructor, where you can load the configuration.
        ' InitializeComponent()

        ' Create a ConfigurationBuilder
        Dim builder As New ConfigurationBuilder()

        ' Set the base path for the configuration file
        builder.SetBasePath(Directory.GetCurrentDirectory())

        ' Add the secrets.json file to the configuration
        builder.AddJsonFile("secrets.json")

        ' Build the configuration
        Dim configuration As IConfiguration = builder.Build()

        ' Retrieve values from the configuration
        apiKey = configuration("ApiSettings:ApiKey")
        baseApiUrl = configuration("ApiSettings:baseApiUrl")
    End Sub


    Private originalStoreData As List(Of StoreData) ' Keep a copy of the original items

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Enabled = False

        Dim databaseCreated As Boolean = CreateDatabaseIfNotExists()
        NewConfigure()

        If IsInternetConnected() Then ' The machine is connected to the internet, so you can run procedures that require internet access.

            Await MergeEmployeeData() ' Merge employee data
            Await MergeStoreData() ' Merge store data
            Await MergeTravelData() 'Merge Travel Distances
            Await LoadTimeSheets()
            SyncDatabases()

        Else ' The machine is not connected to the internet, so handle it accordingly.
            LogWarning("No internet connection available. The application will attempt to sync at a later time.", Color.Red)
        End If

        Await LoadStoreDataFromDatabase() ' Load store data and populate ComboBoxes
        storeCb1.Text = String.Empty
        storeCb2.Text = String.Empty

        ' Load employee data and populate ComboBox
        Await LoadEmployeeDataFromDatabase()
        employeeNameCb.Text = String.Empty

        Await LoadTimeSheetDataFromDatabase()

        startTimePicker.Value = DateTime.Now
        endTimePicker.Value = DateTime.Now

        Me.Enabled = True

    End Sub


    'TimeSheet Data Merging and syncing
    Public Class TimeSheetData
        Public Property TimeSheetID As Guid


        <JsonProperty("Employee Name")>
        Public Property EmployeeName As String
        Public Property Location As String

        <JsonProperty("Second Location")>
        Public Property SecondLocation As String


        Public Property [Date] As DateTime


        <JsonProperty("Start Time")>
        Public Property StartTime As TimeSpan

        <JsonProperty("End Time")>
        Public Property EndTime As TimeSpan

        Public Property Timestamp As DateTime

        Public Property Deleted As Integer


    End Class
    Private Async Function LoadTimeSheets() As Task(Of List(Of TimeSheetData))
        Dim timeSheets As New List(Of TimeSheetData)

        Try
            ' Create an instance of HttpClient
            Dim httpClient As New HttpClient()

            ' Set the base address for your API
            httpClient.BaseAddress = New Uri(baseApiUrl)

            ' Set the API key in the request headers
            httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey)

            ' Make GET request to retrieve data from the API
            Dim response As HttpResponseMessage = Await httpClient.GetAsync("api/TimeSheet")

            If response.IsSuccessStatusCode Then
                ' Deserialize the response content into a data structure
                Dim content As String = Await response.Content.ReadAsStringAsync()
                timeSheets = JsonConvert.DeserializeObject(Of List(Of TimeSheetData))(content)

                ' Bind the timesheet data to DataGridView1
                'DataGridView1.DataSource = timeSheets
                InsertDataIntoLocalDatabase(timeSheets)
            Else
                ' Handle errors
                MessageBox.Show("Failed to retrieve timesheet data from the API.3")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the API request
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

        ' Return the list of time sheets
        Return timeSheets
    End Function
    Private Sub InsertDataIntoLocalDatabase(data As List(Of TimeSheetData))
        ' Define the file path for the SQLite database
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        ' Create the connection string with the database file path
        Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"

        Dim syncedRecords As Integer = 0

        Try
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                For Each item As TimeSheetData In data
                    ' Check if a record with the same TimeSheetID already exists in the database
                    Dim checkQuery As String = "SELECT Timestamp FROM TimeSheetT WHERE TimeSheetID = @TimeSheetID"
                    Using checkCmd As New SQLiteCommand(checkQuery, connection)
                        checkCmd.Parameters.AddWithValue("@TimeSheetID", item.TimeSheetID)
                        Dim existingTimestamp As Object = checkCmd.ExecuteScalar()

                        If existingTimestamp Is Nothing OrElse (TypeOf existingTimestamp Is DateTime AndAlso DirectCast(existingTimestamp, DateTime) < item.Timestamp) Then
                            ' Insert or update the record if it doesn't exist or if the Timestamp is newer
                            Dim query As String = "INSERT OR REPLACE INTO TimeSheetT (TimeSheetID, [Employee Name], Location, [Second Location], [Date], [Start Time], [End Time], Timestamp, Deleted) VALUES (@TimeSheetID, @EmployeeName, @Location, @SecondLocation, @Date, @StartTime, @EndTime, @Timestamp, @Deleted)"

                            Using cmd As New SQLiteCommand(query, connection)
                                cmd.Parameters.AddWithValue("@TimeSheetID", item.TimeSheetID)
                                cmd.Parameters.AddWithValue("@EmployeeName", item.EmployeeName)
                                cmd.Parameters.AddWithValue("@Location", item.Location)
                                cmd.Parameters.AddWithValue("@SecondLocation", item.SecondLocation)
                                cmd.Parameters.AddWithValue("@Date", item.Date)
                                cmd.Parameters.AddWithValue("@StartTime", item.StartTime)
                                cmd.Parameters.AddWithValue("@EndTime", item.EndTime)
                                cmd.Parameters.AddWithValue("@Timestamp", item.Timestamp)
                                cmd.Parameters.AddWithValue("@Deleted", item.Deleted)

                                ' Execute the query
                                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                                ' Check if a record was updated or inserted
                                If rowsAffected > 0 Then
                                    syncedRecords += 1
                                End If
                            End Using
                        End If
                    End Using
                Next
            End Using
            ' Display the number of synced records
            LogWarning($"Data inserted or updated in the local SQLite database. Synced Records: {syncedRecords}", Color.Green)
        Catch ex As Exception
            LogWarning("An error occurred while inserting data into the local SQLite database: " & ex.Message, Color.Red)
        End Try
    End Sub
    Public Class TimeEntryData
        Public Property TimeSheetID As Guid

        <JsonProperty("Employee Name")>
        Public Property EmployeeName As String
        Public Property [Date] As Date

        <JsonProperty("Start Time")>
        Public Property StartTime As TimeSpan?

        <JsonProperty("End Time")>
        Public Property EndTime As TimeSpan?
        Public Property Location As String

        <JsonProperty("Second Location")>
        Public Property SecondLocation As String

        Public Property Timestamp As DateTime

        Public Property Deleted As Integer


    End Class
    Public Class ExtendedTimeSheetData
        Inherits TimeSheetData

        Public Property Distance As Double
        Public Property DayOfWeek As String
    End Class
    Private Async Function LoadTimeSheetDataFromDatabase() As Task
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        Dim timeSheets As New List(Of ExtendedTimeSheetData)

        Try
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                Dim travel1 As List(Of TravelData) = Await LoadTravelDataFromDatabase()

                ' Get the selected EmployeeName from the ComboBox
                Dim selectedEmployeeName As String = employeeNameCb.Text

                ' Modify the SQL query to filter records by selected EmployeeName and the selected week
                Dim query As String
                If ShowDeletedCheckBox.Checked Then
                    query = "SELECT TimeSheetID, [Employee Name], Location, [Second Location], [Date], [Start Time], [End Time], [Timestamp], [Deleted] FROM TimeSheetT " &
                          "WHERE [Employee Name] = @EmployeeName AND [Date] >= @StartDate AND [Date] <= @EndDate"
                Else
                    query = "SELECT TimeSheetID, [Employee Name], Location, [Second Location], [Date], [Start Time], [End Time], [Timestamp], [Deleted] FROM TimeSheetT " &
                          "WHERE [Employee Name] = @EmployeeName AND [Date] >= @StartDate AND [Date] <= @EndDate AND [Deleted] = 0"
                End If

                Using cmd As New SQLiteCommand(query, connection)
                    ' Add parameters for the selected EmployeeName and the selected week's start and end dates
                    cmd.Parameters.AddWithValue("@EmployeeName", selectedEmployeeName)
                    cmd.Parameters.AddWithValue("@StartDate", WeekStartDate()) ' WeekStartDate() is a custom function to get the start date of the selected week
                    cmd.Parameters.AddWithValue("@EndDate", WeekEndDate())     ' WeekEndDate() is a custom function to get the end date of the selected week

                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim timeSheetID As Guid = reader.GetGuid(0)
                            Dim employeeName As String = reader.GetString(1)
                            Dim location As String = reader.GetString(2)
                            Dim secondLocation As String = reader.GetString(3)
                            Dim dateStr As String = reader.GetString(4)
                            Dim startTimeStr As String = reader.GetString(5)
                            Dim endTimeStr As String = reader.GetString(6)
                            Dim timestamp As DateTime = reader.GetDateTime(7)
                            Dim Deleted As Integer = reader.GetInt32(8)

                            ' Convert the parsed date to the desired format
                            Dim [date] As String = dateStr
                            Dim startTime As TimeSpan = TimeSpan.Parse(startTimeStr)
                            Dim endTime As TimeSpan = TimeSpan.Parse(endTimeStr)

                            ' Create an instance of the extended class and populate it
                            Dim extendedTimeSheet As New ExtendedTimeSheetData() With {
                            .TimeSheetID = timeSheetID,
                            .EmployeeName = employeeName,
                            .Location = location,
                            .SecondLocation = secondLocation,
                            .Date = [date],
                            .StartTime = startTime,
                            .EndTime = endTime,
                            .Timestamp = timestamp,
                            .Deleted = Deleted,
                            .Distance = CalculateDistance(location, secondLocation, travel1),
                            .DayOfWeek = timestamp.ToString("dddd") ' Calculate the day of the week
                        }

                            timeSheets.Add(extendedTimeSheet)
                        End While
                    End Using
                End Using
            End Using

            ' Create a DataTable to store the data
            Dim dataTable As New DataTable()

            ' Define columns for the DataTable

            dataTable.Columns.Add("Date", GetType(Date))
            dataTable.Columns.Add("DayOfWeek", GetType(String))
            dataTable.Columns.Add("EmployeeName", GetType(String))
            dataTable.Columns.Add("Location", GetType(String))
            dataTable.Columns.Add("SecondLocation", GetType(String))
            dataTable.Columns.Add("StartTime", GetType(String)) ' Change the data type to String for formatted time
            dataTable.Columns.Add("EndTime", GetType(String))   ' Change the data type to String for formatted time
            dataTable.Columns.Add("Timestamp", GetType(DateTime))
            dataTable.Columns.Add("Deleted", GetType(Integer))
            dataTable.Columns.Add("Distance", GetType(Double))
            dataTable.Columns.Add("TimeSheetID", GetType(Guid))

            ' Get the selected week's start date
            Dim startDate As Date = WeekStartDate()

            ' Add rows for each day of the week
            For i As Integer = 0 To 6
                Dim currentDate As Date = startDate.AddDays(i)
                Dim dayOfWeek As String = currentDate.ToString("dddd")

                ' Check if a record exists for the current day
                Dim matchingRecords = timeSheets.Where(Function(t) t.Date = currentDate).ToList()

                If matchingRecords.Count > 0 Then
                    ' Add each matching record as a separate row
                    For Each record In matchingRecords
                        ' Format the StartTime and EndTime to HH:mm tt (AM/PM) as strings
                        Dim formattedStartTime As String = record.[Date].Add(record.StartTime).ToString("hh:mm tt")
                        Dim formattedEndTime As String = record.[Date].Add(record.EndTime).ToString("hh:mm tt")

                        dataTable.Rows.Add(
                currentDate,
                dayOfWeek,
                record.EmployeeName,
                record.Location,
                record.SecondLocation,
                formattedStartTime,
                formattedEndTime,
                record.Timestamp,
                record.Deleted,
                record.Distance,
                record.TimeSheetID
            )
                    Next
                Else
                    ' If no record exists, add a row with default values
                    dataTable.Rows.Add(
            currentDate,
            dayOfWeek,
            SelectedEmployeeName,
            "",
            "",
            "00:00 AM",  ' Set default values for StartTime and EndTime
            "00:00 AM",
            DateTime.MinValue,
            0,
            0.0
        )
                End If
            Next

            ' Set the DataSource of DataGridView1 to the DataTable
            DataGridView1.DataSource = dataTable

            ' Check if DayOfWeekColumn exists in DataGridView.
            If DataGridView1.Columns.Contains("DayOfWeek") Then
                ' If it exists, update the header text to "Day of the Week"
                DataGridView1.Columns("DayOfWeek").HeaderText = "Day of the Week"
            End If

            DataGridView1.Columns("EmployeeName").HeaderText = "Employee Name"
            DataGridView1.Columns("SecondLocation").HeaderText = "Second Location"
            DataGridView1.Columns("StartTime").HeaderText = "Start Time"
            DataGridView1.Columns("EndTime").HeaderText = "End Time"

            ' Highlight missing days of the week in red
            For Each row As DataGridViewRow In DataGridView1.Rows
                Dim locationStr As String = row.Cells("Location").Value.ToString()

                ' Check if the record is missing (empty or DBNull)
                If String.IsNullOrEmpty(locationStr) OrElse locationStr.Equals(DBNull.Value) Then
                    For Each cell As DataGridViewCell In row.Cells
                        cell.Style.BackColor = Color.Red ' Set the font color to red
                    Next
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("An error occurred while loading timesheet data from the local database: " & ex.Message)
        End Try
    End Function






    'Formatting DataGridView1 to show 1 week at a time and only for the selected employee
    Private Function WeekStartDate() As Date
        ' Assuming DateTimePicker1 is your date picker control
        Dim selectedDate As Date = DateTimePicker1.Value.Date
        Dim dayDiff As Integer = If(selectedDate.DayOfWeek = DayOfWeek.Sunday, 6, selectedDate.DayOfWeek - DayOfWeek.Monday)
        Dim startDate As Date = selectedDate.AddDays(-dayDiff)
        Return startDate
    End Function
    Private Function WeekEndDate() As Date
        ' Assuming DateTimePicker1 is your date picker control
        Dim startDate As Date = WeekStartDate()
        Dim endDate As Date = startDate.AddDays(6)
        Return endDate
    End Function
    Private Function SelectedEmployeeName() As String
        ' Assuming employeeNameCb is your ComboBox control
        Return employeeNameCb.Text
    End Function






    'Calculate distance for dataGridView1
    Private Function CalculateDistance(location As String, secondLocation As String, travelData As List(Of TravelData)) As Double
        Dim distance As Double = 0

        ' Iterate through the travel data to find a matching entry
        For Each travelEntry In travelData
            If (location = travelEntry.StoreStart AndAlso secondLocation = travelEntry.StoreEnd) Or
           (location = travelEntry.StoreEnd AndAlso secondLocation = travelEntry.StoreStart) Then
                ' Matching entry found, use its distance
                distance = travelEntry.Distance
                Exit For
            End If
        Next

        Return distance
    End Function





    'syncs changes between both databases to have the newest timestamped record
    Private Async Sub SyncDatabases()
        Try
            ' Retrieve data from the remote API using the TimeEntryData class
            Dim remoteData As List(Of TimeEntryData) = Await RetrieveDataFromRemoteAPI()

            ' Retrieve data from the local SQLite database using the TimeEntryData class
            Dim localData As List(Of TimeEntryData) = RetrieveDataFromLocalDatabase()

            ' Compare and update records in the local SQLite database
            UpdateLocalDatabase(remoteData, localData)

            ' Compare and update records in the remote database
            UpdateRemoteDatabase(localData, remoteData)
        Catch ex As Exception
            ' Handle any exceptions that may occur during the process
            Console.WriteLine("An error occurred during database synchronization: " & ex.Message)
        End Try
        LogWarning("Connected. Databases synced", Color.Green)
    End Sub
    Private Function RetrieveDataFromLocalDatabase() As List(Of TimeEntryData)
        Dim data As New List(Of TimeEntryData)

        Try
            ' Define the file path for the SQLite database
            Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

            ' Create the connection string with the database file path
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"

            ' Open a connection to the SQLite database
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                ' Define a query to retrieve data from the TimeSheetT table
                Dim query As String = "SELECT TimeSheetID, [Employee Name], Location, [Second Location], [Date], [Start Time], [End Time], [Timestamp], [Deleted] FROM TimeSheetT"

                ' Create a command to execute the query
                Using command As New SQLiteCommand(query, connection)
                    ' Execute the query and retrieve data
                    Using reader As SQLiteDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim timeEntry As New TimeEntryData()
                            ' Populate the TimeEntryData object with data from the database
                            timeEntry.TimeSheetID = reader.GetGuid(reader.GetOrdinal("TimeSheetID"))
                            timeEntry.EmployeeName = reader.GetString(reader.GetOrdinal("Employee Name"))
                            timeEntry.Location = reader.GetString(reader.GetOrdinal("Location"))
                            timeEntry.SecondLocation = reader.GetString(reader.GetOrdinal("Second Location"))
                            timeEntry.Date = reader.GetDateTime(reader.GetOrdinal("Date"))

                            Dim startTimeStr As String = reader.GetString(reader.GetOrdinal("Start Time"))
                            timeEntry.StartTime = TimeSpan.Parse(startTimeStr)

                            Dim endTimeStr As String = reader.GetString(reader.GetOrdinal("End Time"))
                            timeEntry.EndTime = TimeSpan.Parse(endTimeStr)




                            timeEntry.Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
                            timeEntry.Deleted = reader.GetInt32(reader.GetOrdinal("Deleted"))

                            ' Add the retrieved data to the list
                            data.Add(timeEntry)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Handle any exceptions that may occur during the data retrieval
            Console.WriteLine("An error occurred while retrieving data from the local database: " & ex.Message)
        End Try

        ' Return the retrieved data
        Return data
    End Function



    Private Async Function RetrieveDataFromRemoteAPI() As Task(Of List(Of TimeEntryData))
        Dim data As New List(Of TimeEntryData)

        Try
            ' Create an instance of HttpClient
            Dim httpClient As New HttpClient()

            ' Set the base address for your API
            httpClient.BaseAddress = New Uri(baseApiUrl)

            ' Set the API key in the request headers
            httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey)


            ' Make GET request to retrieve data from the API
            Dim response As HttpResponseMessage = Await httpClient.GetAsync("api/TimeSheet") ' Updated endpoint


            If response.IsSuccessStatusCode Then
                ' Deserialize the response content into a data structure
                Dim content As String = Await response.Content.ReadAsStringAsync()
                data = JsonConvert.DeserializeObject(Of List(Of TimeEntryData))(content)
            Else
                ' Handle errors
                MessageBox.Show("Failed to retrieve timesheet data from the API.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the API request
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

        ' Return the list of time entries retrieved from the remote API
        Return data
    End Function
    Private Sub UpdateLocalDatabase(remoteData As List(Of TimeEntryData), localData As List(Of TimeEntryData))
        Try
            ' Define the file path for the SQLite database
            Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

            ' Create the connection string with the database file path
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"

            Dim syncedRecords As Integer = 0

            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                For Each remoteRecord As TimeEntryData In remoteData
                    ' Find the corresponding record in localData
                    Dim localRecord As TimeEntryData = localData.FirstOrDefault(Function(r) r.TimeSheetID = remoteRecord.TimeSheetID)

                    ' Check if the record exists locally and if the remote record's timestamp is newer
                    If localRecord IsNot Nothing AndAlso remoteRecord.Timestamp > localRecord.Timestamp Then
                        ' Insert or update the record if it doesn't exist or if the Timestamp is newer
                        Dim query As String = "INSERT OR REPLACE INTO TimeSheetT (TimeSheetID, [Employee Name], Location, [Second Location], [Date], [Start Time], [End Time], Timestamp, Deleted) VALUES (@TimeSheetID, @EmployeeName, @Location, @SecondLocation, @Date, @StartTime, @EndTime, @Timestamp, @Deleted)"

                        Using cmd As New SQLiteCommand(query, connection)
                            cmd.Parameters.AddWithValue("@TimeSheetID", remoteRecord.TimeSheetID)
                            cmd.Parameters.AddWithValue("@EmployeeName", remoteRecord.EmployeeName)
                            cmd.Parameters.AddWithValue("@Location", remoteRecord.Location)
                            cmd.Parameters.AddWithValue("@SecondLocation", remoteRecord.SecondLocation)
                            cmd.Parameters.AddWithValue("@Date", remoteRecord.Date)
                            cmd.Parameters.AddWithValue("@StartTime", remoteRecord.StartTime)
                            cmd.Parameters.AddWithValue("@EndTime", remoteRecord.EndTime)
                            cmd.Parameters.AddWithValue("@Timestamp", remoteRecord.Timestamp)
                            cmd.Parameters.AddWithValue("@Deleted", remoteRecord.Deleted)

                            ' Execute the query
                            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                            ' Check if a record was updated or inserted
                            If rowsAffected > 0 Then
                                syncedRecords += 1
                            End If
                        End Using
                    End If
                Next
            End Using

            ' Display the number of synced records for the local database
            Console.WriteLine($"Data inserted or updated in the local SQLite database. Synced Records: {syncedRecords}")
        Catch ex As Exception
            Console.WriteLine("An error occurred while updating data in the local SQLite database: " & ex.Message)
        End Try
    End Sub

    Private Sub UpdateRemoteDatabase(localData As List(Of TimeEntryData), remoteData As List(Of TimeEntryData))
        Try
            ' Compare the timestamps and update the remote database with newer records

            Using httpClient As New HttpClient()
                httpClient.BaseAddress = New Uri(baseApiUrl)
                httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey)

                For Each localRecord As TimeEntryData In localData
                    ' Find the corresponding record in remoteData
                    Dim remoteRecord As TimeEntryData = remoteData.FirstOrDefault(Function(r) r.TimeSheetID = localRecord.TimeSheetID)

                    If remoteRecord Is Nothing Then
                        ' Record doesn't exist remotely, construct an insert request to your remote API
                        Dim insertData As New TimeEntryData()
                        insertData.TimeSheetID = localRecord.TimeSheetID
                        insertData.EmployeeName = localRecord.EmployeeName
                        insertData.Date = localRecord.Date
                        insertData.StartTime = localRecord.StartTime
                        insertData.EndTime = localRecord.EndTime
                        insertData.Location = localRecord.Location
                        insertData.SecondLocation = localRecord.SecondLocation
                        insertData.Timestamp = localRecord.Timestamp ' Include the Timestamp field
                        insertData.Deleted = localRecord.Deleted


                        ' Serialize insertData to JSON
                        Dim jsonInsertData As String = JsonConvert.SerializeObject(insertData)

                        ' Make an API request to insert the record into the remote database
                        Dim insertResponse As HttpResponseMessage = httpClient.PostAsync("api/timeentries", New StringContent(jsonInsertData, Encoding.UTF8, "application/json")).Result

                        If insertResponse.IsSuccessStatusCode Then
                            ' Handle success (e.g., log success or perform additional actions)
                            Console.WriteLine($"Record with TimeSheetID {localRecord.TimeSheetID} inserted into the remote database.")
                        Else
                            ' Handle API insert failure
                            Dim errorResponse As String = insertResponse.Content.ReadAsStringAsync().Result
                            Console.WriteLine("API Error Response: " & errorResponse)
                            Console.WriteLine($"Failed to insert record with TimeSheetID {localRecord.TimeSheetID} into the remote database.")
                        End If
                    Else
                        ' Record exists remotely, compare timestamps
                        If localRecord.Timestamp > remoteRecord.Timestamp Then
                            ' Local record is newer, construct an update request to your remote API
                            Dim updateData As New TimeEntryData()
                            updateData.TimeSheetID = localRecord.TimeSheetID
                            updateData.EmployeeName = localRecord.EmployeeName
                            updateData.Date = localRecord.Date
                            updateData.StartTime = localRecord.StartTime
                            updateData.EndTime = localRecord.EndTime
                            updateData.Location = localRecord.Location
                            updateData.SecondLocation = localRecord.SecondLocation
                            updateData.Timestamp = localRecord.Timestamp ' Include the Timestamp field
                            updateData.Deleted = localRecord.Deleted ' Include the Deleted field

                            ' Serialize updateData to JSON
                            Dim jsonUpdateData As String = JsonConvert.SerializeObject(updateData)

                            ' Make an API request to update the record in the remote database
                            Dim updateResponse As HttpResponseMessage = httpClient.PutAsync($"api/timeentries/{localRecord.TimeSheetID}", New StringContent(jsonUpdateData, Encoding.UTF8, "application/json")).Result

                            If updateResponse.IsSuccessStatusCode Then
                                ' Handle success (e.g., log success or perform additional actions)
                                Console.WriteLine($"Record with TimeSheetID {localRecord.TimeSheetID} updated in the remote database.")
                            Else
                                ' Handle API update failure
                                Dim errorResponse As String = updateResponse.Content.ReadAsStringAsync().Result
                                Console.WriteLine("API Error Response: " & errorResponse)
                                Console.WriteLine($"Failed to update record with TimeSheetID {localRecord.TimeSheetID} in the remote database.")
                            End If
                        End If
                    End If
                Next
            End Using
        Catch ex As Exception
            ' Handle any exceptions that may occur during the process
            Console.WriteLine("An error occurred while updating data in the remote database: " & ex.Message)
        End Try
    End Sub





    'Employee Name Data Merging 
    Public Class EmployeeData
        Public Property EmployeeID As Integer

        <JsonProperty("Employee Name")>
        Public Property EmployeeName As String
        ' Add other properties as needed
    End Class
    Private Async Function LoadDataEmployeeName() As Task(Of List(Of EmployeeData))
        Dim employees As New List(Of EmployeeData)

        Try
            ' Create an instance of HttpClient
            Dim httpClient As New HttpClient()

            ' Set the base address for your API
            httpClient.BaseAddress = New Uri(baseApiUrl)

            ' Set the API key in the request headers
            httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey)

            ' Make GET request to retrieve data from the API
            Dim response As HttpResponseMessage = Await httpClient.GetAsync("api/Employee")

            If response.IsSuccessStatusCode Then
                ' Deserialize the response content into a data structure
                Dim content As String = Await response.Content.ReadAsStringAsync()
                employees = JsonConvert.DeserializeObject(Of List(Of EmployeeData))(content)
            Else
                ' Handle errors
                MessageBox.Show("Failed to retrieve data from the API 1.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the API request
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

        Return employees
    End Function
    Private Async Function MergeEmployeeData() As Task

        Try
            ' Define and load the apiEmployeeData variable with data from the API
            Dim apiEmployeeData As List(Of EmployeeData) = Await LoadDataEmployeeName()

            ' Load data from the local SQLite database for employees
            Dim localEmployeeData As List(Of EmployeeData) = Await LoadEmployeeDataFromDatabase()

            ' Merge the two data sources
            Dim mergedEmployeeData As List(Of EmployeeData) = MergeData(apiEmployeeData, localEmployeeData, Function(e) e.EmployeeName)

            ' Update the ComboBox with the merged data
            employeeNameCb.DataSource = mergedEmployeeData
            employeeNameCb.DisplayMember = "EmployeeName" ' Replace with the actual property name

            SaveEmployeeDataToDatabase(mergedEmployeeData)

        Catch ex As Exception
            ' Handle any exceptions that may occur during the merge process
            MessageBox.Show("An error occurred while merging data: " & ex.Message)
        End Try
    End Function
    Private Async Function LoadEmployeeDataFromDatabase() As Task(Of List(Of EmployeeData))
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        Dim employees As New List(Of EmployeeData)

        Try
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"
            Using connection As New SQLiteConnection(connectionString)
                Await connection.OpenAsync() ' Use asynchronous open

                ' Modify the query to include EmployeeID
                Dim query As String = "SELECT EmployeeID, [Employee Name] FROM EmployeeT"
                Using cmd As New SQLiteCommand(query, connection)
                    Using reader As SQLiteDataReader = Await cmd.ExecuteReaderAsync() ' Use asynchronous reader
                        While Await reader.ReadAsync() ' Use asynchronous read
                            Dim employeeID As Integer = reader.GetInt32(0)
                            Dim employeeName As String = reader.GetString(1)
                            employees.Add(New EmployeeData() With {.EmployeeID = employeeID, .EmployeeName = employeeName})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading data from the local database: " & ex.Message)
        End Try
        employeeNameCb.DataSource = employees
        employeeNameCb.DisplayMember = "EmployeeName"
        Return employees
    End Function
    Private Sub SaveEmployeeDataToDatabase(employees As List(Of EmployeeData))
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        Try
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                ' Retrieve existing records from the EmployeeT table
                Dim existingRecords As New Dictionary(Of Integer, String) ' Use a dictionary to store both ID and Name
                Dim selectQuery As String = "SELECT EmployeeID, [Employee Name] FROM EmployeeT"
                Using selectCmd As New SQLiteCommand(selectQuery, connection)
                    Using reader As SQLiteDataReader = selectCmd.ExecuteReader()
                        While reader.Read()
                            Dim employeeID As Integer = reader.GetInt32(0)
                            Dim employeeName As String = reader.GetString(1)
                            existingRecords.Add(employeeID, employeeName)
                        End While
                    End Using
                End Using

                ' Identify records that need to be updated and new records to be inserted
                Dim recordsToUpdate As New List(Of EmployeeData)
                Dim recordsToInsert As New List(Of EmployeeData)

                For Each employee As EmployeeData In employees
                    If existingRecords.ContainsValue(employee.EmployeeName) Then
                        ' Find the EmployeeID corresponding to the EmployeeName
                        Dim existingEmployeeID As Integer = existingRecords.FirstOrDefault(Function(pair) pair.Value = employee.EmployeeName).Key
                        employee.EmployeeID = existingEmployeeID ' Update EmployeeID
                        recordsToUpdate.Add(employee)
                    Else
                        recordsToInsert.Add(employee)
                    End If
                Next

                ' Update existing records
                Dim updateQuery As String = "UPDATE EmployeeT SET [Employee Name] = @EmployeeName WHERE EmployeeID = @EmployeeID"
                Using updateCmd As New SQLiteCommand(updateQuery, connection)
                    For Each employee As EmployeeData In recordsToUpdate
                        updateCmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName)
                        updateCmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID)
                        updateCmd.ExecuteNonQuery()
                        updateCmd.Parameters.Clear()
                    Next
                End Using

                ' Insert new records
                Dim insertQuery As String = "INSERT INTO EmployeeT ([Employee Name]) VALUES (@EmployeeName)"
                Using insertCmd As New SQLiteCommand(insertQuery, connection)
                    For Each employee As EmployeeData In recordsToInsert
                        insertCmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName)
                        insertCmd.ExecuteNonQuery()
                        insertCmd.Parameters.Clear()
                    Next
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving data to the local database: " & ex.Message)
        End Try
    End Sub



    ' Store Data Merging 
    Public Class StoreData
        Public Property StoreID As Integer

        Public Property StoreName As String
    End Class
    Private Async Function LoadStoreData() As Task(Of List(Of StoreData))
        Dim stores As New List(Of StoreData)

        Try
            ' Create an instance of HttpClient
            Dim httpClient As New HttpClient()

            ' Set the base address for your API
            httpClient.BaseAddress = New Uri(baseApiUrl)

            ' Set the API key in the request headers
            httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey)

            ' Make GET request to retrieve data from the API
            Dim response As HttpResponseMessage = Await httpClient.GetAsync("api/Store")

            ' Print the response status code
            Console.WriteLine("API Response Status Code: " & response.StatusCode.ToString())

            If response.IsSuccessStatusCode Then
                ' Deserialize the response content into a data structure
                Dim content As String = Await response.Content.ReadAsStringAsync()
                stores = JsonConvert.DeserializeObject(Of List(Of StoreData))(content)

            Else
                ' Handle errors
                MessageBox.Show("Failed to retrieve store data from the API.2")
            End If

        Catch ex As Exception
            ' Handle any exceptions that may occur during the API request
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

        ' Return the list of stores
        Return stores
    End Function
    Private Async Function MergeStoreData() As Task
        Try
            ' Load data from the API
            Dim apiData As List(Of StoreData) = Await LoadStoreData()

            ' Load data from the local SQLite database
            Dim localData As List(Of StoreData) = Await LoadStoreDataFromDatabase()

            If apiData IsNot Nothing Then
                ' Merge the two data sources
                Dim mergedStoreData As List(Of StoreData) = MergeData(apiData, localData, Function(s) s.StoreName)

                ' Update the ComboBox with the merged data
                storeCb1.DataSource = mergedStoreData
                storeCb1.DisplayMember = "StoreName" ' Replace with the actual property name
                storeCb2.DataSource = mergedStoreData
                storeCb2.DisplayMember = "StoreName" ' Replace with the actual property name

                ' Create separate bindings for the SelectedItem property of each ComboBox
                Dim storeBinding1 As New Binding("SelectedItem", mergedStoreData, "StoreName")
                Dim storeBinding2 As New Binding("SelectedItem", mergedStoreData, "StoreName")

                ' Bind the ComboBoxes to the data source with the separate bindings
                storeCb1.DataBindings.Add(storeBinding1)
                storeCb2.DataBindings.Add(storeBinding2)

                SaveStoreDataToDatabase(mergedStoreData)
            Else
                ' Handle the case where apiData is empty or null
                MessageBox.Show("The API did not return any store data.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the merge process
            MessageBox.Show("An error occurred while merging store data: " & ex.Message)
        End Try
    End Function
    Private Async Function LoadStoreDataFromDatabase() As Task(Of List(Of StoreData))
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        Dim stores As New List(Of StoreData)

        Try
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"
            Using connection As New SQLiteConnection(connectionString)
                Await connection.OpenAsync()

                Dim query As String = "SELECT ID, Store FROM StoreT"
                Using cmd As New SQLiteCommand(query, connection)
                    Using reader As SQLiteDataReader = Await cmd.ExecuteReaderAsync()
                        While Await reader.ReadAsync()
                            Dim storeID As Integer = reader.GetInt32(0)
                            Dim storeName As String = reader.GetString(1)
                            stores.Add(New StoreData() With {.StoreID = storeID, .StoreName = storeName})


                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading store data from the local database: " & ex.Message)
        End Try

        ' Update the ComboBox with the merged data
        storeCb1.DataSource = stores
        storeCb1.DisplayMember = "StoreName"

        storeCb2.DataSource = stores.ToList
        storeCb2.DisplayMember = "StoreName"

        ' Return an empty list if an exception occurs or no data is loaded
        Return stores
    End Function
    Private Sub SaveStoreDataToDatabase(stores As List(Of StoreData))
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        Try
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                ' Retrieve existing records from the StoreT table
                Dim existingRecords As New Dictionary(Of Integer, String) ' Use a dictionary to store both ID and Name
                Dim selectQuery As String = "SELECT ID, Store FROM StoreT"
                Using selectCmd As New SQLiteCommand(selectQuery, connection)
                    Using reader As SQLiteDataReader = selectCmd.ExecuteReader()
                        While reader.Read()
                            Dim storeID As Integer = reader.GetInt32(0)
                            Dim storeName As String = reader.GetString(1)
                            existingRecords.Add(storeID, storeName)
                        End While
                    End Using
                End Using

                ' Identify records that need to be updated and new records to be inserted
                Dim recordsToUpdate As New List(Of StoreData)
                Dim recordsToInsert As New List(Of StoreData)

                For Each store As StoreData In stores
                    If existingRecords.ContainsValue(store.StoreName) Then
                        ' Find the StoreID corresponding to the StoreName
                        Dim existingStoreID As Integer = existingRecords.FirstOrDefault(Function(pair) pair.Value = store.StoreName).Key
                        store.StoreID = existingStoreID ' Update StoreID
                        recordsToUpdate.Add(store)
                    Else
                        recordsToInsert.Add(store)
                    End If
                Next

                ' Update existing records
                Dim updateQuery As String = "UPDATE StoreT SET Store = @StoreName WHERE ID = @StoreID"
                Using updateCmd As New SQLiteCommand(updateQuery, connection)
                    For Each store As StoreData In recordsToUpdate
                        updateCmd.Parameters.AddWithValue("@StoreName", store.StoreName)
                        updateCmd.Parameters.AddWithValue("@StoreID", store.StoreID)
                        updateCmd.ExecuteNonQuery()
                        updateCmd.Parameters.Clear()
                    Next
                End Using

                ' Insert new records
                Dim insertQuery As String = "INSERT INTO StoreT (Store) VALUES (@StoreName)"
                Using insertCmd As New SQLiteCommand(insertQuery, connection)
                    For Each store As StoreData In recordsToInsert
                        insertCmd.Parameters.AddWithValue("@StoreName", store.StoreName)
                        insertCmd.ExecuteNonQuery()
                        insertCmd.Parameters.Clear()
                    Next
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving store data to the local database: " & ex.Message)
        End Try
    End Sub







    'Travel Distance - Loading Code
    Public Class TravelData
        Public Property ID As Integer

        <JsonProperty("Store Start")>
        Public Property StoreStart As String

        <JsonProperty("Store End")>
        Public Property StoreEnd As String

        <JsonProperty("Distance (km)")>
        Public Property Distance As Double
    End Class
    Private Async Function LoadTravelData() As Task(Of List(Of TravelData))
        Dim travel As New List(Of TravelData)

        Try
            ' Create an instance of HttpClient
            Dim httpClient As New HttpClient()

            ' Set the base address for your API
            httpClient.BaseAddress = New Uri(baseApiUrl)

            ' Set the API key in the request headers
            httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey)

            ' Make GET request to retrieve data from the API
            Dim response As HttpResponseMessage = Await httpClient.GetAsync("api/Travel")

            ' Print the response status code
            Console.WriteLine("API Response Status Code: " & response.StatusCode.ToString())

            If response.IsSuccessStatusCode Then
                ' Deserialize the response content into a data structure
                Dim content As String = Await response.Content.ReadAsStringAsync()
                travel = JsonConvert.DeserializeObject(Of List(Of TravelData))(content)
            Else
                ' Handle errors
                MessageBox.Show("Failed to retrieve travel data from the API.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the API request
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

        ' Return the list of travel data
        Return travel
    End Function
    Private Async Function MergeTravelData() As Task
        Try
            ' Load data from the API
            Dim apiData As List(Of TravelData) = Await LoadTravelData()

            ' Load data from the local SQLite database
            Dim localData As List(Of TravelData) = Await LoadTravelDataFromDatabase()

            If apiData IsNot Nothing Then
                ' Merge the two data sources based on some property (e.g., ID)
                Dim mergedTravelData As List(Of TravelData) = MergeData(apiData, localData, Function(t) t.ID)

                ' Implement your logic to update the user interface and save data to the local database.
                ' For example, set DataGridView's DataSource to mergedTravelData
                SaveTravelDataToDatabase(mergedTravelData) ' and save mergedTravelData to the local database using SaveTravelDataToDatabase.
            Else
                ' Handle the case where apiData is empty or null
                MessageBox.Show("The API did not return any travel data.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the merge process
            MessageBox.Show("An error occurred while merging travel data: " & ex.Message)
        End Try
    End Function
    Private Async Function LoadTravelDataFromDatabase() As Task(Of List(Of TravelData))
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        Dim travelData As New List(Of TravelData)

        Try
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"

            ' Open the database connection asynchronously
            Using connection As New SQLiteConnection(connectionString)
                Await connection.OpenAsync()

                Dim query As String = "SELECT ID, [Store Start], [Store End], [Distance (km)] FROM TravelT"

                ' Create and execute the command asynchronously
                Using cmd As SQLiteCommand = New SQLiteCommand(query, connection)
                    Using reader As SQLiteDataReader = Await cmd.ExecuteReaderAsync()
                        While Await reader.ReadAsync()
                            Dim id As Integer = reader.GetInt32(0)
                            Dim storeStart As String = reader.GetString(1)
                            Dim storeEnd As String = reader.GetString(2)
                            Dim distance As Double = reader.GetDouble(3)

                            travelData.Add(New TravelData() With {
                            .ID = id,
                            .StoreStart = storeStart,
                            .StoreEnd = storeEnd,
                            .Distance = distance.ToString()
                        })
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading travel data from the local database: " & ex.Message)
        End Try

        ' Return an empty list if an exception occurs or no data is loaded
        Return travelData


        If travelData.Count > 0 Then
            Dim message As New StringBuilder()
            For Each travelItem As TravelData In travelData
                message.AppendLine($"ID: {travelItem.ID}, Store Start: {travelItem.StoreStart}, Store End: {travelItem.StoreEnd}, Distance: {travelItem.Distance} km")
            Next

            MessageBox.Show(message.ToString(), "Travel Data")
        Else
            MessageBox.Show("No travel data found.", "Travel Data")
        End If
    End Function
    Private Sub SaveTravelDataToDatabase(travelData As List(Of TravelData))
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        Try
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                ' Retrieve existing records from the TravelT table
                Dim existingRecords As New Dictionary(Of Integer, String) ' Use a dictionary to store both ID and Name
                Dim selectQuery As String = "SELECT ID, [Store Start], [Store End], [Distance (km)] FROM TravelT"
                Using selectCmd As New SQLiteCommand(selectQuery, connection)
                    Using reader As SQLiteDataReader = selectCmd.ExecuteReader()
                        While reader.Read()
                            Dim id As Integer = reader.GetInt32(0)
                            Dim storeStart As String = reader.GetString(1)
                            Dim storeEnd As String = reader.GetString(2)
                            Dim distance As Double = reader.GetDouble(3)

                            ' Use storeStart and storeEnd to identify existing records
                            Dim key As String = $"{storeStart}_{storeEnd}"
                            existingRecords.Add(id, key)
                        End While
                    End Using
                End Using

                ' Identify records that need to be updated and new records to be inserted
                Dim recordsToUpdate As New List(Of TravelData)
                Dim recordsToInsert As New List(Of TravelData)

                For Each travel As TravelData In travelData
                    ' Use storeStart and storeEnd to identify existing records
                    Dim key As String = $"{travel.StoreStart}_{travel.StoreEnd}"
                    If existingRecords.ContainsValue(key) Then
                        ' Find the ID corresponding to the StoreStart and StoreEnd
                        Dim existingID As Integer = existingRecords.FirstOrDefault(Function(pair) pair.Value = key).Key
                        travel.ID = existingID ' Update ID
                        recordsToUpdate.Add(travel)
                    Else
                        recordsToInsert.Add(travel)
                    End If
                Next

                ' Update existing records
                Dim updateQuery As String = "UPDATE TravelT SET [Store Start] = @StoreStart, [Store End] = @StoreEnd, [Distance (km)] = @Distance WHERE ID = @ID"
                Using updateCmd As New SQLiteCommand(updateQuery, connection)
                    For Each travel As TravelData In recordsToUpdate
                        updateCmd.Parameters.AddWithValue("@StoreStart", travel.StoreStart)
                        updateCmd.Parameters.AddWithValue("@StoreEnd", travel.StoreEnd)
                        updateCmd.Parameters.AddWithValue("@Distance", Double.Parse(travel.Distance))
                        updateCmd.Parameters.AddWithValue("@ID", travel.ID)
                        updateCmd.ExecuteNonQuery()
                        updateCmd.Parameters.Clear()
                    Next
                End Using

                ' Insert new records
                Dim insertQuery As String = "INSERT INTO TravelT ([Store Start], [Store End], [Distance (km)]) VALUES (@StoreStart, @StoreEnd, @Distance)"
                Using insertCmd As New SQLiteCommand(insertQuery, connection)
                    For Each travel As TravelData In recordsToInsert
                        insertCmd.Parameters.AddWithValue("@StoreStart", travel.StoreStart)
                        insertCmd.Parameters.AddWithValue("@StoreEnd", travel.StoreEnd)
                        insertCmd.Parameters.AddWithValue("@Distance", Double.Parse(travel.Distance))
                        insertCmd.ExecuteNonQuery()
                        insertCmd.Parameters.Clear()
                    Next
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving travel data to the local database: " & ex.Message)
        End Try
    End Sub







    'Merge Data Function, called in multiple merge functions
    Private Function MergeData(Of T)(apiData As List(Of T), localData As List(Of T), keySelector As Func(Of T, Object)) As List(Of T)
        ' Merge the two data sources while avoiding duplicates
        Dim mergedData As New List(Of T)(localData)

        For Each apiItem As T In apiData
            Dim key = keySelector(apiItem)
            If Not mergedData.Any(Function(localItem) Object.Equals(keySelector(localItem), key)) Then
                mergedData.Add(apiItem)
            End If
        Next

        Return mergedData
    End Function



    'Add Time Record to Database
    Private Sub AddTimeRecordToLocalDatabase(timeEntry As TimeEntryData)
        ' Define the file path for the SQLite database
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        ' Create the connection string with the database file path
        Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"

        Try
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                ' Check if a record with the same TimeSheetID already exists in the database
                Dim checkQuery As String = "SELECT Timestamp FROM TimeSheetT WHERE TimeSheetID = @TimeSheetID"
                Using checkCmd As New SQLiteCommand(checkQuery, connection)
                    checkCmd.Parameters.AddWithValue("@TimeSheetID", timeEntry.TimeSheetID)
                    Dim existingTimestamp As Object = checkCmd.ExecuteScalar()

                    If existingTimestamp Is Nothing OrElse (TypeOf existingTimestamp Is DateTime AndAlso DirectCast(existingTimestamp, DateTime) < timeEntry.Timestamp) Then
                        ' Insert or update the record if it doesn't exist or if the Timestamp is newer
                        Dim query As String = "INSERT OR REPLACE INTO TimeSheetT (TimeSheetID, [Employee Name], Location, [Second Location], [Date], [Start Time], [End Time], Timestamp, Deleted) VALUES (@TimeSheetID, @EmployeeName, @Location, @SecondLocation, @Date, @StartTime, @EndTime, @Timestamp, @Deleted)"

                        Using cmd As New SQLiteCommand(query, connection)
                            cmd.Parameters.AddWithValue("@TimeSheetID", timeEntry.TimeSheetID)
                            cmd.Parameters.AddWithValue("@EmployeeName", timeEntry.EmployeeName)
                            cmd.Parameters.AddWithValue("@Location", timeEntry.Location)
                            cmd.Parameters.AddWithValue("@SecondLocation", timeEntry.SecondLocation)
                            cmd.Parameters.AddWithValue("@Date", timeEntry.Date)
                            cmd.Parameters.AddWithValue("@StartTime", timeEntry.StartTime)
                            cmd.Parameters.AddWithValue("@EndTime", timeEntry.EndTime)
                            cmd.Parameters.AddWithValue("@Timestamp", timeEntry.Timestamp)
                            cmd.Parameters.AddWithValue("@Deleted", timeEntry.Deleted)

                            ' Execute the query
                            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                            ' Check if a record was updated or inserted
                            If rowsAffected > 0 Then
                                ' Handle success (e.g., refresh the data grid)
                                LogWarning("Time entry added to the local database", Color.Green)
                                RefreshData()
                            Else
                                ' Handle errors
                                LogWarning("Failed to add time entry to the local database", Color.Red)
                            End If
                        End Using
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Log the exception and show an error message
            Console.WriteLine("Error: " & ex.Message)
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub
    Private Sub addTimeButton_Click(sender As Object, e As EventArgs) Handles addTimeButton.Click
        Try
            ' Create a model class for your time entry data
            Dim timeEntry As New TimeEntryData()

            ' Generate a new Guid for TimeSheetID
            timeEntry.TimeSheetID = Guid.NewGuid()

            timeEntry.Deleted = 0

            ' Populate the time entry object with the form data
            timeEntry.EmployeeName = employeeNameCb.Text
            timeEntry.Date = DateTimePicker1.Value.Date

            Dim isNoWork As Boolean = (storeCb1.Text = "No Work" Or storeCb2.Text = "No Work")

            If Not isNoWork Then
                Dim startTime As TimeSpan = startTimePicker.Value.TimeOfDay
                Dim formattedStartTime As String = startTime.ToString("hh\:mm") 'removes the milliseconds
                timeEntry.StartTime = TimeSpan.Parse(formattedStartTime)

                Dim endTime As TimeSpan = endTimePicker.Value.TimeOfDay
                Dim formattedEndTime As String = endTime.ToString("hh\:mm") 'removes the milliseconds
                timeEntry.EndTime = TimeSpan.Parse(formattedEndTime)

                timeEntry.Location = storeCb1.Text
                timeEntry.SecondLocation = storeCb2.Text
            Else
                timeEntry.StartTime = TimeSpan.Parse("00:00:00")
                timeEntry.EndTime = TimeSpan.Parse("00:00:00")
                timeEntry.Location = "No Work"
                timeEntry.SecondLocation = ""
            End If

            ' Set the Timestamp property to the current date and time
            timeEntry.Timestamp = DateTime.Now

            ' Call the function to add the time entry to the local database
            AddTimeRecordToLocalDatabase(timeEntry)
        Catch ex As Exception
            ' Log the exception and show an error message
            Console.WriteLine("Error: " & ex.Message)
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub





    'Delete Records Functions
    Public Class TimeSheetDeleteData
        Public Property TimeSheetID As Guid
    End Class
    Private Sub deleteRecordButton_Click(sender As Object, e As EventArgs) Handles deleteRecordButton.Click
        Try
            ' Check if a row is selected
            If DataGridView1.SelectedRows.Count > 0 Then
                ' Get the ID of the selected row
                Dim selectedRowID As Guid = CType(DataGridView1.SelectedRows(0).Cells("TimeSheetID").Value, Guid)

                ' Update the local SQLite database to set the "Deleted" column to true
                UpdateDeletedFlagInLocalDatabase(selectedRowID)

                ' Handle success (e.g., refresh the data grid)
                LogWarning("Record marked as deleted successfully", Color.Green)
                RefreshData()
            Else
                MessageBox.Show("Please select a row to mark as deleted.")
            End If
        Catch ex As Exception
            ' Log the exception and show an error message
            Console.WriteLine("Error: " & ex.Message)
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub
    Private Sub UpdateDeletedFlagInLocalDatabase(selectedRowID As Guid)
        Try
            ' Define the file path for the SQLite database
            Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

            ' Create the connection string with the database file path
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"

            ' Get the current date and time
            Dim currentTimestamp As DateTime = DateTime.Now

            ' Update the "Deleted" column in the local database for the selected record
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                ' Define the update query
                Dim updateQuery As String = "UPDATE TimeSheetT SET Deleted = 1, Timestamp = @Timestamp WHERE TimeSheetID = @TimeSheetID"


                Using cmd As New SQLiteCommand(updateQuery, connection)
                    cmd.Parameters.AddWithValue("@TimeSheetID", selectedRowID)
                    cmd.Parameters.AddWithValue("@Timestamp", currentTimestamp)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' Handle any exceptions that may occur while updating the local database
            MessageBox.Show("An error occurred while updating the local SQLite database: " & ex.Message)
        End Try
    End Sub



    'Creates Local SQLite database on the local machine, if one doesnt exist
    Private Function CreateDatabaseIfNotExists() As Boolean
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

        If Not File.Exists(databaseFilePath) Then
            Try
                Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"
                Using connection As New SQLiteConnection(connectionString)
                    connection.Open()

                    ' Create the EmployeeT table with the specified properties.
                    Using createEmployeeTable As New SQLiteCommand(connection)
                        createEmployeeTable.CommandText = "CREATE TABLE IF NOT EXISTS EmployeeT (
                                        EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                                        [Employee Name] TEXT);"
                        createEmployeeTable.ExecuteNonQuery()
                    End Using

                    ' Create the StoreT table with the specified properties.
                    Using createStoreTable As New SQLiteCommand(connection)
                        createStoreTable.CommandText = "CREATE TABLE IF NOT EXISTS StoreT (
                                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Store TEXT);"
                        createStoreTable.ExecuteNonQuery()
                    End Using

                    ' Create the TimeSheetT table with the specified properties.
                    Using createTimeSheetTable As New SQLiteCommand(connection)
                        createTimeSheetTable.CommandText = "CREATE TABLE IF NOT EXISTS TimeSheetT (
                                        TimeSheetID GUID PRIMARY KEY,
                                        [Employee Name] TEXT,
                                        Location TEXT,
                                        [Second Location] TEXT,
                                        [Date] DATE,
                                        [Start Time] TIME,
                                        [End Time] TIME,
                                        [Timestamp] DATETIME,
                                        Deleted INTEGER DEFAULT 0);"
                        createTimeSheetTable.ExecuteNonQuery()
                    End Using

                    ' Create the TravelT table with the specified properties.
                    Using createTravelTable As New SQLiteCommand(connection)
                        createTravelTable.CommandText = "CREATE TABLE IF NOT EXISTS TravelT (
                                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                        [Store Start] TEXT,
                                        [Store End] TEXT,
                                        [Distance (km)] REAL);"
                        createTravelTable.ExecuteNonQuery()
                    End Using
                End Using

                Return True
            Catch ex As Exception
                MessageBox.Show("Failed to create SQLite database: " & ex.Message)
                Return False
            End Try
        End If

        Return True
    End Function



    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        ' Get the text that is displayed in the column header
        Dim columnHeaderText As String = DataGridView1.Columns(e.ColumnIndex).HeaderText

        ' Check if the edited column is one of the columns that you want to update
        If columnHeaderText = "Employee Name" OrElse columnHeaderText = "Location" OrElse columnHeaderText = "Second Location" OrElse columnHeaderText = "Date" OrElse columnHeaderText = "Start Time" OrElse columnHeaderText = "End Time" Then
            ' Get the updated value from the cell
            Dim newValue As Object = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

            If Not (TypeOf newValue Is TimeSpan) OrElse Not String.IsNullOrEmpty(newValue.ToString()) Then
                ' Validate and convert the data before updating the SQL Server database
                If columnHeaderText = "Employee Name" Then
                    ' Check if the employee name is not empty
                    If String.IsNullOrEmpty(newValue.ToString()) Then
                        MessageBox.Show("The employee name cannot be empty")
                        Return
                    End If
                ElseIf columnHeaderText = "Location" OrElse columnHeaderText = "Second Location" Then
                    ' Check if the location is not empty
                    If String.IsNullOrEmpty(newValue.ToString()) Then
                        MessageBox.Show("The location cannot be empty")
                        Return
                    End If
                ElseIf columnHeaderText = "Date" Then
                    ' Check if the date is in the correct format (yyyy-MM-dd)
                    Dim regex As New Regex("^\d{4}-\d{2}-\d{2}$")
                    If Not regex.IsMatch(newValue.ToString()) Then
                        MessageBox.Show("The date must be in the format yyyy-MM-dd")
                        Return
                    End If
                ElseIf columnHeaderText = "Start Time" OrElse columnHeaderText = "End Time" Then
                    ' Convert the time to the correct format (HH:mm tt)
                    If Not String.IsNullOrEmpty(newValue.ToString()) Then
                        Dim time As DateTime
                        Dim formats() As String = {"h:mm tt", "hh:mm tt"}
                        If DateTime.TryParseExact(newValue.ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, time) Then
                            newValue = time.ToString("HH:mm tt")
                        Else
                            MessageBox.Show("The time must be in the format hh:mm tt")
                            Return
                        End If
                    Else
                        newValue = DBNull.Value
                    End If
                End If
            End If

            ' Get the primary key value of the record being updated
            Dim primaryKeyValue As Object = DataGridView1.Rows(e.RowIndex).Cells("TimeSheetIDDataGridViewTextBoxColumn").Value


            ' Define the file path for the SQLite database
            Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim databaseFilePath As String = Path.Combine(documentsPath, "LocalTimesheet.db")

            ' Create the connection string with the database file path
            Dim connectionString As String = $"Data Source={databaseFilePath};Version=3;"



            ' Create a new SqlConnection object to update the record in the database
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Build the SQL query to update the record
                Dim query As String = "UPDATE TimeSheetT SET [" & columnHeaderText & "] = @NewValue WHERE TimeSheetID = @PrimaryKeyValue"

                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.Add("@NewValue", SqlDbType.VarChar).Value = newValue
                    cmd.Parameters.Add("@PrimaryKeyValue", SqlDbType.Int).Value = primaryKeyValue

                    ' Execute the query
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End If
    End Sub
    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        ' Check if there is at least one selected row in the DataGridView
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Assuming the "Date" column is the first column (index 0)
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim dateValue As Date = CDate(selectedRow.Cells(0).Value)

            ' Set the DateTimePicker1 value to the date from the selected row
            DateTimePicker1.Value = dateValue
        End If
    End Sub


    'Refresh the DataGridView1
    Private Async Sub RefreshData()
        If IsInternetConnected() Then
            Await LoadTimeSheetDataFromDatabase()
            SyncDatabases()
        Else
            LogWarning("No internet connection available. The application will attempt to sync at a later time", Color.Red)
            Await LoadTimeSheetDataFromDatabase()
        End If
    End Sub




    Private Async Sub employeeNameCb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles employeeNameCb.SelectedIndexChanged
        Await LoadTimeSheetDataFromDatabase()
    End Sub

    Private Async Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Await LoadTimeSheetDataFromDatabase()
    End Sub

    Private Sub refreshDataButton_Click(sender As Object, e As EventArgs) Handles refreshDataButton.Click
        RefreshData()

    End Sub

    Private Sub dateUpButton_Click(sender As Object, e As EventArgs) Handles dateUpButton.Click

        Dim currentDate As Date = DateTimePicker1.Value ' Get the current selected date from DateTimePicker1

        Dim newDate As Date = currentDate.AddDays(7) ' Increase the date by 7 days

        DateTimePicker1.Value = newDate  ' Set the new date in DateTimePicker1
    End Sub
    Private Sub dateDownButton_Click(sender As Object, e As EventArgs) Handles dateDownButton.Click

        Dim currentDate As Date = DateTimePicker1.Value ' Get the current selected date from DateTimePicker1

        Dim newDate As Date = currentDate.AddDays(-7) ' Increase the date by 7 days

        DateTimePicker1.Value = newDate  ' Set the new date in DateTimePicker1
    End Sub





    'Check Internet Connection
    Public Function IsInternetConnected() As Boolean
        Try
            Dim ping As New Ping()
            Dim reply As PingReply = ping.Send("www.google.com")

            ' Check if the ping was successful
            If reply.Status = IPStatus.Success Then
                ' Machine is connected to the internet
                Return True
            Else
                ' Machine is not connected to the internet
                Return False
            End If
        Catch ex As Exception
            ' An exception occurred (e.g., no network available)
            Return False
        End Try
    End Function


    'Creates messages in the RichTextBox with specified color
    Private Sub LogWarning(message As String, color As Color)

        WarningLogRichTextBox.AppendText(DateTime.Now & "  -  ") ' Datae and Time Stamp

        WarningLogRichTextBox.SelectionColor = color  ' Set the color for the new text

        WarningLogRichTextBox.AppendText(message & Environment.NewLine)   ' Append the message with the specified color

        ' Reset the color to the default color
        WarningLogRichTextBox.SelectionColor = WarningLogRichTextBox.ForeColor

        ' Scroll to the bottom
        WarningLogRichTextBox.SelectionStart = WarningLogRichTextBox.TextLength
        WarningLogRichTextBox.ScrollToCaret()
    End Sub




#Region "deleteRocordButtonTest_Click" ' Start of the commented-out region




#End Region ' End of the commented-out region

End Class